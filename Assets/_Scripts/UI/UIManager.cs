using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Scenegleton<UIManager>
{
    [SerializeField] private GameObject blackFilter;
    [SerializeField] private GameObject settingsPanel;


    public void ShowBlackFilter()
    {
        blackFilter.SetActive(true);
    }

    public void HideBlackFilter()
    {
        blackFilter.SetActive(false);
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
