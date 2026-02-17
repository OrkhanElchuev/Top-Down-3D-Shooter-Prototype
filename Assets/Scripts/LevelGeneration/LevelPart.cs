using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents one modular piece of a level that can be aligned and snapped to a target <see cref="SnapPoint"/>.
/// 
/// Responsibilities:
/// - Finds an entrance and exit snap point among its children
/// - Rotates and positions itself so its entrance aligns with a target snap point
/// - Detects intersection with other placed parts via overlap checks
/// </summary>

public class LevelPart : MonoBehaviour
{
    #region Inspector - Intersection Check

    [Header("Intersection Check")]

    [Tooltip("Which layers count as 'solid' for intersection testing.")]
    [SerializeField] private LayerMask intersectionLayer;

    [Tooltip("Colliders used as overlap volumes to test whether this part intersects other parts.")]
    [SerializeField] private Collider[] intersectionCheckColliders;

    [Tooltip("Parent transform that identifies THIS part's intersection volume group (used to ignore self hits).")]
    [SerializeField] private Transform intersectionCheckParent;

    #endregion

    #region SnapPoint Accessors

    public SnapPoint GetEntrancePoint() => GetSnapPointOfType(SnapPointType.Enter);
    public SnapPoint GetExitPoint() => GetSnapPointOfType(SnapPointType.Exit);

    #endregion

    #region Snapping / Alignment
    
    /// <summary>
    /// Aligns rotation first, then snaps position so that this part's entrance point
    /// matches the provided target snap point.
    /// </summary>
    /// <param name="targetSnapPoint">The snap point we want to connect to.</param>
    public void SnapAndAlignPartTo(SnapPoint targetSnapPoint)
    {
        SnapPoint entrancePoint = GetEntrancePoint();

        if (entrancePoint == null)
        {
            Debug.LogError($"Part '{name}' has no Entrance SnapPoint.", this);
            return;
        }

        // Align first, then snap the position.
        AlignTo(entrancePoint, targetSnapPoint); 
        SnapTo(entrancePoint, targetSnapPoint);
    }

    #endregion

    #region Intersection Detection

    /// <summary>
    /// Checks whether any of this part's intersection-check volumes overlap colliders
    /// on <see cref="intersectionLayer"/> that belong to other placed parts.
    /// </summary>
    /// <returns>True if an intersection with another part is detected.</returns>
    public bool IntersectionDetected()
    {
        // Ensure physics world matches the latest transforms after snapping.
        Physics.SyncTransforms();

        foreach (var collider in intersectionCheckColliders)
        {
            // Use OverlapBox around each check collider's bounds.
            Collider[] hitColliders = 
                Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, Quaternion.identity, intersectionLayer);

            foreach (var hit in hitColliders)
            {
                // Only care about objects marked with IntersectionCheck somewhere in their hierarchy.
                IntersectionCheck intersectionCheck = hit.GetComponentInParent<IntersectionCheck>();

                // If it's a valid hit and not our own intersection group, it's a collision with another part.
                if (intersectionCheck != null && intersectionCheckParent != intersectionCheck.transform)
                    return true;
            }
        }

        return false;
    }

    #endregion

    #region Private Helpers - Align + Snap
    
    /// <summary>
    /// Rotates this part so that <paramref name="ownSnapPoint"/> faces and matches
    /// <paramref name="targetSnapPoint"/> (plus a 180-degree flip so entrances/exits face each other).
    /// </summary>
    private void AlignTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        // Calculate the rotation offset between the level part's current rotation
        // and it's own snap point's rotation.
        var rotationOffset = ownSnapPoint.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;

        // Level part's rotation matches the target snap point's rotation.
        transform.rotation = targetSnapPoint.transform.rotation;
        // Rotate the level part by 180 degrees, since snap points are facing
        // opposite directions.
        transform.Rotate(0, 180, 0);
        // Apply previously calculated offset.
        transform.Rotate(0, -rotationOffset, 0);
    }

    /// <summary>
    /// Moves this part so that <paramref name="ownSnapPoint"/> ends up exactly at
    /// <paramref name="targetSnapPoint"/>'s position.
    /// </summary>
    private void SnapTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        // Calculate the offset between the level part's current position and
        // it's own snap point's position. Hence the distance and direction from the level
        // part's pivot to it's snap point.
        var offset = transform.position - ownSnapPoint.transform.position;

        // Determine the new position for the level part. By adding previously calculated
        // offset to the target snap point's position. Making it align with the snap point.
        var newPosition = targetSnapPoint.transform.position + offset;

        transform.position = newPosition;
    }

    /// <summary>
    /// Finds all <see cref="SnapPoint"/> components in children and returns one that matches the requested type.
    /// If multiple exist, returns a random one.
    /// </summary>
    private SnapPoint GetSnapPointOfType(SnapPointType pointType)
    {
        SnapPoint[] snapPoints = GetComponentsInChildren<SnapPoint>();
        List<SnapPoint> filteredSnapPoints = new List<SnapPoint>();

        // Gather all snap points of the specificed level type
        foreach (SnapPoint snapPoint in snapPoints)
        {
            if (snapPoint.pointType == pointType)
                filteredSnapPoints.Add(snapPoint);
        }

        // Of there are matching snap points, choose one random
        if (filteredSnapPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, filteredSnapPoints.Count);
            return filteredSnapPoints[randomIndex];
        }

        // Return null if no matching snap points are found
        return null;
    }

    #endregion
}
