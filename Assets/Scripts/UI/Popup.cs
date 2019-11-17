using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class Popup : MonoBehaviour
{
    public TMP_Text popupText;
    public Button btnConfirm;
    public Button btnOk;
    public Button btnCancel;

    private RectTransform popupTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        popupTransform = gameObject.GetComponent<RectTransform>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        popupTransform.localScale = new Vector3(0, 0, 0);
        SetBlocking(false);
    }

    public void SetText(string text)
    {
        popupText.text = text;
    }

    public void SetAction(UnityAction action)
    {
        btnConfirm.onClick.AddListener(() => action());
    }

    public void SetSingleButton(bool isSingle)
    {
        btnOk.gameObject.SetActive(isSingle);
        btnConfirm.gameObject.SetActive(!isSingle);
        btnCancel.gameObject.SetActive(!isSingle);
    }

    public void Close()
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), .4f).setEaseInCubic();
        LeanTween.delayedCall(.4f, () => { SetBlocking(false); });
    }

    public void Open()
    {
        LeanTween.scale(gameObject, new Vector3(1, 1, 0), .4f).setEaseInCubic();
        LeanTween.delayedCall(.4f, () => { SetBlocking(true); });
    }

    private void SetBlocking(bool isBlocking)
    {
        canvasGroup.blocksRaycasts = isBlocking;
        canvasGroup.interactable = isBlocking;
    }
}
