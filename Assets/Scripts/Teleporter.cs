using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform otherPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            // Access the Character Controller component
            CharacterController characterController = other.GetComponent<CharacterController>();

            // Disable the Character Controller before teleporting
            characterController.enabled = false;

            // Update the player's position
            other.transform.position = otherPosition.position;

            // Re-enable the Character Controller after teleporting
            characterController.enabled = true;
        }
    }
}
