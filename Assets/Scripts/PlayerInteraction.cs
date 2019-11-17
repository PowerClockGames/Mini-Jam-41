using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInteraction : MonoBehaviour
{
    public Tilemap tilemap;
    public Grid grid;
    public SortingLayer rayCastLayer;
    public Bounds gridBounds = new Bounds(Vector3.zero, Vector3.one * 10);

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {        
            Vector3 pos = Input.mousePosition;
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(pos);
            Vector3Int cellPos = tilemap.WorldToCell(mousePoint);
            Vector3 cellCenter = grid.GetCellCenterWorld(cellPos);

            if (GameManager.Instance.gameIsPlaying && IsInBounds(mousePoint, gridBounds))
            {
                RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);

                Debug.Log("Clicked Inside grid");
                if (hit.collider != null && hit.transform.GetComponent<Building>() == null)
                {
                    Builder.Instance.PlaceBuilding(null, cellCenter);
                }
            }
        }
    }

    private bool IsInBounds(Vector3 position, Bounds bounds)
    {
        return position.x > bounds.min.x  &&
                position.x < bounds.max.x &&
                position.x > bounds.min.y &&
                position.y < bounds.max.y;
    }
}
