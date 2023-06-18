using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassesController : MonoBehaviour
{
    [SerializeField] float initialCountdown = 10f;
    private bool canUse3DGlasses = false;
    private Camera cam;
    private List<GameObject> pictures = new List<GameObject>();
    private bool timerRunning = false;
    private float countdown;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        countdown = initialCountdown;
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

        if (Input.GetKeyDown(KeyCode.R) && canUse3DGlasses && !timerRunning)
        {
            cam.enabled = !cam.enabled;

            foreach (GameObject pic in pictures)
            {
                pic.GetComponent<Collider>().enabled = cam.enabled;
            }
            timerRunning = true;
        }

        if (timerRunning)
        {
            if (countdown > 0)
            {
                countdown -= Time.deltaTime;
            }
            else
            {

                cam.enabled = !cam.enabled;

                foreach (GameObject pic in pictures)
                {
                    pic.GetComponent<Collider>().enabled = cam.enabled;
                }
                countdown = initialCountdown;
                timerRunning = false;
            }
        }
    }
}
