using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensei : MonoBehaviour
{
    public CanvasGroup popupUI;
    public CanvasGroup warningUI;
    public GameObject tutorialUI;

    private void Update()
    {
        if(GameManager.Instance.isHouseOnFire)
        {
            warningUI.alpha = 1;
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
    }

    private void OnMouseExit()
    {
        popupUI.FadeOut(this, .2f);
    }
}
