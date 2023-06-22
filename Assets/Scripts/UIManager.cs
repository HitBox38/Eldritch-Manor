using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject uiPrompt;
    [SerializeField] private GameObject finishMenu;
    [SerializeField] private TMP_Text timeField;

    private TMP_Text promptText;

    private void OnEnable()
    {
        PlayerActions.OnCloseToInteract += ShowPrompt;
        PlayerActions.OnLeftFromInteract += HidePrompt;
        GameManager.OnFinish += ShowFinishMenu;
    }

    private void OnDisable()
    {
        PlayerActions.OnCloseToInteract -= ShowPrompt;
        PlayerActions.OnLeftFromInteract -= HidePrompt;
        GameManager.OnFinish -= ShowFinishMenu;
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

    public void ShowFinishMenu(float time)
    {
        finishMenu.SetActive(true);
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        timeField.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
