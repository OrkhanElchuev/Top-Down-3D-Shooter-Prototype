using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling instance;

    [SerializeField] private int poolSize = 20;

    [Header("To Initialize")]
    [SerializeField] private GameObject weaponPickup;
    //[SerializeField] private GameObject ammoPickup;

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        InitializeNewPool(weaponPickup);
        //InitializeNewPool(ammoPickup);
    }

    public GameObject GetObject(GameObject prefab)
    {
        if (poolDictionary.ContainsKey(prefab) == false)
            InitializeNewPool(prefab);

        if (poolDictionary[prefab].Count == 0)
            CreateNewObject(prefab);

        GameObject objectToGet = poolDictionary[prefab].Dequeue();

        ResetVisuals(objectToGet);

        objectToGet.SetActive(true);
        objectToGet.transform.parent = null;

        return objectToGet;
    }

    public void ReturnObject(GameObject objectToReturn, float delay = 0.01f)
    {
        StartCoroutine(DelayReturn(delay, objectToReturn));
    } 
        
    public IEnumerator DelayReturn(float delay, GameObject objectToReturn)
    {
        yield return new WaitForSeconds(delay);

        ReturnToPool(objectToReturn);
    }

    private void ReturnToPool(GameObject objectToReturn)
    {
        ResetVisuals(objectToReturn);
        
        GameObject originalPrefab = objectToReturn.GetComponent<PooledObject>().originalPrefab;

        objectToReturn.SetActive(false);
        objectToReturn.transform.parent = transform;

        poolDictionary[originalPrefab].Enqueue(objectToReturn);
    }

    private void InitializeNewPool(GameObject prefab)
    {
        poolDictionary[prefab] = new Queue<GameObject>();
        
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewObject(prefab);
        }
    }

    private void CreateNewObject(GameObject prefab)
    {
        GameObject newObject = Instantiate(prefab, transform);
        newObject.AddComponent<PooledObject>().originalPrefab = prefab;
        newObject.SetActive(false);

        poolDictionary[prefab].Enqueue(newObject);
    }

    private static void ResetVisuals(GameObject bullet)
    {
        // Clear trails
        if (bullet.TryGetComponent(out TrailRenderer trail))
            trail.Clear();
    }
}
