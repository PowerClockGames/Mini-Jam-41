using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Magicmeter : MonoBehaviour
{
    public TextMeshProUGUI text;

    private Slider progressSlider;

    private void Awake()
    {
        this.progressSlider = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //string minutes = Mathf.Floor(GameManager.TimeLeft / 60).ToString("00");
        //string seconds = (GameManager.TimeLeft % 60).ToString("00");
        //text.text = minutes + ":" + seconds;

        float magicPercentage = GameManager.CurrentMagic / GameManager.MaxMagic;
        progressSlider.value = magicPercentage;
    }
}
