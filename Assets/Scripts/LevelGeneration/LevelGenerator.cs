using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Generation Settings")]
    [SerializeField] private List<Transform> levelParts;
    [SerializeField] private SnapPoint nextSnapPoint;
    [SerializeField] private float generationDelay;

    private float delayTimer;
    private List<Transform> currentLevelParts;

    private void Start()
    {
        currentLevelParts = new List<Transform>(levelParts);
    }

    private void Update()
    {
        delayTimer -= Time.deltaTime;

        if (delayTimer < 0)
        {
            delayTimer = generationDelay;
            GenerateNextLevelPart();
        }
    }

    [ContextMenu("Create next Level Part")]
    private void GenerateNextLevelPart()
    {
        Transform newPart = Instantiate(ChooseRandomLevelPart());
        LevelPart levelPartScript = newPart.GetComponent<LevelPart>();

        levelPartScript.SnapAndAlignPartTo(nextSnapPoint);
        nextSnapPoint = levelPartScript.GetExitPoint();
    }

    private Transform ChooseRandomLevelPart()
    {
        int randomIndex = Random.Range(0, currentLevelParts.Count);

        Transform chosenPart = currentLevelParts[randomIndex];

        // currentLevelParts.RemoveAt(randomIndex);

        return chosenPart;
    }
}
