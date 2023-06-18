using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangedDomino : MonoBehaviour
{
    [SerializeField] private bool isHanged = true;

    public bool IsHanged
    {
        get { return isHanged; }
        set { isHanged = value; }
    }

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (isHanged)
        {
            rb.Sleep();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHanged)
        {
            rb.AddForce(Vector3.down * 9.81f * 2);
        }
        else
        {
            rb.Sleep();
        }
    }
}
