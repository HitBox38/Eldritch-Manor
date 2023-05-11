using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupItem
{
    void OnPickup();
    void OnDrop();
}

public class PickupItem : MonoBehaviour, IPickupItem
{
    public void OnPickup()
    {
        Debug.Log("Picked up item");
    }

    public void OnDrop()
    {
        transform.parent = null;
    }
}
