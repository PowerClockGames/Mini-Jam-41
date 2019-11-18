using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSFX : MonoBehaviour
{
    public AudioClip pressSFX;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(pressSFX, transform.position);
        });
    }
}
