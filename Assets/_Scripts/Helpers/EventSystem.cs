using UnityEngine;

public static class EventSystem
{
    public delegate void DefaultEvent();
    public delegate void BoolParamsEvent(params bool[] boolParams);
    public delegate void IntParamsEvent(params int[] intParams);
    public delegate void FloatParamsEvent(params float[] floatParams);

    public static event DefaultEvent OnCubeSideRotated;
    public static event DefaultEvent OnCubeSolved;

    public static event BoolParamsEvent OnInvertZoomToggled;
    public static event BoolParamsEvent OnNoAnimationToggled;

    public static event IntParamsEvent OnCubeSizeChanged;

    public static event FloatParamsEvent OnAnimationSpeedChanged;
    public static event FloatParamsEvent OnRotationSensivityChanged;
    public static event FloatParamsEvent OnZoomSensivityChanged;


    public static void CubeSideRotated() => OnCubeSideRotated?.Invoke();

    public static void CubeSolved() => OnCubeSolved?.Invoke();

    public static void InvertZoomToggled(params bool[] boolParams) => OnInvertZoomToggled?.Invoke(boolParams);

    public static void NoAnimationToggled(params bool[] boolParams) => OnNoAnimationToggled?.Invoke(boolParams);

    public static void CubeSizeChanged(params int[] intParams) => OnCubeSizeChanged?.Invoke(intParams);

    public static void RotationSensivityChanged(params float[] floatParams) => OnRotationSensivityChanged?.Invoke(floatParams);

    public static void AnimationSpeedChanged(params float[] floatParams) => OnAnimationSpeedChanged?.Invoke(floatParams);

    public static void ZoomSensivityChanged(params float[] floatParams) => OnZoomSensivityChanged?.Invoke(floatParams);
}
