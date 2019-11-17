using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensei : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup popupUI;
    public CanvasGroup warningUI;
    public GameObject tutorialUI;

    [Header("Audio")]
    public AudioClip senseiHoverSFX;
    public AudioClip senseiWarningSFX;

    private void Update()
    {
        if(GameManager.Instance.isHouseOnFire)
        {
            warningUI.alpha = 1;
            SoundManager.Instance.PlaySound(senseiWarningSFX, transform.position);
        } else
        {
            warningUI.alpha = 0;
        }

    }
    void OnMouseDown()
    {
        if (GameManager.Instance.selectedBuilding == null && !GameManager.Instance.isHouseOnFire)
        {
            tutorialUI.SetActive(true);
        }
    }

    private void OnMouseEnter()
    {
        popupUI.FadeIn(this, .2f);
        SoundManager.Instance.PlaySound(senseiHoverSFX, transform.position);
    }

    private void OnMouseExit()
    {
        popupUI.FadeOut(this, .2f);
    }
}
