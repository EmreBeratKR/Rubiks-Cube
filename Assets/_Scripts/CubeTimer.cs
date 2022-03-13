using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CubeTimer : Scenegleton<CubeTimer>
{
    [SerializeField] private TextMeshProUGUI timerText;
    private DateTime startTime;

    public bool isStarted { get; private set; }


    private void OnEnable()
    {
        EventSystem.OnCubeShuffled += StartTimer;
        EventSystem.OnCubeSolved += StopTimer;
        EventSystem.OnCubeGenerateStarted += ResetTimer;
    }

    private void OnDisable()
    {
        EventSystem.OnCubeShuffled -= StartTimer;
        EventSystem.OnCubeSolved -= StopTimer;
        EventSystem.OnCubeGenerateStarted -= ResetTimer;
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        if (!isStarted) return;

        TimeSpan timeSpan = DateTime.Now - startTime;
        timerText.text = timeSpan.ToString(@"hh\:mm\:ss");
    }

    private void StartTimer()
    {
        isStarted = true;
        startTime = DateTime.Now;
    }

    private void StopTimer()
    {
        isStarted = false;
    }

    private void ResetTimer()
    {
        isStarted = false;
        timerText.text = "00:00:00";
    }
}
