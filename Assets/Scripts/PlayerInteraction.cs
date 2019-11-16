using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInteraction : MonoBehaviour
{
    public Tilemap tilemap;
    public Grid grid;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Input.mousePosition;
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(pos);

            if (tilemap != null)
            {
                Vector3Int tilePos = tilemap.WorldToCell(mousePoint);
                Debug.Log(tilePos);
            }

            RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);

            if (hit.collider != null && hit.transform.GetComponent<Building>() == null)
            {
                Builder.Instance.PlaceBuilding(null, mousePoint);
                Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);
            }

        }
    }
}
