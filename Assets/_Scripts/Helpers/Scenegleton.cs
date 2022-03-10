using UnityEngine;

public abstract class Scenegleton<T> : MonoBehaviour where T : Component
{
    public static T Instance;

    public virtual void Awake() => Instance = this as T;
}
