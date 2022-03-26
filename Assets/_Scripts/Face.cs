using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{

    [field: SerializeField] public ColorCode colorCode { get; private set; }
    

    public enum ColorCode { RED, ORANGE, GREEN, BLUE, YELLOW, WHITE, NONE };
}
