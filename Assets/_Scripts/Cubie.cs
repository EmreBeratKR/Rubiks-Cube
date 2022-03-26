using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cubie : MonoBehaviour
{
    [SerializeField] private CubieModels models;

    [field: SerializeField]
    public CubieFaces faces { get; private set; }
    
    public Vector3Int gridPosition { get; private set; }
    public float size { get => transform.localScale.x; set => transform.localScale = Vector3.one * value; }


    public void SetType(int x, int y, int z, int size)
    {
        if (size == 1)
        {
            Instantiate(models.All, this.transform);
            return;
        }

        bool R = x == size-1;
        bool L = x == 0;

        bool T = y == size-1;
        bool Bo = y == 0;

        bool F = z == 0;
        bool Ba = z == size-1;

        if (R && T && F)
        {
            Instantiate(models.R_T_F, this.transform);
            return;
        }

        if (R && T && Ba)
        {
            Instantiate(models.R_T_Ba, this.transform);
            return;
        }

        if (R && Bo && F)
        {
            Instantiate(models.R_Bo_F, this.transform);
            return;
        }

        if (R && Bo && Ba)
        {
            Instantiate(models.R_Bo_Ba, this.transform);
            return;
        }

        if (L && T && F)
        {
            Instantiate(models.L_T_F, this.transform);
            return;
        }

        if (L && T && Ba)
        {
            Instantiate(models.L_T_Ba, this.transform);
            return;
        }

        if (L && Bo && F)
        {
            Instantiate(models.L_Bo_F, this.transform);
            return;
        }

        if (L && Bo && Ba)
        {
            Instantiate(models.L_Bo_Ba, this.transform);
            return;
        }

        if (R && T)
        {
            Instantiate(models.R_T, this.transform);
            return;
        }

        if (R && Bo)
        {
            Instantiate(models.R_Bo, this.transform);
            return;
        }

        if (L && T)
        {
            Instantiate(models.L_T, this.transform);
            return;
        }

        if (L && Bo)
        {
            Instantiate(models.L_Bo, this.transform);
            return;
        }

        if (R && F)
        {
            Instantiate(models.R_F, this.transform);
            return;
        }

        if (R && Ba)
        {
            Instantiate(models.R_Ba, this.transform);
            return;
        }

        if (L && F)
        {
            Instantiate(models.L_F, this.transform);
            return;
        }

        if (L && Ba)
        {
            Instantiate(models.L_Ba, this.transform);
            return;
        }

        if (T && F)
        {
            Instantiate(models.T_F, this.transform);
            return;
        }

        if (T && Ba)
        {
            Instantiate(models.T_Ba, this.transform);
            return;
        }
        
        if (Bo && F)
        {
            Instantiate(models.Bo_F, this.transform);
            return;
        }

        if (Bo && Ba)
        {
            Instantiate(models.Bo_Ba, this.transform);
            return;
        }

        if (R)
        {
            Instantiate(models.R, this.transform);
            return;
        }

        if (L)
        {
            Instantiate(models.L, this.transform);
            return;
        }

        if (T)
        {
            Instantiate(models.T, this.transform);
            return;
        }

        if (Bo)
        {
            Instantiate(models.Bo, this.transform);
            return;
        }

        if (F)
        {
            Instantiate(models.F, this.transform);
            return;
        }

        if (Ba)
        {
            Instantiate(models.Ba, this.transform);
            return;
        }
    }
    
    public void SetPosition(int x, int y, int z)
    {
        gridPosition = new Vector3Int(x, y, z);
    }

    public void SetPosition(Vector3Int gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public Vector3Int GetGridPosition()
    {
        var aprx = (this.transform.localPosition + RubiksCube.offset) / RubiksCube.cubieSize;
        return new Vector3Int(Mathf.RoundToInt(aprx.x), Mathf.RoundToInt(aprx.y), Mathf.RoundToInt(aprx.z));
    }

    [System.Serializable]
    private struct CubieModels
    {
        public GameObject All;
        public GameObject R_T_F;
        public GameObject R_T_Ba;
        public GameObject R_Bo_F;
        public GameObject R_Bo_Ba;
        public GameObject L_T_F;
        public GameObject L_T_Ba;
        public GameObject L_Bo_F;
        public GameObject L_Bo_Ba;
        public GameObject R_T;
        public GameObject R_Bo;
        public GameObject L_T;
        public GameObject L_Bo;
        public GameObject R_F;
        public GameObject R_Ba;
        public GameObject L_F;
        public GameObject L_Ba;
        public GameObject T_F;
        public GameObject T_Ba;
        public GameObject Bo_F;
        public GameObject Bo_Ba;
        public GameObject R;
        public GameObject L;
        public GameObject T;
        public GameObject Bo;
        public GameObject F;
        public GameObject Ba;
    }
}

[System.Serializable]
public struct CubieFaces
{
    public const int Count = 6;

    public Face top;
    public Face bottom;
    public Face right;
    public Face left;
    public Face front;
    public Face back;

    public Face this[int index]
    {
        get
        {
            switch (index)
            {
                case 0:
                    return top;
                case 1:
                    return bottom;
                case 2:
                    return right;
                case 3:
                    return left;
                case 4:
                    return front;
                case 5:
                    return back;
                default:
                    return null;
            }
        }
    }
}