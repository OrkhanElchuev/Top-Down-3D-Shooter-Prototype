using UnityEngine;

[CreateAssetMenu(fileName = "New Key Quest", menuName = "Quest / Key Quest")]
public class Quest_FindKey : Quest
{   
    [SerializeField] private GameObject key;
    private bool keyFound;

    public override void StartQuest()
    {
        QuestObject_Key.OnKeyPickedUp += PickUpKey;
    }

    public override bool QuestCompleted()
    {
        return keyFound;
    }

    private void PickUpKey()
    {
        keyFound = true;
        QuestObject_Key.OnKeyPickedUp -= PickUpKey;
        Debug.Log("Key is Picked up");
    }
}
