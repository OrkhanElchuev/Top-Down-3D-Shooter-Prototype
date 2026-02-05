using System.Collections;
using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// Centralized object pooling system.
/// Reuses GameObjects instead of instantiating/destroying them repeatedly,
/// improving performance and reducing garbage collection.
///
/// Objects are grouped by prefab type and stored in queues.
/// Each pooled object keeps a reference to its original prefab.
/// </summary>

public class ObjectPooling : MonoBehaviour
{
    // Singleton instance for easy global access
    public static ObjectPooling instance;

    [SerializeField] private int poolSize = 20;

    [Header("To Initialize")]
    [SerializeField] private GameObject weaponPickup;
    [SerializeField] private GameObject ammoPickup;
    
    // Maps each prefab to its pool of inactive instances
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        // Singleton pattern enforcement
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        // Pre-create pools for known prefabs
        InitializeNewPool(weaponPickup);
        InitializeNewPool(ammoPickup);
    }

    /// <summary>
    /// Retrieves an object from the pool.
    /// If the pool doesn't exist or is empty, it is created/expanded automatically.
    /// </summary>
    public GameObject GetObject(GameObject prefab)
    {
        // Create pool if it doesn't exist yet
        if (poolDictionary.ContainsKey(prefab) == false)
            InitializeNewPool(prefab);

        // Expand pool if empty
        if (poolDictionary[prefab].Count == 0)
            CreateNewObject(prefab);

        // Grab object from queue
        GameObject objectToGet = poolDictionary[prefab].Dequeue();

        ResetVisuals(objectToGet);

        objectToGet.SetActive(true);
        objectToGet.transform.parent = null;

        return objectToGet;
    }
    
    /// <summary>
    /// Returns an object to its pool after an optional delay.
    /// </summary>
    public void ReturnObject(GameObject objectToReturn, float delay = 0.01f)
    {
        StartCoroutine(DelayReturn(delay, objectToReturn));
    } 
        
    public IEnumerator DelayReturn(float delay, GameObject objectToReturn)
    {
        yield return new WaitForSeconds(delay);

        ReturnToPool(objectToReturn);
    }

    /// <summary>
    /// Disables an object and places it back into its original prefab pool.
    /// </summary>
    private void ReturnToPool(GameObject objectToReturn)
    {
        ResetVisuals(objectToReturn);
        
        // Retrieve the prefab this object originated from
        GameObject originalPrefab = objectToReturn.GetComponent<PooledObject>().originalPrefab;

        objectToReturn.SetActive(false);
        objectToReturn.transform.parent = transform;

        // Return to its corresponding queue
        poolDictionary[originalPrefab].Enqueue(objectToReturn);
    }

    /// <summary>
    /// Creates a new pool for the given prefab and pre-fills it.
    /// </summary>
    private void InitializeNewPool(GameObject prefab)
    {
        poolDictionary[prefab] = new Queue<GameObject>();
        
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewObject(prefab);
        }
    }
    
    /// <summary>
    /// Instantiates a new pooled object and assigns its original prefab reference.
    /// </summary>
    private void CreateNewObject(GameObject prefab)
    {
        GameObject newObject = Instantiate(prefab, transform);
        newObject.AddComponent<PooledObject>().originalPrefab = prefab;
        newObject.SetActive(false);

        poolDictionary[prefab].Enqueue(newObject);
    }

    /// <summary>
    /// Resets visual effects before reusing an object.
    /// Clears TrailRenderer artifacts for bullets.
    /// </summary>
    private static void ResetVisuals(GameObject pooledObject)
    {
        // Clear trails
        if (pooledObject.TryGetComponent(out TrailRenderer trail))
            trail.Clear();
    }
}
