using UnityEngine;

public abstract class Quest : ScriptableObject
{
    public string questName;

    [TextArea]
    public string questDescription;

    public abstract void StartQuest();
    public abstract bool QuestCompleted();

    public virtual void UpdateQuest(){}
}
