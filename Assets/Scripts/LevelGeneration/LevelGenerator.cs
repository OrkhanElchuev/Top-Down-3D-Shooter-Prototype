using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns a sequence of modular level parts by "snapping" each newly spawned part
/// to the previous part's exit <see cref="SnapPoint"/>. 
/// 
/// Workflow:
/// 1) Start with an initial snap point (default).
/// 2) Instantiate a random part from a list, align + snap it.
/// 3) If it intersects something, restart and try again from scratch.
/// 4) When all random parts are used, spawn a final "last part".
/// </summary>

public class LevelGenerator : MonoBehaviour
{
    #region Inspector - Generation Settings

    [Header("Generation Settings")]

    [Tooltip("All possible level parts to generate (each should have a LevelPart component).")]
    [SerializeField] private List<Transform> levelParts;

    [Tooltip("The final piece spawned after all random parts are placed (end room).")]
    [SerializeField] private Transform lastLevelPart;

    [Tooltip("The starting SnapPoint where the very first piece will attach.")]
    [SerializeField] private SnapPoint nextSnapPoint;

    [Tooltip("Delay (seconds) between spawning successive level parts at runtime.")]
    [SerializeField] private float generationDelay = 0.1f;

    #endregion

    #region Runtime State

    private float delayTimer;
    private bool generationEnded;

    #endregion

    private SnapPoint defaultSnapPoint; 
    private List<Transform> currentLevelParts;
    private List<Transform> generatedLevelParts = new List<Transform>();

    #region Unity Lifecycle

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
            // Still have parts to place -> place next piece.
            if (currentLevelParts.Count > 0)
            {
                delayTimer = generationDelay;
                GenerateNextLevelPart();
            }
            // No parts left -> finish by spawning the last part once.
            else if (generationEnded == false)
            {
                FinishGeneration();
            }
        }
    }
    
    #endregion

    #region Generation Control

    /// <summary>
    /// Resets generator state and restarts the generation process.
    /// Also destroys previously spawned level parts.
    /// </summary>
    [ContextMenu("Restart Generation")]
    private void InitializeGeneration()
    {
        nextSnapPoint = defaultSnapPoint;
        generationEnded = false;

        // Copy list so we can remove as we use parts, without modifying the original inspector list.
        currentLevelParts = new List<Transform>(levelParts);

        DestroyOldLevelParts();
    }

    /// <summary>
    /// Destroys all spawned level parts from the previous attempt/run.
    /// </summary>    
    private void DestroyOldLevelParts()
    {
        foreach (Transform item in generatedLevelParts)
        {
            if (item != null)
                Destroy(item.gameObject);
        }

        generatedLevelParts = new List<Transform>();
    }

    /// <summary>
    /// Marks generation as ended and spawns the final part.
    /// </summary>
    private void FinishGeneration()
    {
        generationEnded = true;
        GenerateNextLevelPart();
    }

    #endregion

    #region Spawning

    /// <summary>
    /// Instantiates the next level part (random during generation, otherwise the last part),
    /// aligns + snaps it to <see cref="nextSnapPoint"/>, checks intersections, and updates
    /// <see cref="nextSnapPoint"/> to the newly placed part's exit snap point.
    /// </summary>
    [ContextMenu("Create Next Level Part")]
    private void GenerateNextLevelPart()
    {
        Transform newPart;
        
        // Choose what to spawn: random part vs final part.
        if (generationEnded)
            newPart = Instantiate(lastLevelPart);
        else
            newPart = Instantiate(ChooseRandomLevelPart());

        generatedLevelParts.Add(newPart);

        // Required component: LevelPart
        LevelPart levelPartScript = newPart.GetComponent<LevelPart>();

        // Snap and align this part so its entrance connects to the current "nextSnapPoint".
        levelPartScript.SnapAndAlignPartTo(nextSnapPoint);

        // If this placement intersects existing geometry, restart generation from scratch.
        if (levelPartScript.IntersectionDetected())
        {
            InitializeGeneration();
            return;
        }

        // Continue from the newly placed part's exit point.
        nextSnapPoint = levelPartScript.GetExitPoint();
    }

    /// <summary>
    /// Picks and removes a random level part from <see cref="currentLevelParts"/>.
    /// This ensures parts are used at most once per run.
    /// </summary>
    private Transform ChooseRandomLevelPart()
    {
        int randomIndex = Random.Range(0, currentLevelParts.Count);
        Transform chosenPart = currentLevelParts[randomIndex];

        currentLevelParts.RemoveAt(randomIndex);
        return chosenPart;
    }

    #endregion
}
