using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private Slider rotationSensivitySlider;
    [SerializeField] private Slider zoomSensivitySlider;
    [SerializeField] private Slider animationSpeedSlider;
    [SerializeField] private Toggle invertZoomToggle;
    [SerializeField] private Toggle noAnimationToggle;


    public void OnRotationSensivitySlided()
    {
        EventSystem.RotationSensivityChanged(rotationSensivitySlider.value);
    }

    public void OnZoomSensivitySlided()
    {
        EventSystem.ZoomSensivityChanged(zoomSensivitySlider.value);
    }

    public void OnAnimationSpeedSlided()
    {
        EventSystem.AnimationSpeedChanged(animationSpeedSlider.value);
    }

    public void OnInvertZoomToggled()
    {
        EventSystem.InvertZoomToggled(invertZoomToggle.isOn);
    }

    public void OnNoAnimationToggled()
    {
        animationSpeedSlider.gameObject.SetActive(!noAnimationToggle.isOn);
        EventSystem.NoAnimationToggled(noAnimationToggle.isOn);
    }
}
