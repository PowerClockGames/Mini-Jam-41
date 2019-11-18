using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public GameObject baseBuidlingPrefab;
    public BuildingAsset[] buildings;

    private static Builder _instance;
    public static Builder Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
    }

    public void PlaceBuilding(BuildingAsset building, Vector3 position)
    {
        position = new Vector3(position.x, position.y, 0);
        GameObject newBuilding = Instantiate(baseBuidlingPrefab, position, Quaternion.identity);
        Building buildingComp = newBuilding.GetComponent<Building>();
        buildingComp.SetAsset(building);
        SoundManager.Instance.PlaySound(building.buildingPlacedSFX, transform.position);
        buildingComp.StartConstruction(building.levels[0]);
    }
}
