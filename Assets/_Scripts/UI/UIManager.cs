using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Scenegleton<UIManager>
{
    [SerializeField] private GameObject blackFilter;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject settingsPanel;


    private void OnEnable()
    {
        EventSystem.OnCubeGenerateStarted += ShowLoadingScreen;
        EventSystem.OnCubeGenerated += HideLoadingScreen;
    }

    private void OnDisable()
    {
        EventSystem.OnCubeGenerateStarted -= ShowLoadingScreen;
        EventSystem.OnCubeGenerated -= HideLoadingScreen;
    }

    public void ShowBlackFilter()
    {
        blackFilter.SetActive(true);
    }

    public void HideBlackFilter()
    {
        blackFilter.SetActive(false);
    }

    public void ShowLoadingScreen()
    {
        loadingScreen.SetActive(true);
    }

    public void HideLoadingScreen()
    {
        loadingScreen.SetActive(false);
    }

    public void OpenSettings()
    {
        ShowBlackFilter();
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        HideBlackFilter();
        settingsPanel.SetActive(false);
    }
}
