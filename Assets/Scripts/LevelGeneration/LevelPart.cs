using System.Collections.Generic;
using UnityEngine;

public class LevelPart : MonoBehaviour
{
    public SnapPoint GetEntrancePoint() => GetSnapPointOfType(SnapPointType.Enter);
    public SnapPoint GetExitPoint() => GetSnapPointOfType(SnapPointType.Exit);

    public void SnapAndAlignPartTo(SnapPoint targetSnapPoint)
    {
        SnapPoint entrancePoint = GetEntrancePoint();

        // Align first, then snap the position.
        AlignTo(entrancePoint, targetSnapPoint); 
        SnapTo(entrancePoint, targetSnapPoint);
    }

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
}
