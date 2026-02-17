using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public Quest currentQuest;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentQuest?.StartQuest();
    }

    private void Update()
    {
        currentQuest?.UpdateQuest();
    }

    private void StartQuest() => currentQuest.StartQuest();
    public bool QuestCompleted() => currentQuest.QuestCompleted();
}
