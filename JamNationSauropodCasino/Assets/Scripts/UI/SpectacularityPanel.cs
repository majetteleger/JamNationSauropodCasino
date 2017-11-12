using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpectacularityPanel : MonoBehaviour
{
    public static SpectacularityPanel Instance;

    public Image SpectacularityGauge;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateSpectacularityGauge(float currentValue, float maxValue)
    {
        SpectacularityGauge.fillAmount = currentValue / maxValue;
    }
}
