using UnityEngine;

/// <summary>
/// - "Enter" points are where the part connects FROM the previous part.
/// - "Exit" points are where the next part will connect TO.
/// </summary>
public enum SnapPointType { Enter, Exit }

/// <summary>
/// A simple marker component used to classify transforms as snap points.
/// The generator uses these to align and position parts.
/// </summary>
public class SnapPoint : MonoBehaviour
{
    [Tooltip("Defines whether this snap point is used as an entrance or exit.")]
    public SnapPointType pointType;

    private void OnValidate()
    {
        gameObject.name = "SnapPoint - " + pointType.ToString();
    }
}
