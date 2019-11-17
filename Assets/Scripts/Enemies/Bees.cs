using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base code by Brackeys
public class Bees : MonoBehaviour
{
    public float minWaitTime = 2f;
    public float maxWaitTime = 60f;

    public GameObject beePrefab;
    public AudioClip flyingSFX;

    public Transform spawnPoint01;
    public Transform spawnPoint02;
    public Transform spawnPoint03;

    public int spawnPointIndex = 0;

    private AudioSource _flyingAudioSource;

    void Start()
    {
        StartCoroutine(BeeBehaviour());
    }

    IEnumerator BeeBehaviour()
    {
        while (true)
        {
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            SpawnBee();
        }
    }

    void SpawnBee()
    {
        Building buildingTile = SelectBuilding();
        if (buildingTile != null)
        {
            if (buildingTile.IsUnderAttack())
                return;

            Vector3 spawnPoint = new Vector3();
            if (spawnPointIndex == 0)
            {
                spawnPoint = spawnPoint01.position;
                spawnPointIndex++;
            }
            else if (spawnPointIndex == 1)
            {
                spawnPoint = spawnPoint02.position;
                spawnPointIndex++;
            }
            else if (spawnPointIndex == 2)
            {
                spawnPoint = spawnPoint03.position;
                spawnPointIndex = 0;
            }
            _flyingAudioSource = SoundManager.Instance.PlaySound(flyingSFX, transform.position, true);
            GameObject go = Instantiate(beePrefab, spawnPoint, Quaternion.identity);
            Bee bee = go.GetComponent<Bee>();
            bee.SetTarget(buildingTile, _flyingAudioSource);

            buildingTile.SetUnderAttack();
        }
    }

    Building SelectBuilding()
    {
        List<Building> BuildingSelection = new List<Building>();

        Building[] Buildings = FindObjectsOfType<Building>();
        foreach (Building building in Buildings)
        {
            if (building.buildingState == BuildingState.Built)
            {
                BuildingSelection.Add(building);
            }
        }

        if (BuildingSelection.Count >= 1)
        {
            int index = Random.Range(0, BuildingSelection.Count);
            return BuildingSelection[index];
        }
        else
        {
            return null;
        }
    }
}
