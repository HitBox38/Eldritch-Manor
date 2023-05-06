using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    [SerializeField] private bool _isWith3DGlasses = false;

    public bool IsWith3DGlasses
    {
        get => _isWith3DGlasses;
        set => _isWith3DGlasses = value;
    }
}
