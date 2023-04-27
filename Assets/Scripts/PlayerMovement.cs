using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController cc;

    [Header("Movement Properties")]
    [Tooltip("Movement Speed")][SerializeField] private float speed = 6f;
    [Tooltip("Sprint Modifier")][SerializeField] private float sprint = 2f;
    [Tooltip("Crouch Speed")][SerializeField] private float crouchSpeed = 3f;
    private float currentSpeed;
    [SerializeField] private float gravity = -19.62f;
    [SerializeField] private float jumpHeight = 3f;

    [Header("Crouch Properties")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float crouchTime = 0.5f;
    private float currentHeight;
    private bool isCrouching;
    private Coroutine crouchCoroutine;

    [Header("Slide Properties")]
    [SerializeField] private float slideSpeed = 10f;
    [SerializeField] private float slideDuration = 1f;
    private bool isSliding;
    private float slideStartTime;

    [Header("Ground Check Properties")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;


    private Vector3 velocity;
    private bool isGrounded;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        currentHeight = cc.height;
        currentSpeed = speed;
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
            currentSpeed = currentSpeed * sprint;
        }
        if (Input.GetButtonUp("Sprint"))
        {
            currentSpeed = currentSpeed / sprint;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            if (crouchCoroutine != null)
            {
                StopCoroutine(crouchCoroutine);
            }

            isCrouching = !isCrouching;
            crouchCoroutine = StartCoroutine(Crouch());
        }

        if (Input.GetButton("Crouch") && !isSliding && isGrounded && Input.GetButton("Sprint"))
        {
            isSliding = true;
            slideStartTime = Time.time;
            StartCoroutine(Crouch());
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        cc.Move(move * currentSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        cc.Move(velocity * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (isSliding)
        {
            cc.Move(transform.forward * slideSpeed * Time.fixedDeltaTime);

            if (Time.time - slideStartTime > slideDuration)
            {
                isSliding = false;
                StartCoroutine(Crouch());

            }
        }
    }

    private IEnumerator Crouch()
    {
        float timeElapsed = 0;
        float startHeight = cc.height;
        Vector3 cameraStartPosition = Camera.main.transform.localPosition;

        while (timeElapsed < crouchTime)
        {
            if (isCrouching)
            {
                currentSpeed = Mathf.Lerp(speed, crouchSpeed, timeElapsed / crouchTime);
                currentHeight = Mathf.Lerp(startHeight, crouchHeight, timeElapsed / crouchTime);
                Camera.main.transform.localPosition = new Vector3(cameraStartPosition.x, cameraStartPosition.y * currentHeight / startHeight, cameraStartPosition.z);
            }
            else if (!Physics.Raycast(transform.position, Vector3.up, standingHeight))
            {
                currentSpeed = Mathf.Lerp(crouchSpeed, speed, timeElapsed / crouchTime);
                currentHeight = Mathf.Lerp(startHeight, standingHeight, timeElapsed / crouchTime);
                Camera.main.transform.localPosition = new Vector3(cameraStartPosition.x, cameraStartPosition.y * currentHeight / startHeight, cameraStartPosition.z);
            }
            cc.height = currentHeight;

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}