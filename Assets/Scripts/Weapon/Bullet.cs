using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Handle Bullet Collision.
/// </summary>

public class Bullet : MonoBehaviour
{
    // REFERENCES
    private Rigidbody rb => GetComponent<Rigidbody>();
    
    [Header("VFX Settings")]
    [SerializeField] private GameObject bulletHitVFX;

    private float destroyDelayOfVFX = 1f;

    private void OnCollisionEnter(Collision collision)
    {
        CreateHitFX(collision);
        Destroy(gameObject);
    }

    /// <summary>
    /// Spawn a hit visual effect at the point where collision happened.
    /// </summary>
    /// <param name="collision"></param>
    private void CreateHitFX(Collision collision)
    {
        // Make sure the collision actually has contact points.
        if (collision.contacts.Length > 0)
        {
            // Take the first contact point of the collision. (Point of 2 colliders colliding).
            ContactPoint contact = collision.contacts[0];
            // Instantiate the hit VFX at the contact point, make it face away from the surface.
            GameObject newHitFX = Instantiate(bulletHitVFX, contact.point, Quaternion.LookRotation(contact.normal));

            Destroy(newHitFX, destroyDelayOfVFX);
        }
    }
}
