using UnityEngine;

public static class EventSystem
{
    public delegate void DefaultEvent();
    public delegate void IntParamsEvent(params int[] intParams);

    public static event DefaultEvent OnCubeSideRotated;
    public static event DefaultEvent OnCubeSolved;

    public static event IntParamsEvent OnCubeSizeChanged;


    public static void CubeSideRotated() => OnCubeSideRotated?.Invoke();

    public static void CubeSolved() => OnCubeSolved?.Invoke();

    public static void CubeSizeChanged(params int[] intParams) => OnCubeSizeChanged?.Invoke(intParams);
}
