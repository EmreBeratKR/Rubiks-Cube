using UnityEngine;

public static class EventSystem
{
    public delegate void DefaultEvent();

    public static event DefaultEvent OnCubeSideRotated;
    public static event DefaultEvent OnCubeSolved;


    public static void CubeSideRotated() => OnCubeSideRotated?.Invoke();

    public static void CubeSolved() => OnCubeSolved?.Invoke();
}
