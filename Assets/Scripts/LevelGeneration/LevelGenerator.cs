using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Generation Settings")]
    [SerializeField] private List<Transform> levelParts;
    [SerializeField] private Transform lastLevelPart;
    [SerializeField] private SnapPoint nextSnapPoint;
    [SerializeField] private float generationDelay;

    private float delayTimer;
    private List<Transform> currentLevelParts;

    private bool generationEnded;

    private void Start()
    {
        currentLevelParts = new List<Transform>(levelParts);
    }

    private void Update()
    {
        delayTimer -= Time.deltaTime;

        if (delayTimer < 0)
        {
            if (currentLevelParts.Count > 0)
            {
                delayTimer = generationDelay;
                GenerateNextLevelPart();
            }

            else if (generationEnded == false)
            {
                FinishGeneration();
            }
        }
    }

    private void FinishGeneration()
    {
        generationEnded = true;

        Transform finalLevelPart = Instantiate(lastLevelPart);
        LevelPart levelPartScript = finalLevelPart.GetComponent<LevelPart>();

        levelPartScript.SnapAndAlignPartTo(nextSnapPoint);
    }

    private void GenerateNextLevelPart()
    {
        Transform newPart = Instantiate(ChooseRandomLevelPart());
        LevelPart levelPartScript = newPart.GetComponent<LevelPart>();

        levelPartScript.SnapAndAlignPartTo(nextSnapPoint);

        if (levelPartScript.IntersectionDetected())
        {
            Debug.LogWarning("Intersection between levels");
        }

        nextSnapPoint = levelPartScript.GetExitPoint();
    }

    private Transform ChooseRandomLevelPart()
    {
        int randomIndex = Random.Range(0, currentLevelParts.Count);

        Transform chosenPart = currentLevelParts[randomIndex];

        currentLevelParts.RemoveAt(randomIndex);

        return chosenPart;
    }
}
