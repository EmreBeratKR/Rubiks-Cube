using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMapRaycaster : MonoBehaviour
{
    public const float MAX_DISTANCE = 1000f;
    
    [SerializeField] private LayerMask layerMask;


    public bool IsSolved
    {
        get
        {
            var _colors = colors;
            var pivot = colors[0];

            for (int i = 1; i < _colors.Count; i++)
            {
                if (_colors[i] != pivot) return false;
            }
            return true;
        }
    }

    private List<Face.ColorCode> colors
    {
        get
        {
            var result = new List<Face.ColorCode>();

            float distance = (RubiksCube.Instance.size - 1) * 0.5f;

            for (float i = -distance; i <= distance; i++)
            {
                for (float j = -distance; j <= distance; j++)
                {
                    var origin = this.transform.position;
                    var deltaUp = this.transform.up * i * RubiksCube.cubieSize;
                    var deltaRight = this.transform.right * j * RubiksCube.cubieSize;

                    Ray ray = new Ray(origin + deltaUp + deltaRight, this.transform.forward);

                    if (Physics.Raycast(ray, out RaycastHit hit, MAX_DISTANCE, layerMask))
                    {
                        var face = hit.collider.GetComponent<Face>();
                        result.Add(face.colorCode);
                    }
                }
            }
            return result;
        }
    }
}
