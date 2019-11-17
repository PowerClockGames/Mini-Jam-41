using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public TMP_Text crystalText;
    public Popup popup;
    public GameObject startUI;
    public GameObject endGameUI;

    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        ToggleStartUI(true);
    }

    // Update is called once per frame
    void Update()
    {
        crystalText.text = GameManager.Instance.crystalAmount.ToString();
    }

    public void ToggleStartUI(bool visible)
    {
        startUI.SetActive(visible);
    }

    public void ToggleEndUI(bool visible)
    {
        endGameUI.SetActive(visible);
    }

    public void ShowPopup(string text, bool isOkBox, UnityAction confirmAction)
    {
        popup.SetSingleButton(isOkBox);
        popup.SetText(text);
        popup.SetAction(confirmAction);
        popup.Open();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
