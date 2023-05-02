using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassesController : MonoBehaviour
{
    private bool canUse3DGlasses = false;
    private Camera cam;
    private List<GameObject> pictures = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();

        GameObject[] pics = GameObject.FindGameObjectsWithTag("pictures");
        foreach (GameObject pic in pics)
        {
            pictures.Add(pic);
        }
    }

    // Update is called once per frame
    void Update()
    {
        canUse3DGlasses = GameObject.Find("Player").GetComponent<PlayerAttributes>().IsWith3DGlasses;

        if (Input.GetKeyDown(KeyCode.F) && canUse3DGlasses)
        {
            cam.enabled = !cam.enabled;

            foreach (GameObject pic in pictures)
            {
                pic.GetComponent<Collider>().enabled = cam.enabled;
            }
        }
    }
}
