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
    public GameObject btnConfirmBG;
    public Button btnOk;
    public GameObject btnOkBG;
    public Button btnCancel;
    public GameObject btnCancelBG;
    public AudioClip openSFX;
    public AudioClip closeSFX;

    private RectTransform popupTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        popupTransform = gameObject.GetComponent<RectTransform>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        popupTransform.localScale = new Vector3(0, 0, 0);

        btnCancel.onClick.AddListener(() => Close());
        btnOk.onClick.AddListener(() => Close());
        btnConfirm.onClick.AddListener(() => Close());
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

        btnOkBG.SetActive(isSingle);
        btnCancelBG.SetActive(!isSingle);
        btnConfirmBG.SetActive(!isSingle);
    }

    public void Close()
    {
        SoundManager.Instance.PlaySound(closeSFX, transform.position);
        canvasGroup.alpha = 0;
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), .4f).setEaseInCubic();
        LeanTween.delayedCall(.4f, () => {
            UIManager.Instance.isInMenu = false;
            Destroy(gameObject);
        });
    }

    public void Open()
    {
        UIManager.Instance.isInMenu = true;
        SoundManager.Instance.PlaySound(openSFX, transform.position);
        canvasGroup.alpha = 1;
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), .4f).setEaseInCubic();
    }
}
