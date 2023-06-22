using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class FadeImageController : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private GameObject canvas;

    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
        DontDestroyOnLoad(canvas);
    }
    void Start()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }

    public IEnumerator FadeInAndOut(Func<IEnumerator> coroutineToRunWhileBlack)
    {
        // Fade in
        float startTime = Time.time;
        while (Time.time - startTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, (Time.time - startTime) / fadeDuration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
        }

        // Start the WaitForSceneLoad coroutine while the screen is black
        yield return StartCoroutine(coroutineToRunWhileBlack());

        // Wait for 0.5 seconds
        yield return new WaitForSeconds(0.5f);

        // Fade out
        startTime = Time.time;
        while (Time.time - startTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, (Time.time - startTime) / fadeDuration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
        }
    }
}
