using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SizePanel : Scenegleton<SizePanel>
{
    [SerializeField] private Slider sizeSlider;
    [SerializeField] private TextMeshProUGUI sizeText;


    public void OnSlided()
    {
        sizeText.text = sizeSlider.value + "x" + sizeSlider.value + "x" + sizeSlider.value;
    }

    public void ApplySize()
    {
        EventSystem.CubeSizeChanged(Mathf.RoundToInt(sizeSlider.value));
    }
}
