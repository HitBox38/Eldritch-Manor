using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    [SerializeField] private float topSpeed = 10f;
    [SerializeField] private float maxHeight = 5f;
    [SerializeField] private GameObject nextRoom;

    private Rigidbody rb;
    private bool isDone = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!isDone)
        {
            // Maintain top speed
            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (horizontalVelocity.magnitude < topSpeed && !(rb.velocity.y < 0))
            {
                horizontalVelocity = horizontalVelocity.normalized * topSpeed;
                rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
            }
        }

        // Keep the ball at the same height
        if (transform.position.y > maxHeight && rb.velocity.y < 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Pipe")
        {
            isDone = true;
            rb.velocity = new Vector3(0, 0, 0);
            nextRoom.layer = 8;
        }
        if (other.name == "Player")
        {
            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (horizontalVelocity.magnitude > topSpeed / 2)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
