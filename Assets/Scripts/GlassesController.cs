using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassesController : MonoBehaviour
{
    [SerializeField] private float initialCountdown = 10f;
    [SerializeField] private RectTransform glassesEffect;
    [SerializeField] private float glassesSpeed = 2f;
    private bool canUse3DGlasses = false;
    private Camera cam;
    private List<GameObject> pictures = new List<GameObject>();
    private float countdown;
    private Vector3 startPosition = new Vector3(0, Screen.height, 0);
    private Vector3 endPosition = new Vector3(0, 0, 0);

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

    void Update()
    {
        canUse3DGlasses = GameObject.Find("Player").GetComponent<PlayerAttributes>().IsWith3DGlasses;

        if (Input.GetKeyDown(KeyCode.R) && canUse3DGlasses && cam.enabled)
        {
            StartCoroutine(UseGlasses());
        }
    }

    IEnumerator UseGlasses()
    {
        float lerpTime = 0;
        cam.enabled = false;
        while (lerpTime < 1)
        {
            lerpTime += Time.deltaTime * glassesSpeed;
            glassesEffect.localPosition = Vector3.Lerp(startPosition, endPosition, lerpTime);
            yield return null;
        }

        yield return new WaitForSeconds(countdown);

        lerpTime = 0;
        while (lerpTime < 1)
        {
            lerpTime += Time.deltaTime * glassesSpeed;
            glassesEffect.localPosition = Vector3.Lerp(endPosition, startPosition, lerpTime);
            yield return null;
        }

        cam.enabled = true;
    }
}
