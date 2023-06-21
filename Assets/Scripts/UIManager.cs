using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject uiPrompt;

    private TMP_Text promptText;

    private void OnEnable()
    {
        PlayerActions.OnCloseToInteract += ShowPrompt;
        PlayerActions.OnLeftFromInteract += HidePrompt;
    }

    private void OnDisable()
    {
        PlayerActions.OnCloseToInteract -= ShowPrompt;
        PlayerActions.OnLeftFromInteract -= HidePrompt;
    }

    private void Start()
    {
        promptText = uiPrompt.GetComponentInChildren<TMP_Text>();
    }

    public void ShowPrompt(string text)
    {
        uiPrompt.SetActive(true);

        promptText.text = text;
    }

    public void HidePrompt()
    {
        uiPrompt.SetActive(false);
        promptText.text = "";
    }
}
