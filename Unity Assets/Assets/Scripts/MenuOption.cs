using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOption : MonoBehaviour
{
    ButtonManager myButtonManager;

    public int minimumValue;
    public int maximumValue;
    public int defaultValue;
    
    public Text bodyPartValue;
    public Slider mySlider;
    private Text valueText;
    private Text tooltipText;
    
    public bool[] lockedOptions;

    // Start is called before the first frame update
    void Start()
    {
        InitializeSlider();
        InitializeValueText();
        InitializeTooltipText();

        bodyPartValue.text = mySlider.value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeSlider()
    {
        mySlider = gameObject.GetComponentInChildren<Slider>();
        mySlider.maxValue = maximumValue;
        mySlider.minValue = minimumValue;
        mySlider.value = defaultValue;
        mySlider.wholeNumbers = true;
        mySlider.onValueChanged.AddListener(delegate { OnValueSliderChanged(); });
    }

    private void InitializeValueText()
    {
        Text[] allText = gameObject.GetComponentsInChildren<Text>();

        foreach(Text text in allText)
        {
            if(text.name == "Value_Text")
            {
                valueText = text;
            }
        }

        SetValueTextAndBodyPartColor(lockedOptions[(int)mySlider.value -1]);
        valueText.text = defaultValue.ToString();
    }

    private void InitializeTooltipText()
    {
        Text[] allText = gameObject.GetComponentsInChildren<Text>();

        foreach (Text text in allText)
        {
            if (text.name == "Tooltip_Text")
            {
                tooltipText = text;
            }
        }

        tooltipText.text = gameObject.name + ":";
    }

    private void OnValueSliderChanged()
    {
        string newValue = mySlider.value.ToString();
        SetValueTextAndBodyPartColor(lockedOptions[(int)mySlider.value - 1]);
        valueText.text = newValue;
        bodyPartValue.text = newValue;
    }

    public void SetValueTextAndBodyPartColor(bool optionLocked)
    {
        
        if (optionLocked)
        {
            valueText.color = Color.red;
            bodyPartValue.color = Color.red;
        }
        else
        {
            valueText.color = Color.white;
            Color32 bodyPartColor = new Color32(92, 109, 125, 255);
            bodyPartValue.color = bodyPartColor;
        }
    }

    public int GetSliderValue()
    {
        return (int)mySlider.value;
    }

}
