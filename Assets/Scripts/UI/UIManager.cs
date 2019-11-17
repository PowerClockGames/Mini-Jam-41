using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public TMP_Text timerText;
    public TMP_Text crystalText;
    public Popup popup;
    public GameObject gameOverlayUI;
    public GameObject startUI;
    public GameObject endGameUI;
    public GameObject hoverBuildingPrefab;
    public bool isInMenu;

    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    private HoverBuilding _hoverBuildingInstance;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        ToggleStartUI(true);
        ToggleGameOverlayUI(false);
        ToggleEndUI(false);
    }

    // Update is called once per frame
    private void Update()
    {
        crystalText.text = GameManager.Instance.crystalAmount.ToString();
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        float timeLeft = GameManager.Instance.timeLeft;
        string minutes = Mathf.Floor(timeLeft / 60).ToString("00");
        string seconds = (timeLeft % 60).ToString("00");
        timerText.text = minutes + ":" + seconds;
    }

    public void ToggleGameOverlayUI(bool visible)
    {
        gameOverlayUI.SetActive(visible);
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

    public void ShowHoverBuilding(Vector3 position, Sprite preview)
    {
        _hoverBuildingInstance = Instantiate(hoverBuildingPrefab, position, Quaternion.identity).GetComponent<HoverBuilding>();
        _hoverBuildingInstance.SetData(preview);
    }

    public void HideHoverBuilding()
    {
        _hoverBuildingInstance.Remove();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
