using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    [SerializeField] private GameObject linkedObject;
    [SerializeField] private GameObject wheel;

    private enum Axis
    {
        X,
        Y,
        Z
    }

    [SerializeField] private Axis rotationAxis = Axis.Y;

    public void Rotate(float amount)
    {
        if (wheel.activeSelf)
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


            // Calculate the center of the object's bounds
            Bounds bounds = new Bounds(transform.position, Vector3.zero);
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                bounds.Encapsulate(renderer.bounds);
            }
            Vector3 center = bounds.center;

            transform.Rotate(Vector3.up, amount * speed * Time.deltaTime);
            linkedObject.transform.RotateAround(center, axis, amount * speed * Time.deltaTime);
        }
    }
}
