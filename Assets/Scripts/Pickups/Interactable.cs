using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Represents an object in the world that the player can interact with.
/// Handles proximity detection.
/// </summary>

public class Interactable : MonoBehaviour
{
    [Tooltip("Material applied when this interactable is highlighted.")]
    [SerializeField] private Material highlightMaterial;
    
    protected Material defaultMaterial;
    protected MeshRenderer mesh;

    private void Start()
    {
        // If no MeshRenderer is assigned in the Inspector,
        // try to find one on this object or its children
        if (mesh == null)
            mesh = GetComponentInChildren<MeshRenderer>();

        // Store the default material so we can revert later
        defaultMaterial = mesh.sharedMaterial;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            HighlightActive(true);
    }

    /// <summary>
    /// Enables or disables the highlight visual for this interactable.
    /// </summary>
    public void HighlightActive(bool active)
    {
        if (active)
            mesh.material = highlightMaterial;
        else
            mesh.material = defaultMaterial;
    }

    public virtual void Interaction()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }

    protected void UpdateMeshAndMaterial(MeshRenderer newMesh)
    {
        mesh = newMesh;
        defaultMaterial = newMesh.sharedMaterial;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        // Try to get the PlayerInteraction component from the entering object
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        // Ignore anything that isn't the player
        if (playerInteraction == null)
            return;
        // Register this interactable with the player
        playerInteraction.GetInteractables().Add(this);
        // Recalculate which interactable is closest
        playerInteraction.UpdateClosestInteractable();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        // Try to get the PlayerInteraction component from the exiting object
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        // Ignore anything that isn't the player
        if (playerInteraction == null)
            return;
        // Remove this interactable from the player's list
        playerInteraction.GetInteractables().Remove(this);
        // Recalculate which interactable is closest
        playerInteraction.UpdateClosestInteractable();
    }
}
