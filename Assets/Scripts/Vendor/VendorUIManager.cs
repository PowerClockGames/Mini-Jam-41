using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VendorUIManager : MonoBehaviour
{
    private CanvasGroup vendorGroup;
    public VendorItem vendorItemPrefab;
    public RectTransform scrollList;
    public AudioClip vendorOpenSFX;
    public AudioClip purchaseSFX;
    public AudioClip cantPurchaseSFX;

    private List<BuildingAsset> _buildings;

    private static VendorUIManager _instance;
    public static VendorUIManager Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
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
            BuildingAsset baseBuildingAsset = _buildings[i];
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
            vendorItem.btnBuy.onClick.AddListener(() => BuyBuilding(baseBuildingAsset, baseBuidling));
        }
    }

    private void BuyBuilding(BuildingAsset building, Level level)
    {
        if(GameManager.Instance.crystalAmount >= level.levelCost)
        {
            SoundManager.Instance.PlaySound(purchaseSFX, transform.position);
            GameManager.Instance.selectedBuilding = building;
			GameManager.Instance.DecreaseCrystals(level.levelCost);
            UIManager.Instance.ShowHoverBuilding(gameObject.transform.position, level.levelSprite);
            Close(.2f);
        } else
        {
            SoundManager.Instance.PlaySound(cantPurchaseSFX, transform.position);    
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
        vendorGroup.FadeInCallback(this, .2f, (done) => { vendorGroup.alpha = 1; });
        SoundManager.Instance.PlaySound(vendorOpenSFX, transform.position);
    }
}
