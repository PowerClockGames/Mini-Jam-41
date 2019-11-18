using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensei : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup warningUI;

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

    private void OnMouseEnter()
    {
        SoundManager.Instance.PlaySound(senseiHoverSFX, transform.position);
    }
}
