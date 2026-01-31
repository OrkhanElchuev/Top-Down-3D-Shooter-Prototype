using UnityEngine;

/// <summary>
/// Handle Bullet Collision.
/// </summary>

public class Bullet : MonoBehaviour
{
    // REFERENCES
    private Rigidbody rb => GetComponent<Rigidbody>();

    private void OnCollisionEnter(Collision collision)
    {
        // Stop moving the bullet after it hits an object.
        // rb.constraints = RigidbodyConstraints.FreezeAll;
        Destroy(gameObject);
    }
}
