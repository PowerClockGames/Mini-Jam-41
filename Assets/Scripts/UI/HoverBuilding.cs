using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverBuilding : MonoBehaviour
{
    private SpriteRenderer _hoverSprite;
    public Color correctPlacementColor;
    public Color inCorrectPlacementColor;

    void Awake()
    {
        _hoverSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(GameManager.Instance.canBuildHere)
        {
            TurnGreen();
        } else
        {
            TurnRed();
        }
    }

    public void Remove()
    {
        Cursor.visible = true;
        Destroy(gameObject);
    }

    public void TurnGreen()
    {
        _hoverSprite.color = correctPlacementColor;
    }

    public void TurnRed()
    {
        _hoverSprite.color = inCorrectPlacementColor;
    }

    public void SetData(Sprite preview)
    {
        _hoverSprite.sprite = preview;
    }
}
