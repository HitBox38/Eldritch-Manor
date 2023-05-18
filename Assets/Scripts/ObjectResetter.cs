using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectResetter : MonoBehaviour
{
    [SerializeField] private Transform Ball;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = Ball.position;
        originalRotation = Ball.rotation;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Ball"))
        {
            other.collider.transform.position = originalPosition;
            other.collider.transform.rotation = originalRotation;

            // Wait a few seconds and then push the object
            StartCoroutine(PushAfterDelay(other.collider.GetComponent<Rigidbody>(), 0));
        }
    }

    IEnumerator PushAfterDelay(Rigidbody objectRigidbody, float delay)
    {
        objectRigidbody.velocity = Vector3.zero;
        yield return new WaitForSeconds(delay);
        objectRigidbody.velocity = Vector3.forward * 75;
    }
}
