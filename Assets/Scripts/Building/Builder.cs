using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public Building[] buildings;

    private static Builder _instance;
    public static Builder Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
    }

    public void PlaceBuilding(BuildingAsset building, Vector3 position)
    {
        position = new Vector3(position.x, position.y, 0);
        GameObject newBuilding = Instantiate(buildings[0].gameObject, position, Quaternion.identity);
        Building buildingComp = newBuilding.GetComponent<Building>();
        buildingComp.StartConstruction(buildingComp.asset.levels[0]);
    }
}
