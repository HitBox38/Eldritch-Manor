using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassesController : MonoBehaviour
{
    private Camera cam;
    private List<GameObject> pictures = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        // Debug.Log()
        GameObject[] go = GameObject.FindGameObjectsWithTag("pictures");
        foreach (GameObject item in go)
        {
            pictures.Add(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            cam.enabled = !cam.enabled;
            foreach (GameObject item in pictures)
            {
                item.GetComponent<Collider>().enabled = cam.enabled;
            }
        }
    }
}
