using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float playerActivateDistance;
    [SerializeField] private Transform hand;
    [SerializeField] private int numRays = 10;
    [SerializeField] private float coneAngle = 45f;
    [SerializeField] private float grabDuration = 0.5f;

    private bool active = false;
    private IPickupItem currentPickupItem;

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
                    IPickupItem pickupItem = hit.collider.GetComponent<IPickupItem>();
                    if (pickupItem != null)
                    {
                        currentPickupItem = pickupItem;

                        hit.transform.parent = hand;
                        StartCoroutine(MoveToHand(hit.transform));
                    }
                }
            }
            else if (active && hit.transform.tag == "Net")
            {
                if (Input.GetKeyDown(KeyCode.F) && currentPickupItem != null)
                {
                    StartCoroutine(DropAndRemove(hit.transform));
                    currentPickupItem.OnDrop();
                    Destroy(((Component)currentPickupItem).gameObject);
                    currentPickupItem = null;
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
        float dropDuration = 0.2f;
        float elapsedTime = 0f;
        Vector3 initialPosition = item.localPosition;
        Vector3 dropPosition = initialPosition + new Vector3(0, -2f, 0);

        while (elapsedTime < dropDuration)
        {
            item.localPosition = Vector3.Lerp(initialPosition, dropPosition, elapsedTime / dropDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(item.gameObject);
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
