using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class playerMovement : MonoBehaviour
{
    public Animator playerAnim;

    public float moveSpeed = 7f;
    public float jumpForce = 25f;          // Added jump force
    public float groundCheckDistance = 1f; // Ground check distance

    private Rigidbody rb;
    private Vector3 movement;
    private bool isGrounded;

    private Transform currentLogTransform;
    private Quaternion lastLogRotation;
    private float logAngularVelocity;
    public float flingThreshold = 40f;


    public Transform cameraTransform;
    public Transform playerSpawn;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Animation control
        playerAnim.SetBool("isRunning", Mathf.Abs(moveZ) > 0.1f || Mathf.Abs(moveX) > 0.1f);

        //Ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + 0.1f);
        playerAnim.SetBool("isGrounded", isGrounded);

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        movement = (forward * moveZ + right * moveX).normalized * moveSpeed;

        // Adjust camera if needed
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                10f * Time.deltaTime
            );
        }

        // Jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerAnim.SetTrigger("Jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        rb.angularVelocity = Vector3.zero;

        // Move using physics
        Vector3 newPosition = rb.position + movement * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
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