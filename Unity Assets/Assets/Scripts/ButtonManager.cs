using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    AdManager myAdManager;
    ShopManager myShopManager;
    CameraManager myCameraManager;
    int invalidSlot = 0;
    public bool validCharacter { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        myCameraManager = gameObject.GetComponent<CameraManager>();
        myShopManager = gameObject.GetComponent<ShopManager>();
        myAdManager = gameObject.AddComponent<AdManager>();
        myAdManager.currentType = AdManager.AdvertisementType.Unity;
        myAdManager.DisplayBanner();
        DisableButton("Unity-Button");

    }

    void Update()
    {
        if(myCameraManager.maincam.isActiveAndEnabled)
        {
            foreach (MenuOption option in myShopManager.menuOptions)
            {
                if (option.lockedOptions[(int)option.mySlider.value - 1])
                {
                    invalidSlot++;
                }
                else
                {
                    invalidSlot--;

                    if (invalidSlot < 0)
                    {
                        invalidSlot = 0;
                    }
                }
            }

            if (invalidSlot == 0)
            {
                getButton("Confirm-Button").enabled = true;
            }
            else
            {
                getButton("Confirm-Button").enabled = false;
            }
        }

    }

    public void OnConfirmClick()
    {
        
    }

    public void OnRandomClick()
    {
        foreach(MenuOption option in myShopManager.menuOptions)
        {
            int randValue = Random.Range(option.minimumValue, option.maximumValue);

            option.mySlider.value = randValue;
        }
    }

    public void OnCameraToggleClick()
    {
        CameraManager cameraManager = GetComponent<CameraManager>();

        cameraManager.ToggleCamera();
    }

    public void OnUnityButtonClick()
    {
        myAdManager.currentType = AdManager.AdvertisementType.Unity;
        myAdManager.DisplayBanner();

        Button myButton = getButton("Unity-Button");
        Button otherButton = getButton("Admob-Button");

        myButton.interactable = false;
        otherButton.interactable = true;

        EnableButton("Admob-Button");
        DisableButton("Unity-Button");
    }

    public void OnAdmobButtonClick()
    {
        myAdManager.currentType = AdManager.AdvertisementType.Admob;
        myAdManager.DisplayBanner();

        EnableButton("Unity-Button");
        DisableButton("Admob-Button");
    }

    private void EnableButton(string buttonName)
    {
        Button myButton = getButton(buttonName);
        myButton.interactable = true;
    }

    private void DisableButton(string buttonName)
    {
        Button myButton = getButton(buttonName);
        myButton.interactable = false;
    }

    public void OnInterstatialButtonClick()
    {
        myAdManager.DisplayInterstatial();
    }

    public void OnRewardedButtonClick()
    {
        myAdManager.DisplayRewarded();
    }

    private Button getButton(string buttonName)
    {
        Button[] buttonList = GetComponentsInChildren<Button>();


        foreach (Button button in buttonList)
        {
            if(button.gameObject.name == buttonName)
            {
                return button;
            }
        }

        return null;
    }

    public void OnPurchaseComplete(Product product)
    {
        if (product.definition.id == "com.dkc1011.5gems")
        {
            myShopManager.Gems += 5;
        }
        else if (product.definition.id == "com.dkc1011.15gems")
        {
            myShopManager.Gems += 15;
        }

    }

    public void OnPurchaseFailure(Product product, PurchaseFailureReason reason)
    {
        print("Purchase of Product " + product.definition.id + " failed due to " + reason);
    }
}
