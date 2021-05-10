using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopItemController : MonoBehaviour
{
    string itemName;
    public int itemCost;
    int itemIndex;
    int menuItemIndex;
    bool purchased;
    Text ItemNameText;
    Text ItemCostText;
    Button myButton;
    ShopManager myShopManager;
    Image purchasedImage;
    

    // Start is called before the first frame update
    void Start()
    {
        myShopManager = GetComponentInParent<ShopManager>();
        itemIndex = myShopManager.GetItemIndex();
        itemName = myShopManager.GetNextLockedItemName();
        myButton = GetComponentInChildren<Button>();
        InitializePurchasedImage();
        InitializeNameAndCostText();
        InitializeAvailable();
        purchased = false;
    }

    private void Update()
    {
        if(!purchased)
        {
            InitializeAvailable();
        }
    }

    private void InitializePurchasedImage()
    {
        Image[] allImages = gameObject.GetComponentsInChildren<Image>();

        foreach (Image image in allImages)
        {
            if (image.name == "Purchased-Image")
            {
                purchasedImage = image;
            }
        }

        purchasedImage.enabled = false;
    }

    private void InitializeAvailable()
    {
        if(myShopManager.Gems < itemCost)
        {
            ItemCostText.color = Color.red;
            myButton.enabled = false;
        }
        else
        {
            ItemCostText.color = Color.white;
            myButton.enabled = true;
        }
    }

    private void InitializeNameAndCostText()
    {
        Text[] allText = gameObject.GetComponentsInChildren<Text>();

        foreach (Text text in allText)
        {
            if (text.name == "Item-Name-Text")
            {
                ItemNameText = text;
            }
            
            if (text.name == "Item-Cost-Text")
            {
                ItemCostText = text;
            }
        }

        ItemNameText.text = itemName;
        ItemCostText.text = itemCost.ToString() + " Gems";
    }

    public void OnBuyButtonClick()
    {
        if(myShopManager.Gems >= itemCost)
        {
            myButton.enabled = false;
            ItemNameText.color = Color.grey;
            ItemCostText.color = Color.grey;
            purchasedImage.enabled = true;

            int optionId = parseOptionId() - 1;

            if(optionId != -1)
            {
                myShopManager.UnlockItem(parseName(), optionId);
            }

            myShopManager.Gems -= itemCost;
            purchased = true;
            
        }
    }

    private int parseOptionId()
    {
        for(int i = 0; i < itemName.Length; i++)
        {
            if(char.IsDigit(itemName[i]))
            {
                return int.Parse(itemName[i].ToString());
            }
        }

        return -1;
    }

    private string parseName()
    {
        return itemName.Substring(0, itemName.Length - 2);
    }
}
