using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController cc;

    [Header("Movement Properties")]
    [Tooltip("Movement Speed")][SerializeField] private float speed = 6f;
    [Tooltip("Sprint Modifier")][SerializeField] private float sprint = 2f;
    private float _currentSpeed;
    [SerializeField] private float gravity = -19.62f;
    [SerializeField] private float jumpHeight = 3f;

    [Header("Ground Check Properties")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    public float CurrentSpeed
    {
        get => _currentSpeed;
        set => _currentSpeed = value;
    }

    private Vector3 velocity;
    private bool isGrounded;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        CurrentSpeed = speed;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Sprint"))
        {
            CurrentSpeed = CurrentSpeed * sprint;
        }
        if (Input.GetButtonUp("Sprint"))
        {
            CurrentSpeed = CurrentSpeed / sprint;
        }

        if (cc.enabled)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            cc.Move(move * CurrentSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        cc.Move(velocity * Time.deltaTime);
    }

}