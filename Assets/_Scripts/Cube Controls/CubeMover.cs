using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMover : MonoBehaviour
{
    public const float DragThreshold_InPixels = 50f;

    [SerializeField] private RubiksCube rubiksCube;
    [SerializeField] private Camera _camera;
    [SerializeField] private float speed;
    private Vector3 lastPos;
    private Cubie targetCubie;
    private RaycastHit? hit;
    private Axis? normal;

    public Vector3? rotationAxis
    {
        get
        {
            if (normal == null) return null;

            if (normal == Axis.X) return GetRotationAxis(rubiksCube.transform.up, rubiksCube.transform.forward);

            if (normal == Axis.Y) return GetRotationAxis(rubiksCube.transform.right, rubiksCube.transform.forward);

            if (normal == Axis.Z) return GetRotationAxis(rubiksCube.transform.right, rubiksCube.transform.up);

            return null;
        }
    }

    public Vector3 LocalAxis(Axis axis)
    {
        return axis == Axis.X ? rubiksCube.transform.right :
            axis == Axis.Y ? rubiksCube.transform.up : rubiksCube.transform.forward;
    }


    private void OnEnable()
    {
        EventSystem.OnAnimationSpeedChanged += UpdateAnimationSpeed;
        EventSystem.OnNoAnimationToggled += UpdateNoAnimation;
        EventSystem.OnCubeGenerateStarted += CancelShuffle;
    }

    private void OnDisable()
    {
        EventSystem.OnAnimationSpeedChanged -= UpdateAnimationSpeed;
        EventSystem.OnNoAnimationToggled -= UpdateNoAnimation;
        EventSystem.OnCubeGenerateStarted -= CancelShuffle;
    }

    private void Update()
    {
        HandleMovement();
    }
    
    public void Shuffle()
    {
        if (isShuffling) return;

        EventSystem.CubeShuffleStarted();
        shuffleCoroutine = StartCoroutine(Shuffle_Co());
    }

    private void CancelShuffle()
    {
        StopAllCoroutines();
        shuffleCoroutine = null;
        moveCoroutine = null;
    }

    private Coroutine shuffleCoroutine;
    private bool isShuffling => shuffleCoroutine != null;

    public IEnumerator Shuffle_Co()
    {
        var sequence = CubeShuffler.shuffleSequence;
        foreach (var move in sequence)
        {
            var moveInfo = GetMoveInfo(move.origin, Vector3.zero, move.axis ,true);
            moveInfo.axisEnum = move.axis;
            moveInfo.isReversed = move.isReversed;
            moveInfo.isShuffle = true;
            yield return StartCoroutine(Move_Co(moveInfo));
        }
        shuffleCoroutine = null;
        EventSystem.CubeShuffled();
    }

    public bool isMoving => moveCoroutine != null;
    private bool canMove => !isMoving && InputChecker.Instance.isEnabled && !isShuffling && CubeTimer.Instance.isStarted;

    private void HandleMovement()
    {
        if (!canMove)
        {
            lastPos = Input.mousePosition;
            targetCubie = null;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            lastPos = Input.mousePosition;
            targetCubie = CubieRaycaster.Instance.GetTarget(out hit, out normal);
            return;
        }

        if (Input.GetMouseButton(0))
        {
            if (targetCubie == null) return;

            var rotAxis = rotationAxis;

            if (rotAxis == null) return;

            var _axis = CubieRaycaster.Instance.GetAxis(rotAxis.Value);

            var moveInfo = GetMoveInfo(targetCubie, rotAxis.Value, _axis.Value);
            
            moveCoroutine = StartCoroutine(Move_Co(moveInfo));
        }
    }

    private bool IsReversed(Vector3 normal, Axis axis)
    {
        if (axis == Axis.X)
        {
            if (normal == -rubiksCube.transform.up) return true;

            if (normal == rubiksCube.transform.forward) return true;
        }

        if (axis == Axis.Y)
        {
            if (normal == rubiksCube.transform.right) return true;

            if (normal == -rubiksCube.transform.forward) return true;
        }

        if (axis == Axis.Z)
        {
            if (normal == -rubiksCube.transform.right) return true;

            if (normal == rubiksCube.transform.up) return true;
        }

        return false;
    }

    private Vector3? GetRotationAxis(Vector3 first, Vector3 second)
    {
        var firstAxis = hit.Value.point + first;
        var secondAxis = hit.Value.point + second;
        
        var wp_firstAxis = _camera.WorldToScreenPoint(firstAxis);
        var wp_secondAxis = _camera.WorldToScreenPoint(secondAxis);

        var currentMousePos = Input.mousePosition;

        var dotFirst = Vector3.Dot(wp_firstAxis - lastPos, currentMousePos - lastPos);
        var dotSecond = Vector3.Dot(wp_secondAxis - lastPos, currentMousePos - lastPos);

        if ((Input.mousePosition - lastPos).magnitude < DragThreshold_InPixels) return null;

        if (Mathf.Abs(dotFirst) > Mathf.Abs(dotSecond))
        {
            return second * (dotFirst > 0f ? 1f : -1f);
        }
        else
        {
            return first * (dotSecond > 0f ? 1f : -1f);
        }
    }

    private MoveInfo GetMoveInfo(Cubie origin, Vector3 signedAxis, Axis axis, bool isShuffle = false)
    {
        var result = MoveInfo.Default;

        result.axis = signedAxis;
        if (!isShuffle) result.isReversed = IsReversed(hit.Value.normal, axis);
        
        if (axis == Axis.X)
        {
            for (int y = 0; y < rubiksCube.size; y++)
            {
                for (int z = 0; z < rubiksCube.size; z++)
                {
                    var cubie = rubiksCube.cubies[origin.gridPosition.x, y, z];

                    if (cubie == null) continue;

                    result.cubies.Add(cubie);
                }
            }
        }

        else if (axis == Axis.Y)
        {
            for (int x = 0; x < rubiksCube.size; x++)
            {
                for (int z = 0; z < rubiksCube.size; z++)
                {
                    var cubie = rubiksCube.cubies[x, origin.gridPosition.y, z];

                    if (cubie == null) continue;

                    result.cubies.Add(cubie);
                }
            }
        }

        else if (axis == Axis.Z)
        {
            for (int x = 0; x < rubiksCube.size; x++)
            {
                for (int y = 0; y < rubiksCube.size; y++)
                {
                    var cubie = rubiksCube.cubies[x, y, origin.gridPosition.z];

                    if (cubie == null) continue;

                    result.cubies.Add(cubie);
                }
            }
        }

        return result;
    }

    private void UpdateAnimationSpeed(params float[] args)
    {
        this.speed = args[0];
    }

    private void UpdateNoAnimation(params bool[] args)
    {
        this.noAnim = args[0];
    }

    private Coroutine moveCoroutine;
    private bool noAnim;

    private IEnumerator Move_Co(MoveInfo moveInfo)
    {
        float rotated = 0f;
        while (true)
        {    
            var axis = moveInfo.isShuffle ? LocalAxis(moveInfo.axisEnum) : moveInfo.axis;

            float angle = noAnim ? 90f : Time.deltaTime * (moveInfo.isShuffle ? CubeShuffler.SHUFFLE_SPEED : speed);
            float lastRotated = rotated;
            rotated += angle;
            if (rotated >= 90f)
            {
                angle = 90f - lastRotated;
                rotated = 90f;
                foreach (var cubie in moveInfo.cubies)
                {
                    cubie.transform.RotateAround(rubiksCube.transform.position, axis, angle * (moveInfo.isReversed ? -1f : 1f));
                }
                break;
            }
            foreach (var cubie in moveInfo.cubies)
            {
                cubie.transform.RotateAround(rubiksCube.transform.position, axis, angle * (moveInfo.isReversed ? -1f : 1f));
            }

            yield return 0;
        }

        foreach (var cubie in moveInfo.cubies)
        {
            cubie.SetPosition(cubie.GetGridPosition());
            rubiksCube.cubies[cubie.gridPosition.x, cubie.gridPosition.y, cubie.gridPosition.z] = cubie;
        }

        yield return new WaitForFixedUpdate();
        EventSystem.CubeSideRotated();

        moveCoroutine = null;
    }
}

public enum Axis { X, Y, Z }

public struct MoveInfo
{
    public bool isShuffle;
    public bool isReversed;
    public Vector3 axis;
    public Axis axisEnum;
    public List<Cubie> cubies;

    public static MoveInfo Default => new MoveInfo
    {
        isShuffle = false,
        isReversed = false,
        axis = Vector3.zero,
        axisEnum = Axis.X,
        cubies = new List<Cubie>()
    };
}