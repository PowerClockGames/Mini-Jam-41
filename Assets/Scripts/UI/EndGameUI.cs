using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameUI : MonoBehaviour
{
    public TMP_Text magicCountText;
    public TMP_Text timeCountText;

    [Header("Stars")]
    public GameObject star1;
    public int coinsToUnlock1 = 100;
    public GameObject star2;
    public int coinsToUnlock2 = 200;
    public GameObject star3;
    public int coinsToUnlock3 = 500;

    private int _magicAmount;

    private void Awake()
    {
        star1.SetActive(false);
        star2.SetActive(false);
        star3.SetActive(false);
    }

    public void SetData()
    {
        _magicAmount = GameManager.Instance.crystalAmount;
        magicCountText.text = _magicAmount.ToString();
        timeCountText.text = UIManager.FormatTime(GameManager.Instance.timeLeft);

        if(_magicAmount > coinsToUnlock1)
        {
            star1.SetActive(true);
        }

        if (_magicAmount > coinsToUnlock2)
        {
            star2.SetActive(true);
        }

        if (_magicAmount > coinsToUnlock3)
        {
            star3.SetActive(true);
        }
    }

    public void Restart()
    {
        GameManager.Instance.ResetGame();
        UIManager.Instance.ToggleStartUI(true);
        UIManager.Instance.ToggleEndUI(false);
    }
}
