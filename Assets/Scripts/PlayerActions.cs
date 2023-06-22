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
    [SerializeField] private float grabDuration = 0.5f;
    [SerializeField] private GameObject currentPickupItem;
    private WheelController currentWheel;
    private bool isButtonMoving = false;

    public static event Action<string> OnCloseToInteract;
    public static event Action OnLeftFromInteract;
    // TODO: add more events when adding the sound manager!


    private void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(cam.position.x, cam.position.y - 1, cam.position.z), playerActivateDistance);

        foreach (Collider hit in hitColliders)
        {
            if (hit.transform.tag == "3D Glasses Holder" && !GetComponent<PlayerAttributes>().IsWith3DGlasses)
            {
                // hit.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                OnCloseToInteract?.Invoke("Press F to acquire the 3D glasses");
                if (Input.GetKeyDown(KeyCode.F))
                {
                    OnLeftFromInteract?.Invoke();
                    hit.transform.GetChild(0).gameObject.SetActive(false);
                    GetComponent<PlayerAttributes>().IsWith3DGlasses = true;
                }
            }
            else if (hit.transform.tag == "PickupItem" && hit.GetComponentInParent<Transform>().name != "Hand")
            {
                OnCloseToInteract?.Invoke("Press F to pick up " + hit.name);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GameObject pickedOne = hit.transform.gameObject;
                    GameObject pickupItem = HasComponent(pickedOne, "IPickupItem") ? pickedOne : null;
                    if (pickupItem != null)
                    {
                        OnLeftFromInteract?.Invoke();
                        currentPickupItem = pickupItem;

                        pickedOne.transform.parent = hand;
                        pickedOne.tag = "Player";
                        StartCoroutine(MoveToHand(pickedOne.transform));
                    }
                }
            }
            else if (FindChildWithTag(hit.transform.gameObject, "PickupItem") != null && hit.name == "Hand")
            {
                OnCloseToInteract?.Invoke("Press F to pick up " + hit.name);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GameObject pickedOne = FindChildWithTag(hit.transform.gameObject, "PickupItem");
                    if (pickedOne != null)
                    {
                        OnLeftFromInteract?.Invoke();
                        currentPickupItem = pickedOne;

                        pickedOne.transform.parent = hand;
                        StartCoroutine(MoveToHand(pickedOne.transform));
                    }
                }
            }
            else if (hit.transform.tag == "Net" && currentPickupItem != null)
            {
                OnCloseToInteract?.Invoke("Press F to cut the net");
                if (Input.GetKeyDown(KeyCode.F))
                {
                    OnLeftFromInteract?.Invoke();
                    StartCoroutine(DropAndRemove(hit.transform));

                    currentPickupItem.GetComponent<IPickupItem>().OnDrop();
                    Destroy(currentPickupItem);
                    currentPickupItem = null;
                }
            }
            else if (hit.transform.tag == "Rope" && currentPickupItem != null && currentPickupItem.name == "Key")
            {
                OnCloseToInteract?.Invoke("Press F to cut the net");
                if (Input.GetKeyDown(KeyCode.F))
                {
                    OnLeftFromInteract?.Invoke();
                    Destroy(hit.transform.gameObject);
                    redDomino.IsHanged = false;

                    currentPickupItem.GetComponent<IPickupItem>().OnDrop();
                    Destroy(currentPickupItem);
                    currentPickupItem = null;
                }
            }
            else if (hit.transform.tag == "Rotator")
            {
                if (currentPickupItem != null)
                {
                    OnCloseToInteract?.Invoke("Press F to insert the wheel");
                    if (Input.GetKeyDown(KeyCode.F) && currentPickupItem.name == "Wheel")
                    {
                        OnLeftFromInteract?.Invoke();
                        hit.transform.GetChild(0).gameObject.SetActive(true);

                        currentPickupItem.GetComponent<IPickupItem>().OnDrop();
                        Destroy(currentPickupItem);
                        currentPickupItem = null;
                    }
                }
                else if (currentWheel == null)
                {
                    OnCloseToInteract?.Invoke("Press F to use the wheel");
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        OnLeftFromInteract?.Invoke();
                        WheelController wheel = hit.gameObject.GetComponentInParent<WheelController>();
                        if (wheel != null)
                        {
                            currentWheel = wheel;
                            GetComponent<CharacterController>().enabled = false;
                            OnCloseToInteract?.Invoke("Hold A or D to rotate. Press F to leave the wheel.");
                        }
                    }
                }
                else if (currentWheel != null)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        currentWheel = null;
                        GetComponent<CharacterController>().enabled = true;
                        OnLeftFromInteract?.Invoke();
                    }
                }
            }
            else if (FindChildWithTag(hit.transform.gameObject, "Button") != null)
            {
                OnCloseToInteract?.Invoke("Press F to press the button");
                if (Input.GetKeyDown(KeyCode.F) && !isButtonMoving)
                {
                    GameObject button = FindChildWithTag(hit.transform.gameObject, "Button");
                    Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, button.transform.position.z);
                    Vector3 direction = (center - button.transform.position).normalized;
                    float scale = -0.1f; // Adjust this value to control how far inward you want the button to move
                    Vector3 targetPosition = button.transform.position + direction * scale;

                    OnLeftFromInteract?.Invoke();

                    StartCoroutine(MoveButton(button, targetPosition, 0.25f)); // Adjust the duration as needed

                    GameObject ball = FindChildWithTag(hit.transform.gameObject, "Ball_Room_2");
                    ball.GetComponent<BallController>().IsDone = false;
                }
            }
        }
        if (hitColliders.Length <= 2)
        {
            OnLeftFromInteract?.Invoke();
        }

        if (Input.GetKey(KeyCode.A) && currentWheel != null)
        {
            currentWheel.Rotate(-1f);
        }
        if (Input.GetKey(KeyCode.D) && currentWheel != null)
        {
            currentWheel.Rotate(1f);
        }
        if (Input.GetKey(KeyCode.G) && currentWheel != null && !GetComponent<CharacterController>().enabled)
        {
            currentWheel = null;
            GetComponent<CharacterController>().enabled = true;
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

    IEnumerator MoveButton(GameObject button, Vector3 targetPosition, float duration)
    {
        isButtonMoving = true;
        Vector3 startPosition = button.transform.position;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            button.transform.position = Vector3.Lerp(startPosition, targetPosition, (Time.time - startTime) / duration);
            yield return null;
        }

        button.transform.position = targetPosition;

        yield return new WaitForSeconds(0.5f); // Adjust this value to control how long the button stays pushed in

        startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            button.transform.position = Vector3.Lerp(targetPosition, startPosition, (Time.time - startTime) / duration);
            yield return null;
        }

        button.transform.position = startPosition;
        isButtonMoving = false;
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
        Gizmos.DrawWireSphere(new Vector3(cam.position.x, cam.position.y - 1, cam.position.z), playerActivateDistance);
    }

}
