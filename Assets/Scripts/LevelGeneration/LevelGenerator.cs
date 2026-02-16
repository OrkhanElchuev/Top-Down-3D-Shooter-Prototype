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
    private bool generationEnded;

    private SnapPoint defaultSnapPoint;
    private List<Transform> currentLevelParts;
    private List<Transform> generatedLevelParts = new List<Transform>();


    private void Start()
    {
        defaultSnapPoint = nextSnapPoint;
        InitializeGeneration();
    }

    private void Update()
    {
        if (generationEnded)
            return;

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

    [ContextMenu("Restart Generation")]
    private void InitializeGeneration()
    {
        nextSnapPoint = defaultSnapPoint;
        generationEnded = false;
        currentLevelParts = new List<Transform>(levelParts);

        DestroyOldLevelParts();
    }

    private void DestroyOldLevelParts()
    {
        foreach (Transform item in generatedLevelParts)
        {
            Destroy(item.gameObject);
        }

        generatedLevelParts = new List<Transform>();
    }

    private void FinishGeneration()
    {
        generationEnded = true;
        GenerateNextLevelPart();
    }

    [ContextMenu("Create Next Level Part")]
    private void GenerateNextLevelPart()
    {
        Transform newPart = null;

        if (generationEnded)
            newPart = Instantiate(lastLevelPart);
        else
            newPart = Instantiate(ChooseRandomLevelPart());

        generatedLevelParts.Add(newPart);

        LevelPart levelPartScript = newPart.GetComponent<LevelPart>();
        levelPartScript.SnapAndAlignPartTo(nextSnapPoint);

        if (levelPartScript.IntersectionDetected())
        {
            InitializeGeneration();
            return;
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
