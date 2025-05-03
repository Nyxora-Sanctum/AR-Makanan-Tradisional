using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class ARClickSpawner : MonoBehaviour
{
    [Header("SPAWN SETTINGS")]
    [Tooltip("List of child objects to animate (must be disabled by default)")]
    public List<FallingObject> fallingObjects = new List<FallingObject>();

    [Header("COLLIDER (REQUIRED)")]
    [Tooltip("The collider that detects touches/clicks")]
    public Collider targetCollider;

    [Header("ANIMATION SETTINGS")]
    public float spawnHeight = 2f; // Height above original position

#if ENABLE_INPUT_SYSTEM
    private Mouse mouse;
    private Touchscreen touchscreen;
#endif

    private int currentIndex = 0; // Tracks which object to spawn next

    private void Start()
    {
        // Auto-detect collider if not assigned
        if (targetCollider == null)
        {
            targetCollider = GetComponent<Collider>();
            Debug.LogWarning(targetCollider ?
                "Auto-assigned collider: " + targetCollider.name :
                "❌ No collider found on this object!");
        }

        // Initialize all objects as disabled
        foreach (var obj in fallingObjects)
        {
            if (obj.targetObject != null)
                obj.targetObject.gameObject.SetActive(false);
        }

#if ENABLE_INPUT_SYSTEM
        mouse = Mouse.current;
        touchscreen = Touchscreen.current;
#endif
    }

    private void Update()
    {
        // Don't process input if all objects have fallen
        if (currentIndex >= fallingObjects.Count) return;

        bool inputDetected = false;
        Vector2 inputPosition = Vector2.zero;

#if ENABLE_INPUT_SYSTEM
        if (touchscreen != null && touchscreen.primaryTouch.press.isPressed)
        {
            inputDetected = true;
            inputPosition = touchscreen.primaryTouch.position.ReadValue();
        }
        else if (mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            inputDetected = true;
            inputPosition = mouse.position.ReadValue();
        }
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            inputDetected = true;
            inputPosition = Input.GetTouch(0).position;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            inputDetected = true;
            inputPosition = Input.mousePosition;
        }
#endif

        if (inputDetected)
        {
            HandleInteraction(inputPosition);
        }
    }

    private void HandleInteraction(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (targetCollider.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log("🎯 Click/Touch detected!");
            StartCoroutine(AnimateNextObject());
        }
    }

    private IEnumerator AnimateNextObject()
    {
        // Check if we have any objects left to spawn
        if (currentIndex >= fallingObjects.Count) yield break;

        var obj = fallingObjects[currentIndex];
        currentIndex++; // Move to next object for next click

        if (obj.targetObject == null) yield break;

        // Store original position
        Vector3 originalPos = obj.targetObject.localPosition;

        // Set starting position (above original)
        obj.targetObject.localPosition = originalPos + Vector3.up * spawnHeight;
        obj.targetObject.gameObject.SetActive(true);

        // Animate back to original position
        yield return obj.targetObject.DOLocalMove(originalPos, obj.fallDuration)
            .SetEase(obj.fallEase)
            .WaitForCompletion();
    }

    public void ResetAllObjects()
    {
        currentIndex = 0; // Reset the counter
        foreach (var obj in fallingObjects)
        {
            if (obj.targetObject != null)
            {
                obj.targetObject.gameObject.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        ResetAllObjects();
    }
}

[System.Serializable]
public class FallingObject
{
    public Transform targetObject;
    public float fallDuration = 1f;
    public Ease fallEase = Ease.OutBounce;
}