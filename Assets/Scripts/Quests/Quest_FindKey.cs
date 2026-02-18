using UnityEngine;

[CreateAssetMenu(fileName = "New Key Quest", menuName = "Quest / Key Quest")]
public class Quest_FindKey : Quest
{   
    [SerializeField] private GameObject key;
    private bool keyFound;

    public override void StartQuest()
    {
        
    }

    public override bool QuestCompleted()
    {
        return keyFound;
    }

    private void PickUpKey()
    {
        keyFound = true;
    }
}
