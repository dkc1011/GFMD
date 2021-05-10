using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class ShopManager : MonoBehaviour
{
    public int Gems { get; set; }
    public MenuOption[] menuOptions;
    private int nextLockedItemIndex = 0;
    private string[] lockedItems;
    private int[] menuItemId;


    // Start is called before the first frame update
    void Awake()
    {      
        lockedItems = new string[9];
        menuItemId = new int[9];
        menuOptions = GetComponentsInChildren<MenuOption>();
        Gems = 0;
        //Generates Dummy Data for the lockedOptions value within MenuOption
        foreach (MenuOption menuOption in menuOptions)
        {
            menuOption.lockedOptions = initializeLockedOptions(menuOption);
        }

        nextLockedItemIndex = 0;
    }

    public int GetItemIndex()
    {
        return nextLockedItemIndex;
    }

    public void UnlockItem(string itemName, int lockedOptionIndex)
    {
        foreach(MenuOption menuOption in menuOptions)
        {
            if(menuOption.name == itemName)
            {
                menuOption.lockedOptions[lockedOptionIndex] = false;
                menuOption.SetValueTextAndBodyPartColor(menuOption.lockedOptions[lockedOptionIndex]);
            }
        }
    }

    public string GetNextLockedItemName()
    { 

        string value = lockedItems[nextLockedItemIndex];

        nextLockedItemIndex++;

        return value;
    }

    private bool[] initializeLockedOptions(MenuOption menuOption)
    {
        bool[] lockedOptions = new bool[menuOption.maximumValue];

        //rolledNumbers list is used to make sure the randomIndex value is never repeated.
        List<int> rolledNumbers = new List<int>();
        
        //Sets all menuOptions to false(unlocked)
        for (int i = 0; i == menuOption.maximumValue; i++)
        {
            lockedOptions[i] = false;
        }

        //If there are more than 3 options available on a particular menuOption, set 3 random options to be true(locked)
        if (menuOption.maximumValue > 3)
        {
            for (int i = 1; i < 4; i++)
            {
                bool success = false;

                while(!success)
                {
                    int randomIndex = Random.Range(0, menuOption.maximumValue);

                    if (!rolledNumbers.Contains(randomIndex))
                    {
                        lockedOptions[randomIndex] = true;
                        lockedItems[nextLockedItemIndex] = menuOption.gameObject.name + " " + (randomIndex + 1);
                        nextLockedItemIndex++;
                        rolledNumbers.Add(randomIndex);
                        success = true;
                    }
                }

                
            }

        }

        return lockedOptions;
    }


}
