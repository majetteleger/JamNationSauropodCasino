using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketInfo : MonoBehaviour
{
    public Image Icon;
    public Image KeyBackground;
    public Text Key;
    public Image LaunchSlider;
    
    public void UpdateLaunchSlider(float currentValue, float maxValue)
    {
        LaunchSlider.fillAmount = currentValue / maxValue;
    }

    public void DisplayKey(string key)
    {
        Key.text = key;
    }
}
