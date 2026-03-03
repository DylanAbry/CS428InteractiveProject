using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class playerMovement : MonoBehaviour
{
    public Animator playerAnim;

    public float moveSpeed = 7f;
    public float jumpForce = 25f;
    public float groundCheckDistance = 1f;

    private Rigidbody rb;
    private Vector3 movement;
    private bool isGrounded;

    public Transform cameraTransform;
    public Transform playerSpawn;

    Transform currentLogTransform;
    Quaternion lastLogRotation;

    private PlayerControllerActions controls;
    private Vector2 moveInput;

    void Awake()
    {
        controls = new PlayerControllerActions();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Jump.performed += ctx =>
        {
            if (isGrounded)
            {
                playerAnim.SetTrigger("Jump");
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        };
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + 0.1f);
        playerAnim.SetBool("isGrounded", isGrounded);

        // Animation
        playerAnim.SetBool("isRunning", moveInput.magnitude > 0.1f);

        // Camera-relative movement
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        movement = (forward * moveInput.y + right * moveInput.x).normalized * moveSpeed;

        // Rotate player toward movement direction
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

    void FixedUpdate()
    {
        rb.angularVelocity = Vector3.zero;
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
    }


    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Log"))
        {
            currentLogTransform = collision.transform;
            lastLogRotation = currentLogTransform.rotation;
            // transform.SetParent(currentLogTransform);
            foreach (ContactPoint contact in collision.contacts)
            {
                if(collision.gameObject.name == "Cylinder"){    
                    Debug.Log("contact bounces");
                    Vector3 bounceDirection = contact.normal;
                    rb.AddForce(bounceDirection * 20f, ForceMode.Impulse);
                    // break; // Use first contact point only
                }
            }
        }

        if (collision.gameObject.CompareTag("Lava"))
        {
            this.gameObject.transform.position = playerSpawn.position;
        }
    }

    void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.CompareTag("Log"))
        {
            transform.SetParent(null);
            currentLogTransform = null;
        }
    }
}