using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsEnabler : MonoBehaviour
{
    [SerializeField] private GameObject affectedObject;
    [SerializeField] private float requiredSpeed = 15f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            affectedObject.GetComponent<Rigidbody>().velocity = Vector3.forward * requiredSpeed;
            Destroy(this, 0.5f);
        }
    }
}
