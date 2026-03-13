using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class playerMovement : MonoBehaviour
{
    [Header("References")]
    public UIManager uiScript;
    public Animator playerAnim;
    public Animator pitDoor;
    public Transform cameraTransform;
    public Transform playerSpawn;

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

    void Awake()
    {
        controls = new PlayerControllerActions();

        controls.Player.Move.performed += ctx =>
        {
            if (inputEnabled)
                moveInput = ctx.ReadValue<Vector2>();
        };

        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        if (SceneManager.GetActiveScene().name == "SenseiScene") controls.Player.Pause.performed += ctx => uiScript.PauseManager();

        controls.Player.Jump.performed += ctx =>
        {
            if (SceneManager.GetActiveScene().name == "SenseiScene")
            {
                if (!inputEnabled)
                {
                    Begin();
                    return;
                }
            }
            TryJump();
        };
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (SceneManager.GetActiveScene().name != "SenseiScene")
        {
            inputEnabled = true;
        }
    }

    void Update()
    {
        HandleMovement();
        HandleAnimation();
    }

    void FixedUpdate()
    {
        rb.angularVelocity = Vector3.zero;
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
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
        if (collision.gameObject.tag == "Log")
        {
            currentLogTransform = collision.transform;

            foreach (ContactPoint contact in collision.contacts)
            {
                if (collision.gameObject.name == "Cylinder")
                {
                    Vector3 bounceDirection = contact.normal;
                    rb.AddForce(bounceDirection * 20f, ForceMode.Impulse);
                }
            }
        }

        if (collision.gameObject.tag == "Key")
        {
            collision.gameObject.SetActive(false);
            pitDoor.Play("OpenDoor");
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpLocked = false;
            playerAnim.SetBool("isGrounded", true);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Log")
        {
            currentLogTransform = null;
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
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

            
            transform.position = playerSpawn.position;

            
            transform.rotation = playerSpawn.rotation;
        }

        if (collider.gameObject.tag == "Pool")
        {
            StartCoroutine(WinSequence());
        }
    }

    private IEnumerator WinSequence()
    {
        winText.SetActive(true);
        yield return new WaitForSeconds(3f);
        winText.SetActive(false);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;


        transform.position = playerSpawn.position;


        transform.rotation = playerSpawn.rotation;
    }

    void Begin()
    {
        if (!inputEnabled)
        {
            inputEnabled = true;
            startText.SetActive(false);
            return;
        }
    }
}