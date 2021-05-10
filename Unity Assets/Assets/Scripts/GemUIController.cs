using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemUIController : MonoBehaviour
{
    ShopManager shopManager;
    Text valueText;

    // Start is called before the first frame update
    void Awake()
    {
        shopManager = GetComponentInParent<ShopManager>();
        InitializeValueText();
    }

    void Update()
    {
        InitializeValueText();
    }

    private void InitializeValueText()
    {
        Text[] allText = gameObject.GetComponentsInChildren<Text>();

        foreach (Text text in allText)
        {
            if (text.name == "Gem-Value")
            {
                text.text = shopManager.Gems.ToString();
            }
        }
    }
}
