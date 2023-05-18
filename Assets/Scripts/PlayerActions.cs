using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float playerActivateDistance;
    [SerializeField] private Transform hand;
    [SerializeField] private HangedDomino redDomino;
    [SerializeField] private int numRays = 10;
    [SerializeField] private float coneAngle = 45f;
    [SerializeField] private float grabDuration = 0.5f;

    private bool active = false;
    [SerializeField] private GameObject currentPickupItem;

    private void Update()
    {
        for (int i = 0; i < numRays; i++)
        {
            float angle = i * (coneAngle / (numRays - 1)) - (coneAngle / 2f);
            Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * cam.TransformDirection(Vector3.forward);
            RaycastHit hit;
            active = Physics.Raycast(cam.position, rayDirection, out hit, playerActivateDistance);

            // Handle your raycast logic here, e.g., interacting with objects
            if (active && hit.transform.tag == "3D Glasses Holder")
            {
                hit.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F) && !GetComponent<PlayerAttributes>().IsWith3DGlasses)
                {
                    hit.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                    Debug.Log("acquired 3D glasses");
                    GetComponent<PlayerAttributes>().IsWith3DGlasses = true;
                }
            }
            else if (active && hit.transform.tag == "PickupItem")
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GameObject pickedOne = hit.transform.gameObject;
                    GameObject pickupItem = HasComponent(pickedOne, "IPickupItem") ? pickedOne : null;
                    if (pickupItem != null)
                    {
                        currentPickupItem = pickupItem;

                        pickedOne.transform.parent = hand;
                        StartCoroutine(MoveToHand(pickedOne.transform));
                    }
                }
            }
            else if (active && FindChildWithTag(hit.transform.gameObject, "PickupItem") != null)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GameObject pickedOne = FindChildWithTag(hit.transform.gameObject, "PickupItem");
                    if (pickedOne != null)
                    {
                        currentPickupItem = pickedOne;

                        pickedOne.transform.parent = hand;
                        StartCoroutine(MoveToHand(pickedOne.transform));
                    }
                }
            }
            else if (active && hit.transform.tag == "Net")
            {
                if (Input.GetKeyDown(KeyCode.F) && currentPickupItem != null)
                {
                    StartCoroutine(DropAndRemove(hit.transform));

                    currentPickupItem.GetComponent<IPickupItem>().OnDrop();
                    Destroy(currentPickupItem);
                    currentPickupItem = null;
                }
            }
            else if (active && hit.transform.tag == "Rope" && currentPickupItem != null)
            {
                if (Input.GetKeyDown(KeyCode.F) && currentPickupItem.name == "Key")
                {
                    Destroy(hit.transform.gameObject);
                    redDomino.IsHanged = false;

                    currentPickupItem.GetComponent<IPickupItem>().OnDrop();
                    Destroy(currentPickupItem);
                    currentPickupItem = null;
                }
            }
            else if (active && hit.transform.tag == "Rotator" && currentPickupItem != null)
            {
                if (Input.GetKeyDown(KeyCode.F) && currentPickupItem.name == "Wheel")
                {
                    hit.transform.GetChild(0).gameObject.SetActive(true);

                    currentPickupItem.GetComponent<IPickupItem>().OnDrop();
                    Destroy(currentPickupItem);
                    currentPickupItem = null;
                }
            }
            else if (active && FindChildWithTag(hit.transform.gameObject, "Button") != null)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GameObject button = FindChildWithTag(hit.transform.gameObject, "Button");
                    Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, button.transform.position.z);
                    Vector3 direction = (center - button.transform.position).normalized;
                    float scale = 0.1f; // Adjust this value to control how far inward you want the button to move
                    Vector3 targetPosition = button.transform.position + direction * scale;

                    button.transform.position = Vector3.Lerp(button.transform.position, targetPosition, Time.deltaTime);

                    GameObject ball = FindChildWithTag(hit.transform.gameObject, "Ball_Room_2");
                    ball.GetComponent<BallController>().IsDone = false;
                }
            }
        }
    }

    private IEnumerator MoveToHand(Transform item)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = item.localPosition;

        while (elapsedTime < grabDuration)
        {
            item.localPosition = Vector3.Lerp(initialPosition, Vector3.zero, elapsedTime / grabDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        item.localPosition = Vector3.zero;
    }

    private IEnumerator DropAndRemove(Transform item)
    {
        float dropDuration = 5f;
        float elapsedTime = 0f;
        Vector3 dropPosition = item.localPosition + new Vector3(0, -14f, 0);

        while (elapsedTime < dropDuration)
        {
            item.localPosition = Vector3.Lerp(item.localPosition, dropPosition, elapsedTime / dropDuration);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForEndOfFrame();
        }

        Destroy(item.gameObject);
    }

    private GameObject FindChildWithTag(GameObject parentGameObject, string desiredTag)
    {
        for (int i = 0; i < parentGameObject.transform.childCount; i++)
        {
            Transform child = parentGameObject.transform.GetChild(i);
            if (child.CompareTag(desiredTag))
            {
                return child.gameObject;
            }
        }
        return null;
    }

    public bool HasComponent(GameObject gameObject, string componentName)
    {
        if (gameObject == null)
        {
            Debug.LogWarning("The game object is null.");
            return false;
        }

        Type componentType = Type.GetType(componentName);
        if (componentType == null)
        {
            Debug.LogWarning("Component with the given name not found: " + componentName);
            return false;
        }

        Component component = gameObject.GetComponent(componentType);
        return component != null;
    }


    private void OnDrawGizmos()
    {
        if (cam == null) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < numRays; i++)
        {
            float angle = i * (coneAngle / (numRays - 1)) - (coneAngle / 2f);
            Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * cam.TransformDirection(Vector3.forward);
            Gizmos.DrawRay(cam.position, rayDirection * playerActivateDistance);
        }
    }

}
