using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameUI : MonoBehaviour
{
    public TMP_Text magicCount;
    public TMP_Text timeCount;

    public void SetData()
    {
        magicCount.text = GameManager.Instance.crystalAmount.ToString();
        magicCount.text = UIManager.FormatTime(GameManager.Instance.timeLeft);
    }

    public void Restart()
    {
        GameManager.Instance.ResetGame();
        UIManager.Instance.ToggleStartUI(true);
        UIManager.Instance.ToggleEndUI(false);
    }
}
