using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class GameSingleton : MonoBehaviour
{
    [Header("Audio")] 
    public AudioSource appleEatingSource;
    
    [Header("Gravity")]
    public PlanetGravity planetGravity;
    
    [Header("Snake")]
    public SnakeHeadController snakeHeadController;
    public SnakeBodyController snakeBodyController;
    
    [Header("Food")]
    public FoodSpawner foodSpawner;

    #region Singleton

    public static GameSingleton Instance;

    private void Awake()
    {
        if (Instance) Destroy(gameObject);
        else Instance = this;
    }

    #endregion
}
