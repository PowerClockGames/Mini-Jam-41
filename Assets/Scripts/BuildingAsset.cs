using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Upgrade
{
    public int upgradeCost;
    public int upgradeLevel;
    public float upgradeTime;
    public Sprite upgradeSprite;
}

[CreateAssetMenu(fileName = "New Building", menuName = "Building")]
public class BuildingAsset : ScriptableObject
{
    [Header("General")]
    public Sprite buildingImage;
    public int crystalsPerMinute;

    [Header("Construction")]
    public Sprite constructionImage;
    public float constructionTime;
    public int constructionCost;
    public AudioClip constructionCompletedSFX;

    [Header("Upgrades")]
    public Upgrade[] upgrades;
}
