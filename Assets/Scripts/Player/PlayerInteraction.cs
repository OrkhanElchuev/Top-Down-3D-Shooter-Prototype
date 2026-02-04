using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manages player interaction logic.
/// Tracks nearby interactables and determines which one is closest,
/// enabling highlight feedback accordingly.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    [Tooltip("All interactables currently within the player's trigger range.")]
    public List<Interactable> interactables;

    private Interactable closestInteractable;

    private void Start()
    {
        Player player = GetComponent<Player>();

        player.controls.Character.Interaction.performed += ctx => InteractWithClosest();
    }

    private void InteractWithClosest()
    {
        closestInteractable?.Interaction();
    }

    /// <summary>
    /// Determines which interactable is closest to the player and
    /// updates highlight states accordingly.
    /// </summary>
    public void UpdateClosestInteractable()
    {
        // Disable highlight on the previously closest interactable
        closestInteractable?.HighlightActive(false);
        float closestDistance = float.MaxValue;

        closestInteractable = null;

        // Loop through all nearby interactables
        foreach (Interactable interactable in interactables)
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);

            // Update closest interactable if this one is nearer
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = interactable;
            }
        }

        // Highlight the new closest interactable (if any)
        closestInteractable?.HighlightActive(true);
    }
}

