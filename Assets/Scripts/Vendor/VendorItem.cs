using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VendorItem : MonoBehaviour
{
    public Image itemPreview;
    public TMP_Text itemName;
    public TMP_Text itemPrice;
    public Button btnBuy;

    public void SetVendorItemData(Sprite preview, string name, int price)
    {
        itemPreview.sprite = preview;
        itemName.text = name;
        itemPrice.text = price.ToString();
    }
}
