using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling instance;

    [Header("Bullet Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 20;
    [SerializeField] private Transform poolParent;

    private Queue<GameObject> bulletPool = new();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        // If no parent was assigned, create one for cleanliness.
        if (poolParent == null)
            poolParent = transform;

        CreateInitialPool();
    }

    public GameObject GetBullet()
    {
        // If we ran out, grow the pool instead of failing.
        if (bulletPool.Count == 0)
            AddBulletToPool();

        GameObject bullet = bulletPool.Dequeue();

        // Reset physics state so old movement doesn't carry over.
        ResetRBPhysics(bullet);
        ResetVisuals(bullet);
        bullet.SetActive(true);

        return bullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        // Reset physics again just to be safe.
        ResetRBPhysics(bullet);
        ResetVisuals(bullet);
        bullet.SetActive(false);
        bullet.transform.SetParent(poolParent);
        
        bulletPool.Enqueue(bullet);
    }

    private static void ResetRBPhysics(GameObject bullet)
    {
        if (bullet.TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
        }
    }

    private void CreateInitialPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AddBulletToPool();
        }
    }

    private void AddBulletToPool()
    {
        GameObject newBullet = Instantiate(bulletPrefab, poolParent);
        newBullet.SetActive(false);
        bulletPool.Enqueue(newBullet);
    }

    private static void ResetVisuals(GameObject bullet)
    {
        // Clear trails.
        if (bullet.TryGetComponent(out TrailRenderer trail))
            trail.Clear();
    }
}
