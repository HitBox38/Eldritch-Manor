using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float forceAmount = 10f;
    [SerializeField] private KeyCode pushInput = KeyCode.Mouse0;
    [SerializeField] private KeyCode pullInput = KeyCode.Mouse1;
    public float slowDownFactor = 0.5f;
    public float followDistance = 1f;
    public float followSpeed = 5f;

    private Rigidbody rb;
    private PlayerMovement playerMovement;
    private float initialPlayerSpeed;
    private bool isPushing = false;
    private bool isPulling = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = player.GetComponent<PlayerMovement>();
        initialPlayerSpeed = playerMovement.CurrentSpeed;
    }

    private void FixedUpdate()
    {
        if (isPulling)
        {
            // TODO: fix the player going up the cart
            Vector3 targetPosition = player.transform.position + player.transform.forward * followDistance;
            targetPosition.y = transform.position.y;
            rb.MovePosition(Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.fixedDeltaTime));
        }
        if (isPushing)
        {
            Vector3 forceDirection = (transform.position - player.transform.position).normalized;
            rb.AddForce(forceDirection * forceAmount, ForceMode.Force);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKey(pullInput))
            {
                isPulling = true;
                isPushing = false;
                rb.isKinematic = isPulling;
                playerMovement.CurrentSpeed = isPulling ? initialPlayerSpeed * slowDownFactor : initialPlayerSpeed;
            }
            if (Input.GetKey(pushInput))
            {
                isPushing = true;
                isPulling = false;
                rb.isKinematic = false;
                playerMovement.CurrentSpeed = isPulling ? initialPlayerSpeed * slowDownFactor : initialPlayerSpeed;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPushing = false;
            isPulling = false;
            rb.isKinematic = false;
            playerMovement.CurrentSpeed = initialPlayerSpeed;
        }
    }
}
