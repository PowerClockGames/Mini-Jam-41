using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VendorUIManager : MonoBehaviour
{
    private CanvasGroup vendorGroup;
    public VendorItem vendorItemPrefab;
    public RectTransform scrollList;

    private List<Building> _buildings;

    private void Awake()
    {
        vendorGroup = gameObject.GetComponent<CanvasGroup>();
        vendorGroup.alpha = 0;
        vendorGroup.interactable = false;
        vendorGroup.blocksRaycasts = false;
    }

    private void Start()
    {
        _buildings = Builder.Instance.buildings.ToList();
        SetVendorItems();
    }

    public void SetVendorItems()
    {
        
        for (int i = 0; i < _buildings.Count; i++)
        {
            Building currentBuilding = _buildings[i];
            BuildingAsset baseBuildingAsset = currentBuilding.asset;
            Level baseBuidling = baseBuildingAsset.levels[0];

            if ((this.transform == null) || (vendorItemPrefab == null))
            {
                return;
            }

            GameObject instance = (GameObject)Instantiate(vendorItemPrefab.gameObject);
            instance.transform.SetParent(scrollList.transform, false);

            VendorItem vendorItem = instance.GetComponent<VendorItem>();
            if (vendorItem == null)
            {
                return;
            }

            vendorItem.SetVendorItemData(baseBuidling.levelSprite, baseBuildingAsset.buildingName, baseBuidling.levelCost);
            vendorItem.btnBuy.onClick.AddListener(() => BuyBuilding(currentBuilding, baseBuidling));
        }
    }

    private void BuyBuilding(Building building, Level level)
    {
        if(GameManager.Instance.crystalAmount >= level.levelCost)
        {
            GameManager.Instance.selectedBuilding = building;
            UIManager.Instance.ShowHoverBuilding(gameObject.transform.position, level.levelSprite);
            Close(.2f);
        }
    }

    public void Close(float closeSpeed)
    {
        vendorGroup.FadeOutCallback(this, closeSpeed, (done) =>
        {
            vendorGroup.interactable = false;
            vendorGroup.blocksRaycasts = false;
            vendorGroup.alpha = 0;
            UIManager.Instance.isInMenu = false;
        });
    }

    public void Open()
    {
        UIManager.Instance.isInMenu = true;
        vendorGroup.interactable = true;
        vendorGroup.blocksRaycasts = true;
        vendorGroup.FadeIn(this, .2f);
    }
}
