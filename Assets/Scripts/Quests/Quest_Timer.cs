using UnityEngine;

[CreateAssetMenu(fileName = "New Timer Quest", menuName = "Quest / Timer Quest")]
public class Quest_Timer : Quest
{
    public float time;
    private float currentTime;

    public override void StartQuest()
    {
        currentTime = time;
    }

    public override bool QuestCompleted()
    {
        return currentTime > 0;
    }

    public override void UpdateQuest()
    {
        currentTime -= Time.deltaTime;

        if (currentTime < 0)
        {
            Debug.Log("Game Over!");
        }

        string timeText = System.TimeSpan.FromSeconds(currentTime).ToString("mm':'ss");
        Debug.Log(timeText);
    }
}
