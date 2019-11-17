using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level
{
    public int levelCost;
    public int crystalsPerMinute;
    public float levelConstructionTime;
    public Sprite levelSprite;
}

[CreateAssetMenu(fileName = "New Building", menuName = "Building")]
public class BuildingAsset : ScriptableObject
{
    [Header("General")]
    public string buildingName;

    [Header("Construction")]
    public Sprite constructionImage;
    public AudioClip constructionCompletedSFX;

    [Header("Levels")]
    public Level[] levels;
}
