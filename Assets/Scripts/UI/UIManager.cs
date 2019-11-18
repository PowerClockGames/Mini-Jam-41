using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public TMP_Text highScoreText;
    public TMP_Text timerText;
    public TMP_Text crystalText;
    public GameObject popupPrefab;
    public GameObject gameOverlayUI;
    public GameObject introUI;
    public GameObject startUI;
    public GameObject endGameUI;
    public GameObject creditsUI;
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
        highScoreText.text = "HighScore: " + GameManager.Instance.GetHighScore();
        ToggleStartUI(true);
        ToggleGameOverlayUI(false);
        ToggleEndUI(false);
        ToggleIntroUI(false);
        ToggleCreditsUI(false);
    }

    // Update is called once per frame
    private void Update()
    {
        crystalText.text = GameManager.Instance.crystalAmount.ToString();
        UpdateTimer();
    }

    public static string FormatTime(float time)
    {
        string minutes = Mathf.Floor(time / 60).ToString("00");
        string seconds = (time % 60).ToString("00");
        return minutes + ":" + seconds;
    }

    public static bool CanShowUI()
    {
        return GameManager.Instance.selectedBuilding == null && !GameManager.Instance.isHouseOnFire && !GameManager.Instance.gameHasEnded;
    }

    private void UpdateTimer()
    {
        float timeLeft = GameManager.Instance.timeLeft;
        timerText.text = FormatTime(timeLeft);
    }

    public void ShowPopup(string text, bool isOkBox, UnityAction confirmAction)
    {
        GameObject popupInstance = Instantiate(popupPrefab, transform.position, Quaternion.identity);
        popupInstance.transform.SetParent(gameObject.transform, false);
        Popup popup = popupInstance.GetComponent<Popup>();
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

    public void ToggleIntroUI(bool visible)
    {
        introUI.SetActive(visible);
    }

    public void ToggleCreditsUI(bool visible)
    {
        creditsUI.SetActive(visible);
    }

    public void ToggleGameOverlayUI(bool visible)
    {
        gameOverlayUI.SetActive(visible);

        if (visible)
        {
            gameOverlayUI.GetComponent<RectTransform>().LeanMove(Vector3.zero, .4f).setEaseOutCubic();
        }
        else
        {
            gameOverlayUI.GetComponent<RectTransform>().LeanMove(new Vector3(0, 90), .4f).setEaseOutCubic();
        }
    }

    public void ToggleStartUI(bool visible)
    {
        startUI.SetActive(visible);
    }

    public void ToggleEndUI(bool visible)
    {
        endGameUI.SetActive(visible);

        if(visible)
        {
            endGameUI.GetComponent<EndGameUI>().SetData();
        }
    }
}
