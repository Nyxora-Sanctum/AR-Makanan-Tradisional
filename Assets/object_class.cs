using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class FallingObjectConfig
{
    [Tooltip("Prefab of the object that will fall from above")]
    public GameObject prefab;

    [Tooltip("Local position where this object should land relative to the main object")]
    public Vector3 targetLocalPosition;

    [Tooltip("Rotation to apply when the object lands (Euler angles)")]
    public Vector3 targetRotation;

    [Tooltip("Scale to apply to the object when it lands")]
    public Vector3 targetScale = Vector3.one;

    [Tooltip("Height from which this object will start falling")]
    public float spawnHeight = 3f;

    [Tooltip("Time it takes for this object to fall")]
    public float fallDuration = 1f;

    [Tooltip("Delay before this object starts falling")]
    public float spawnDelay = 0f;

    [Tooltip("Ease type for the falling animation")]
    public Ease fallEase = Ease.OutBounce;
}