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

    [Header("Audio")]
    public AudioClip buildingPlacedSFX;
    public AudioClip buildingConstructedSFX;
    public AudioClip buildingSelectedSFX;
    public AudioClip magicGeneratedSFX;
    public AudioClip buildingUpgradedSFX;
    public AudioClip buildingDamagedSFX;
    public AudioClip buildingExtinguishSFX;
    public AudioClip buildingDestroyedSFX;

    [Header("Construction")]
    public Sprite constructionImage;

    [Header("Levels")]
    public Level[] levels;
}
