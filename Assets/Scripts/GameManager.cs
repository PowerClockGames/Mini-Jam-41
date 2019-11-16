using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static float TimeLeft = 312f;
    public static float CurrentMagic = 0;
    public static float MaxMagic = 3000f;

    public GameObject endGameUI;

    public static bool GameHasEnded;

    private void Awake()
    {
        CurrentMagic = 0;
        GameHasEnded = false;
    }

    public static void IncreaseMagic(int amount)
    {
        if (GameHasEnded)
            return;

        CurrentMagic += amount;
    }

    private void Update()
    {
        TimeLeft -= Time.deltaTime;

        if(!GameHasEnded)
        {
            if (CurrentMagic != MaxMagic)
            {
                IncreaseMagic(10);
            }
            else
            {
                Debug.Log("max Magic reached!");
                EndGame();
            }
        }


        if (TimeLeft <= 0f)
        {
            TimeLeft = 0f;
            if (!GameHasEnded)
            {
                EndGame();
            }
        }
    }

    void EndGame()
    {
        GameHasEnded = true;
        endGameUI.SetActive(true);
    }

}
