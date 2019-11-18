using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timeLeft = 312f;
    public float gameRoundTime;
    public int crystalAmount = 0;
    public BuildingAsset selectedBuilding;
    public List<GameObject> placedBuildings;
    public bool gameHasEnded = false;
    public bool gameIsPlaying = false;
    public bool canBuildHere = false;
    public bool isHouseOnFire = false;

    private string _highscoreKey = "Highscore";

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
        gameRoundTime = timeLeft;
    }

    private void Update()
    {
        if (gameIsPlaying)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0f)
            {
                if (!gameHasEnded)
                {
                    EndGame();
                }
            }
        }
    }

    public void SetHighScore(int score)
    {
        PlayerPrefs.SetInt(_highscoreKey, score);
    }

    public int GetHighScore()
    {
        if (PlayerPrefs.HasKey(_highscoreKey))
        {
            return PlayerPrefs.GetInt(_highscoreKey);
        } else
        {
            return 0;
        }
        
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
        UIManager.Instance.ToggleStartUI(false);
        UIManager.Instance.ToggleIntroUI(true);
    }

    public void StartTimer()
    {
        timeLeft = gameRoundTime;
        gameHasEnded = false;
        gameIsPlaying = true;
    }

    public void AddBuilding(GameObject building)
    {
        if(!placedBuildings.Contains(building))
        {
            placedBuildings.Add(building);
        }
    }

    public void RemoveBuilding(GameObject building)
    {
        if (!placedBuildings.Contains(building))
        {
            int index = placedBuildings.IndexOf(building);
            placedBuildings.RemoveAt(index);
        }
    }

    private void EndGame()
    {
        gameHasEnded = true;

        if(GetHighScore() < crystalAmount)
        {
            SetHighScore(crystalAmount);
        }

        selectedBuilding = null;
		ClearHoverBuilding();
        SoundManager.Instance.StopAllLoopingSounds();
        UIManager.Instance.ToggleGameOverlayUI(false);
        UIManager.Instance.ToggleEndUI(true);
        VendorUIManager.Instance.Close(0f);
    }

    public void ResetGame()
    {
        timeLeft = 0f;
        crystalAmount = 0;
        CleanBoard();
    }
	
	private void ClearHoverBuilding() {
		HoverBuilding[] hoverBuidlings = FindObjectsOfType<HoverBuilding>();
		foreach (HoverBuilding hover in hoverBuidlings)
        {
            Destroy(hover.gameObject);
        }
	}

    private void ClearHealthBar()
    {
        HealthBar[] bars = FindObjectsOfType<HealthBar>();
        foreach (HealthBar bar in bars)
        {
            Destroy(bar.gameObject);
        }
    }

    private void ClearParticles()
    {
        GameObject[] fireParticles = GameObject.FindGameObjectsWithTag("Fire");
        foreach (GameObject go in fireParticles)
        {
            Destroy(go);
        }
    }

    private void ClearBees()
    {
        Bee[] spawnedBees = FindObjectsOfType<Bee>();
        foreach (Bee bee in spawnedBees)
        {
            Destroy(bee.gameObject);
        }
    }

    private void ClearBuildings()
    {

        Building[] buildings = FindObjectsOfType<Building>();
        foreach (Building building in buildings)
        {
            Destroy(building.gameObject);
        }
    }

    private void CleanBoard()
    {
        placedBuildings.Clear();
        ClearHealthBar();
        ClearBees();
        ClearBuildings();
        ClearParticles();
    }

}
