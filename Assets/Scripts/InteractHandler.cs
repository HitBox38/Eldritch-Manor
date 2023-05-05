using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractHandler : MonoBehaviour
{

    private bool is3DGlassesInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter " + other.tag);
        if (other.CompareTag("3D Glasses Holder"))
        {
            is3DGlassesInRange = true;
            Debug.Log("3D object entered range");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Leave " + other.tag);
        if (other.CompareTag("3D Glasses Holder"))
        {
            is3DGlassesInRange = false;
            Debug.Log("3D object exited range");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (is3DGlassesInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("acquired 3D glasses");
        }
    }
}
