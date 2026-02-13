using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }
}
