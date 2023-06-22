using System;
using System.Collections.Generic;
using UnityEngine;

public class ReachedGoal : MonoBehaviour
{
    public static event Action OnFall;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Domino_4-2")
        {
            OnFall?.Invoke();
        }
    }
}
