using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BuildingPopBox : MonoBehaviour
{
    public TMP_Text levelText;
    private CanvasGroup _buildingPopbox;
    private Canvas _buildingPopboxCanvas;

    public bool isOpen = false;

    private void Awake()
    {
        _buildingPopbox = gameObject.GetComponent<CanvasGroup>();
        _buildingPopboxCanvas = gameObject.GetComponent<Canvas>();
        _buildingPopboxCanvas.worldCamera = Camera.main;
        _buildingPopbox.alpha = 0;
    }

    public void Show(string level)
    {
        UIManager.Instance.isInMenu = true;
        levelText.text = level;
        _buildingPopbox.FadeIn(this, .2f);
        SetBlocking(true);
        isOpen = true;
    }

    public void Hide()
    {
        if(_buildingPopbox != null)
        {
            _buildingPopbox.FadeOutCallback(this, .2f, (done) =>
            {
                SetBlocking(false);
                isOpen = false;
                _buildingPopbox.alpha = 0;
                UIManager.Instance.isInMenu = false;
            });
        }
    }

    private void SetBlocking(bool interactable)
    {
        _buildingPopbox.interactable = interactable;
        _buildingPopbox.blocksRaycasts = interactable;
    }
}
