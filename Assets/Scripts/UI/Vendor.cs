﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendor : MonoBehaviour
{
    public CanvasGroup popupUI;
    public VendorUIManager vendorUI;

    private void OnMouseDown()
    {
        if(GameManager.Instance.selectedBuilding == null)
        {
            vendorUI.Open();
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