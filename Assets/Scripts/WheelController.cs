using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    [SerializeField] private GameObject linkedObject;

    private enum Axis
    {
        X,
        Y,
        Z
    }

    [SerializeField] private Axis rotationAxis = Axis.Y;

    public void Rotate(float amount)
    {
        Vector3 axis;
        switch (rotationAxis)
        {
            case Axis.X:
                axis = Vector3.right;
                break;

            case Axis.Y:
                axis = Vector3.up;
                break;

            case Axis.Z:
                axis = Vector3.forward;
                break;

            default:
                axis = Vector3.up;
                break;
        }

        transform.Rotate(Vector3.up, amount * speed * Time.deltaTime);
        linkedObject.transform.Rotate(axis, amount * speed * Time.deltaTime);
    }
}
