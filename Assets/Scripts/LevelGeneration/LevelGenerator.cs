using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<Transform> levelParts;
    [SerializeField] private SnapPoint nextSnapPoint;

    [ContextMenu("Create next Level Part")]
    private void GenerateNextLevelPart()
    {
        Transform newPart = Instantiate(ChooseRandomLevelPart());
    }

    private Transform ChooseRandomLevelPart()
    {
        int randomIndex = Random.Range(0, levelParts.Count);

        Transform chosenPart = levelParts[randomIndex];

        levelParts.RemoveAt(randomIndex);

        return chosenPart;
    }
}
