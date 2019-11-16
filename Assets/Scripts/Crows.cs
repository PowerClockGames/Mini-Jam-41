//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Crows : MonoBehaviour
//{
//	public float minWaitTime = 2f;
//	public float maxWaitTime = 60f;

//	public GameObject crowPrefab;

//	public Transform spawnPoint01;
//	public Transform spawnPoint02;
//	public Transform spawnPoint03;

//	public int spawnPointIndex = 0;

//	// Start is called before the first frame update
//	void Start()
//    {
//		StartCoroutine(CrowBehaviour());
//    }

//	IEnumerator CrowBehaviour()
//	{
//		while (true)
//		{
//			float waitTime = Random.Range(minWaitTime, maxWaitTime);
//			yield return new WaitForSeconds(waitTime);

//			SpawnCrow();
//		}
//	}

//	void SpawnCrow ()
//	{
//		DirtTile dirtTile = SelectTile();
//		if (dirtTile != null)
//		{
//			if (dirtTile.hasCrow)
//				return;

//			Vector3 spawnPoint = new Vector3();
//			if (spawnPointIndex == 0)
//			{
//				spawnPoint = spawnPoint01.position;
//				spawnPointIndex++;
//			}
//			else if (spawnPointIndex == 1)
//			{
//				spawnPoint = spawnPoint02.position;
//				spawnPointIndex++;
//			}
//			else if (spawnPointIndex == 2)
//			{
//				spawnPoint = spawnPoint03.position;
//				spawnPointIndex = 0;
//			}
//			GameObject go = Instantiate(crowPrefab, spawnPoint, Quaternion.identity);
//			Crow crow = go.GetComponent<Crow>();
//			crow.SetTarget(dirtTile);

//			dirtTile.hasCrow = true;
//		}
//	}

//	DirtTile SelectTile ()
//	{
//		List<DirtTile> tileSelection = new List<DirtTile>();

//		DirtTile[] dirtTiles = FindObjectsOfType<DirtTile>();
//		foreach (DirtTile dirtTile in dirtTiles)
//		{
//			if (dirtTile.crop.HasCrop())
//			{
//				if (dirtTile.crop.state != CropState.Dead)
//				{
//					tileSelection.Add(dirtTile);
//				}
//			}
//		}

//		if (tileSelection.Count >= 1)
//		{
//			int index = Random.Range(0, tileSelection.Count);
//			return tileSelection[index];
//		} else
//		{
//			return null;
//		}
//	}
//}
