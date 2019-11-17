using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timeLeft = 312f;
    public float gameRoundTime;
    public float currentMagic = 0;
    public float maxMagic = 3000f;
    public int crystalAmount = 0;
    public List<GameObject> placedBuildings;
    public bool gameHasEnded = false;
    public bool gameIsPlaying = false;

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
        currentMagic = 0;
        gameRoundTime = timeLeft;
    }

    public void IncreaseCrystals(int amount)
    {
        crystalAmount += amount;
    }

    public void DecreaseCrystals(int amount)
    {
        crystalAmount -= amount;
    }

    public void IncreaseMagic(int amount)
    {
        if (gameHasEnded)
            return;

        currentMagic += amount;
    }

    public void StartGame()
    {
        timeLeft = gameRoundTime;
        gameIsPlaying = true;
        UIManager.Instance.ToggleStartUI(false);
    }

    public void AddBuilding(GameObject building)
    {
        if(!placedBuildings.Contains(building))
        {
            placedBuildings.Add(building);
        }
    }

    private void Update()
    {
        if(gameIsPlaying)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0f)
            {
                timeLeft = 0f;
                if (!gameHasEnded)
                {
                    EndGame();
                }
            }
        }


        if(!gameHasEnded)
        {
            if (currentMagic != maxMagic)
            {
                IncreaseMagic(10);
            }
            else
            {
                Debug.Log("max Magic reached!");
                EndGame();
            }
        }

    }

    private void EndGame()
    {
        gameHasEnded = true;
        UIManager.Instance.ToggleEndUI(true);
    }

}
