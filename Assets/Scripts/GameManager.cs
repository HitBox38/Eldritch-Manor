using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StartCoroutine(WaitForSceneLoad());
    }

    private IEnumerator WaitForSceneLoad()
    {
        yield return new WaitForSeconds(0.1f); // Wait for scene to fully load
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = Checkpoint.GetActiveCheckPointPosition();
        player.transform.rotation = Checkpoint.GetActiveCheckPointRotation();
        player.GetComponent<CharacterController>().enabled = true;
        Debug.Log(Checkpoint.GetActiveCheckPointIsWith3DGlasses());
        player.GetComponent<PlayerAttributes>().IsWith3DGlasses = Checkpoint.GetActiveCheckPointIsWith3DGlasses();
    }

}
