using UnityEngine;
using UnityEngine.Events;

public class BuildingPopBox : MonoBehaviour
{
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

    public void Show()
    {
        UIManager.Instance.isInMenu = true;
        gameObject.SetActive(true);
        _buildingPopbox.FadeIn(this, .2f);
        isOpen = true;
    }

    public void Hide()
    {
        _buildingPopbox.FadeOutCallback(this, .2f, (done) =>
        {
            gameObject.SetActive(false);
            isOpen = false;
            UIManager.Instance.isInMenu = false;
        });
    }
}
