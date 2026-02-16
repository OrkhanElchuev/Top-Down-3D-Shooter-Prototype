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
    private List<Transform> generatedLevelParts;


    private void Start()
    {
        defaultSnapPoint = nextSnapPoint;
        generatedLevelParts = new List<Transform>();
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

    [ContextMenu("Restart Generation")]
    private void InitializeGeneration()
    {
        nextSnapPoint = defaultSnapPoint;
        generationEnded = false;
        currentLevelParts = new List<Transform>(levelParts);

        foreach (Transform item in generatedLevelParts)
        {
            Destroy(item.gameObject);
        }

        generatedLevelParts.Clear();
    }

    private void FinishGeneration()
    {
        generationEnded = true;
        GenerateNextLevelPart();
    }

    private void GenerateNextLevelPart()
    {
        Transform newPart = null;

        if (generationEnded)
            newPart = Instantiate(lastLevelPart);
        else
            newPart = Instantiate(ChooseRandomLevelPart());

        generatedLevelParts.Add(newPart);

        // Transform newPart = Instantiate(ChooseRandomLevelPart());
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
