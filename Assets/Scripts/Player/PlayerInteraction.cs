using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public List<Interactable> interactables;

    private Interactable closestInteracatable;

    public void UpdateClosestInteractable()
    {
        closestInteracatable?.HighlightActive(false);
        float closestDistance = float.MaxValue;

        foreach (Interactable interactable in interactables)
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteracatable = interactable;
            }
        }

        closestInteracatable?.HighlightActive(true);
    }
}

