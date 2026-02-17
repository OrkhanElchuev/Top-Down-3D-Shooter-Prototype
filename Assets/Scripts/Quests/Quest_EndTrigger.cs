using UnityEngine;

public class Quest_EndTrigger : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != player)
            return;

        if (QuestManager.instance.QuestCompleted())
            Debug.Log("Level Completed");
    }
}
