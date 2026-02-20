using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            // Stops play mode in the Unity Editor
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // Quits the built application
            Application.Quit();
#endif
        }
    }
}
