using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CubeShuffler
{
    public const int MOVE_PER_SIZE = 10;
    public const float SHUFFLE_SPEED = 1500f;

    public static List<CubeMove> shuffleSequence
    {
        get
        {
            var result = new List<CubeMove>();

            for (int i = 0; i < MOVE_PER_SIZE * RubiksCube.Instance.size; i++)
            {
                result.Add(CubeMove.random);
            }

            return result;
        }
    }
}

public struct CubeMove
{
    public Axis axis;
    public Cubie origin;
    public bool isReversed;

    public CubeMove(Axis axis, Cubie origin, bool isReversed)
    {
        this.axis = axis;
        this.origin = origin;
        this.isReversed = isReversed;
    }

    public static CubeMove random
    {
        get
        {
            var axis = (Axis)Random.Range(0, 3);
            var origin = RubiksCube.randomCubie;
            var isReversed = Random.Range(0, 1) == 0;

            return new CubeMove(axis, origin, isReversed);
        }
    }
}