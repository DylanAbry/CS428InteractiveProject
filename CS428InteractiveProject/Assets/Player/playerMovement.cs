using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class playerMovement : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float jumpForce = 25f;          // Added jump force
    public float groundCheckDistance = 1f; // Ground check distance

    private Rigidbody rb;
    private Vector3 movement;
    private bool isGrounded;

    public Transform cameraTransform;

    void Start()
    {
        Debug.Log("hi");
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.Normalize();
        right.Normalize();

        movement = (forward * moveZ + right * moveX).normalized * moveSpeed;
        movement.y = 0;

        // Jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + 0.1f);

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
        Debug.Log("hi");
        foreach (ContactPoint contact in collision.contacts)
        {
            Vector3 bounceDirection = -contact.normal;
            rb.AddForce(bounceDirection * 5f, ForceMode.Impulse);
            break; // Use first contact point only
        }
    }
}