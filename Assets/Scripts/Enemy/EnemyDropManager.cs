using UnityEngine;

public class EnemyDropManager : MonoBehaviour
{
    public void DropItems()
    {
        Debug.Log("Dropped Some Item");
    }

    private void CreateItem(GameObject go)
    {
        GameObject newItem = Instantiate(go, transform.position + Vector3.up, Quaternion.identity);
    }
}
