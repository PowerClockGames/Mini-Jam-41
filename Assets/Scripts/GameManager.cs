using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timeLeft = 312f;
    public float gameRoundTime;
    public int crystalAmount = 0;
    public Building selectedBuilding;
    public List<GameObject> placedBuildings;
    public bool gameHasEnded = false;
    public bool gameIsPlaying = false;
    public bool canBuildHere = false;
    public bool isHouseOnFire = false;

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
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
    }

    private void EndGame()
    {
        gameHasEnded = true;
        UIManager.Instance.ToggleEndUI(true);
    }

}
