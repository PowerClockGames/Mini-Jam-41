using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendor : MonoBehaviour
{
    public CanvasGroup popupUI;
    public VendorUIManager vendorUI;
    public AudioClip vendorHoverSFX;

    private void OnMouseDown()
    {
        if(CanShowVendor())
        {
            vendorUI.Open();
        }
    }

    private void OnMouseEnter()
    {
        if(CanShowVendor())
        {
            popupUI.FadeIn(this, .2f);
            SoundManager.Instance.PlaySound(vendorHoverSFX, transform.position);
        }
    }

    private void OnMouseExit()
    {
        popupUI.FadeOut(this, .2f);
    }

    private bool CanShowVendor()
    {
        return !GameManager.Instance.gameHasEnded &&
            !GameManager.Instance.introIsPlaying &&
            GameManager.Instance.selectedBuilding == null;
    }
}
