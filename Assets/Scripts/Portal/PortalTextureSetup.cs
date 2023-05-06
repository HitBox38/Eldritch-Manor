using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
    [SerializeField] private Camera camA;
    [SerializeField] private Material camMatA;
    [SerializeField] private Camera camB;
    [SerializeField] private Material camMatB;

    // Start is called before the first frame update
    void Start()
    {
        if (camA.targetTexture != null)
        {
            camA.targetTexture.Release();
        }
        camA.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        camMatA.mainTexture = camA.targetTexture;

        if (camB.targetTexture != null)
        {
            camB.targetTexture.Release();
        }
        camB.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        camMatB.mainTexture = camB.targetTexture;
    }


}
