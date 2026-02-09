using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Handle Bullet Collision.
/// </summary>

public class Bullet : MonoBehaviour
{    
    [Header("References")]
    [SerializeField] private Rigidbody rb;

    [Header("VFX Settings")]
    [SerializeField] private GameObject bulletHitVFX;

    private Vector3 startPosition;
    private float flyDistance;
    private float extraDistance = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        DisableBulletAtLaserTip();
    }

    private void DisableBulletAtLaserTip()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance)
            ReturnBullet();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();

        if (enemy != null)
            enemy.GetHit();

        CreateHitFX(collision);

        // Reset velocity before returning.
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        ReturnBullet();
    }

    private void ReturnBullet() => ObjectPooling.instance.ReturnObject(gameObject);

    public void BulletSetup(float flyDistance)
    {
        startPosition = transform.position;
        // extraDistance is created to make bullet fly a bit further than the tip of the laser.
        this.flyDistance = flyDistance + extraDistance;
    }

    /// <summary>
    /// Spawn a hit visual effect at the point where collision happened.
    /// </summary>
    private void CreateHitFX(Collision collision)
    {
        // Make sure the collision actually has contact points.
        if (collision.contacts.Length > 0)
        {
            // Take the first contact point of the collision. (Point of 2 colliders colliding).
            ContactPoint contact = collision.contacts[0];
            // Instantiate the hit VFX at the contact point, make it face away from the surface.
            GameObject newHitFX = ObjectPooling.instance.GetObject(bulletHitVFX);
            newHitFX.transform.position = contact.point;

            ObjectPooling.instance.ReturnObject(newHitFX, 1);
        }
    }
}
