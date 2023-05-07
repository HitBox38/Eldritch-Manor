using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float playerActivateDistance;

    private bool active = false;

    private void Update()
    {
        RaycastHit hit;
        Vector3 rayDirection = cam.TransformDirection(Vector3.forward);
        active = Physics.Raycast(cam.position, rayDirection, out hit, playerActivateDistance);

        // Debug.DrawRay(cam.position, rayDirection * playerActivateDistance, Color.black);

        if (active && hit.transform.tag == "3D Glasses Holder")
        {
            hit.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

            if (Input.GetKeyDown(KeyCode.F) && !GetComponent<PlayerAttributes>().IsWith3DGlasses)
            {
                hit.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                Debug.Log("acquired 3D glasses");
                GetComponent<PlayerAttributes>().IsWith3DGlasses = true;
            }
        }
    }
}
