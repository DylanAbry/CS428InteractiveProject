using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class playerMovement : MonoBehaviour
{
    [Header("References")]
    public UIManager uiScript;
    public Animator playerAnim;
    public Animator pitDoor;
    public Transform cameraTransform;

    public Transform[] playerSpawns;
    public Transform recentCheckpoint;
    int spawnCounter;
    public GameObject[] checkpointCollisions;
    public GameObject recentCollision;

    public GameObject winText;
    public GameObject startText;

    [Header("Movement")]
    public float moveSpeed = 7f;
    public float jumpForce = 25f;

    [Header("Ground Check")]
    public float groundCheckDistance = 1.1f;
    public LayerMask groundLayer;

    Rigidbody rb;
    Vector3 movement;

    bool isGrounded;
    bool jumpLocked;

    PlayerControllerActions controls;
    Vector2 moveInput;

    Transform currentLogTransform;

    bool inputEnabled = false;

    public bool gameActive;


    Transform currentPlatform;
    Vector3 lastPlatformPos;
    Quaternion lastPlatformRot;

    Vector3 platformDelta = Vector3.zero;

    public GameObject pitKey;
    public Timer timeScript;
    public GameObject timePanel;
    public GameObject recordTimePanel;

    public CinemachineFreeLook freelookCam;


    void Awake()
    {
        controls = new PlayerControllerActions();
        freelookCam.enabled = false;

        controls.Player.Move.performed += ctx =>
        {
            if (inputEnabled)
                moveInput = ctx.ReadValue<Vector2>();
        };

        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        if (SceneManager.GetActiveScene().name == "SenseiScene") controls.Player.Pause.performed += ctx => uiScript.PauseManager();

        controls.Player.Jump.performed += ctx =>
        {
            if (!inputEnabled)
            {
                Begin();
                return;
            }
            TryJump();
        };
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        gameActive = false;
        spawnCounter = 0;
        recentCheckpoint = playerSpawns[spawnCounter];
        recentCollision = checkpointCollisions[spawnCounter];
        timePanel.SetActive(false);
        recordTimePanel.SetActive(false);     
    }

    void Update()
    {
        if (currentPlatform != null)
        {
            Vector3 deltaPos = currentPlatform.position - lastPlatformPos;
            Quaternion deltaRot = currentPlatform.rotation * Quaternion.Inverse(lastPlatformRot);

            Vector3 platformCenter = currentPlatform.position;
            Vector3 offset = rb.position - platformCenter;

            offset = deltaRot * offset;
            offset = offset.normalized * offset.magnitude;

            platformDelta = (platformCenter + offset + deltaPos) - rb.position;

            lastPlatformPos = currentPlatform.position;
            lastPlatformRot = currentPlatform.rotation;
        }
        else
        {
            platformDelta = Vector3.zero;
        }

        HandleMovement();
        HandleAnimation();
    }

    void FixedUpdate()
    {
        rb.angularVelocity = Vector3.zero;

        Vector3 finalMove = movement + platformDelta / Time.fixedDeltaTime;

        rb.MovePosition(rb.position + finalMove * Time.fixedDeltaTime);
    }

    void TryJump()
    {
        if (!isGrounded || jumpLocked)
            return;

        isGrounded = false;
        jumpLocked = true;

        playerAnim.SetTrigger("Jump");
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void HandleMovement()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        movement = (forward * moveInput.y + right * moveInput.x).normalized * moveSpeed;

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                10f * Time.deltaTime
            );
        }
    }

    void HandleAnimation()
    {
        playerAnim.SetBool("isRunning", moveInput.magnitude > 0.1f);
    }

    void OnApplicationFocus(bool focus)
    {
        Cursor.lockState = focus ? CursorLockMode.Locked : CursorLockMode.None;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Log"))
        {
            currentPlatform = collision.transform;
            lastPlatformPos = currentPlatform.position;
            lastPlatformRot = currentPlatform.rotation;
        }

        if (collision.gameObject.tag == "Key")
        {
            collision.gameObject.SetActive(false);
            pitDoor.Play("OpenDoor");
        }

        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            if (System.Array.IndexOf(checkpointCollisions, collision.gameObject) > System.Array.IndexOf(checkpointCollisions, recentCollision))
            {
                spawnCounter++;
                recentCollision = checkpointCollisions[spawnCounter];
            }
            else
            {

            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Log") || collision.gameObject.CompareTag("Checkpoint"))
        {
            isGrounded = true;
            jumpLocked = false;
            playerAnim.SetBool("isGrounded", true);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            playerAnim.SetBool("isGrounded", false);
        }

        if (collision.gameObject.CompareTag("Log"))
        {
            currentPlatform = null;
            isGrounded = false;
            playerAnim.SetBool("isGrounded", false);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Lava")
        {
            
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            timeScript.timer += 10f;
            transform.position = playerSpawns[spawnCounter].position;

            
            transform.rotation = playerSpawns[spawnCounter].rotation;
        }

        if (collider.gameObject.tag == "Pool")
        {
            StartCoroutine(WinSequence());
        }

        if (collider.gameObject.tag == "DragonInterior")
        {
            SceneManager.LoadScene("SenseiScene");
        }
    }

    private IEnumerator WinSequence()
    {
        winText.SetActive(true);
        gameActive = false;
        timeScript.SaveBestTime();
        yield return new WaitForSeconds(3f);
        winText.SetActive(false);
        pitDoor.Play("Default");
        pitKey.SetActive(true);


        RestartCourse();
    }

    void Begin()
    {
        if (!inputEnabled)
        {
            inputEnabled = true;
            gameActive = true;

            if (SceneManager.GetActiveScene().name == "SenseiScene")
            {
                timePanel.SetActive(true);
                recordTimePanel.SetActive(true);
            }

            freelookCam.enabled = true;
            startText.SetActive(false);
            return;
        }
    }

    public void RestartCourse()
    {
        if (uiScript.pauseScreen.activeInHierarchy) uiScript.pauseScreen.SetActive(false);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        spawnCounter = 0;
        transform.position = playerSpawns[spawnCounter].position;
        transform.rotation = playerSpawns[spawnCounter].rotation;
        recentCollision = checkpointCollisions[spawnCounter];
        timeScript.timer = 0f;
        gameActive = true;
    }
}