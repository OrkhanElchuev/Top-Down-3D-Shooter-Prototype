using UnityEngine;

/// <summary>
/// Marker component used by <see cref="LevelPart.IntersectionDetected"/> to determine
/// whether an overlap hit belongs to a level part's intersection-check group.
/// 
/// Put this on the root transform (or a known parent) of the "intersection check" collider set.
/// </summary>

public class IntersectionCheck : MonoBehaviour
{
    // Intentionally empty (marker component).
}
