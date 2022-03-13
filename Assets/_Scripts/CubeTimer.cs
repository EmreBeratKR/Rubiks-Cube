using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CubeTimer : Scenegleton<CubeTimer>
{
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject resetButton;
    [SerializeField] private TextMeshProUGUI timerText;
    private DateTime startTime;

    public bool isStarted { get; private set; }


    private void OnEnable()
    {
        EventSystem.OnCubeShuffled += StartTimer;
        EventSystem.OnCubeSolved += StopTimer;
        EventSystem.OnCubeGenerateStarted += OnCubeGenerateStarted;
        EventSystem.OnCubeGenerated += OnCubeGenerated;
        EventSystem.OnCubeShuffleStarted += OnShuffleStarted;
    }

    private void OnDisable()
    {
        EventSystem.OnCubeShuffled -= StartTimer;
        EventSystem.OnCubeSolved -= StopTimer;
        EventSystem.OnCubeGenerateStarted -= OnCubeGenerateStarted;
        EventSystem.OnCubeGenerated -= OnCubeGenerated;
        EventSystem.OnCubeShuffleStarted -= OnShuffleStarted;
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

    private void OnCubeGenerateStarted()
    {
        startButton.SetActive(false);
        resetButton.SetActive(false);
        ResetTimer();
    }

    private void OnCubeGenerated()
    {
        startButton.SetActive(true);
        resetButton.SetActive(false);
    }

    private void OnShuffleStarted()
    {
        startButton.SetActive(false);
        resetButton.SetActive(true);
    }
}
