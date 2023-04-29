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
        active = Physics.Raycast(cam.position, cam.TransformDirection(Vector3.forward), out hit, playerActivateDistance);

        if (active && hit.transform.tag == "3D Glasses Holder")
        {
            hit.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                hit.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                Debug.Log("acquired 3D glasses");
                this.GetComponent<PlayerAttributes>().IsWith3DGlasses = true;
            }
        }
    }
}
