using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private float startTime;

    public static event Action<float> OnFinish;

    private void OnEnable()
    {
        ReachedGoal.OnFall += StopTimer;
    }

    private void OnDisable()
    {
        ReachedGoal.OnFall -= StopTimer;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            startTime = Time.time; // Start the timer
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetScene()
    {
        FadeImageController fadeImageController = FindObjectOfType<FadeImageController>();
        StartCoroutine(fadeImageController.FadeInAndOut(WaitForSceneLoad));
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator WaitForSceneLoad()
    {
        yield return new WaitForSeconds(0.1f); // Wait for scene to fully load
        RestartLevel();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = Checkpoint.GetActiveCheckPointPosition();
        player.transform.rotation = Checkpoint.GetActiveCheckPointRotation();
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<PlayerAttributes>().IsWith3DGlasses = Checkpoint.GetActiveCheckPointIsWith3DGlasses();
    }
    public void StopTimer()
    {
        float elapsedTime = Time.time - startTime;
        OnFinish?.Invoke(elapsedTime);
    }
}
