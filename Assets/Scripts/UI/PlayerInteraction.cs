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
        if(!UIManager.Instance.isInMenu && GameManager.Instance.gameIsPlaying)
        {
            HandleMouseClick();
            HandleMouseOver();
        }
    }

    private void HandleMouseOver()
    {
        if(GameManager.Instance.selectedBuilding != null)
        {
            GameManager.Instance.canBuildHere = false;

            CheckBoundsAndCast((mousePoint) =>
            {
                GameManager.Instance.canBuildHere = true;
            });
        }
    }

    private void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckBoundsAndCast((mousePoint) =>
            {
                Vector3Int cellPos = tilemap.WorldToCell(mousePoint);
                Vector3 cellCenter = grid.GetCellCenterWorld(cellPos);
                PlaceBuildingIfSelected(cellCenter); ;
            });
        }
    }

    private void CheckBoundsAndCast(System.Action<Vector3> onRaycastHit)
    {
        Vector3 mousePoint = GetMouseInWorld();
        if (IsInBounds(mousePoint, gridBounds))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);

            if (hit.collider != null && hit.transform.GetComponent<Building>() == null)
            {
                onRaycastHit(mousePoint);
            }
        }
    }

    private void PlaceBuildingIfSelected(Vector3 position)
    {
        BuildingAsset selectedBuilding = GameManager.Instance.selectedBuilding;
        if (selectedBuilding != null)
        {
            Builder.Instance.PlaceBuilding(selectedBuilding, position);
            GameManager.Instance.selectedBuilding = null;
            UIManager.Instance.HideHoverBuilding();
        }
    }

    private Vector3 GetMouseInWorld()
    {
        Vector3 pos = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(pos);
    }

    private bool IsInBounds(Vector3 position, Bounds bounds)
    {
        return position.x > bounds.min.x &&
        position.x < bounds.max.x &&
        position.y > bounds.min.y &&
        position.y < bounds.max.y;

    }
}
