using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool activated = false;

    [Header("Saved stats")]
    [SerializeField] private Quaternion rotation;
    [SerializeField] private Vector3 position;
    [SerializeField] private bool isWith3DGlasses;
    [SerializeField] private int _amount = 0;

    public int amount { get { return _amount; } set { _amount = value; } }

    private static GameObject[] CheckPointsList;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        CheckPointsList = GameObject.FindGameObjectsWithTag("Checkpoint");
    }

    public void ActivateCheckPoint(GameObject player)
    {
        foreach (GameObject cp in CheckPointsList)
        {
            cp.GetComponent<Checkpoint>().activated = false;
        }
        activated = true;
        position = player.transform.position;
        rotation = player.transform.rotation;
        isWith3DGlasses = player.GetComponent<PlayerAttributes>().IsWith3DGlasses;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateCheckPoint(other.gameObject);
        }
    }

    public static Vector3 GetActiveCheckPointPosition()
    {
        Vector3 result = new Vector3(0, 0, 0);
        if (CheckPointsList != null)
        {
            foreach (GameObject cp in CheckPointsList)
            {
                if (cp.GetComponent<Checkpoint>().activated)
                {
                    result = cp.GetComponent<Checkpoint>().position;
                    break;
                }
            }
        }
        return result;
    }

    public static bool GetActiveCheckPointIsWith3DGlasses()
    {
        bool result = false;
        if (CheckPointsList != null)
        {
            foreach (GameObject cp in CheckPointsList)
            {
                if (cp.GetComponent<Checkpoint>().activated)
                {
                    result = cp.GetComponent<Checkpoint>().isWith3DGlasses;
                    break;
                }
            }
        }
        return result;
    }

    public static Quaternion GetActiveCheckPointRotation()
    {
        Quaternion result = Quaternion.identity;
        if (CheckPointsList != null)
        {
            foreach (GameObject cp in CheckPointsList)
            {
                if (cp.GetComponent<Checkpoint>().activated)
                {
                    result = cp.GetComponent<Checkpoint>().rotation;
                    break;
                }
            }
        }
        return result;
    }

}
