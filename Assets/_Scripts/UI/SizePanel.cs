using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SizePanel : Scenegleton<SizePanel>
{
    [SerializeField] private GameObject applyButton;
    [SerializeField] private Slider sizeSlider;
    [SerializeField] private TextMeshProUGUI sizeText;

    private int sizeSliderValue => Mathf.RoundToInt(sizeSlider.value);

    public void OnSlided()
    {
        applyButton.SetActive(sizeSliderValue != RubiksCube.Instance.size);
        sizeText.text = sizeSlider.value + "x" + sizeSlider.value + "x" + sizeSlider.value;
    }

    public void ApplySize()
    {
        applyButton.SetActive(false);
        EventSystem.CubeSizeChanged(sizeSliderValue);
    }
}
