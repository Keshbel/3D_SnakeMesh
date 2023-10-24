using TMPro;
using UnityEngine;

public class AppleEatenCounter : MonoBehaviour
{
    [Header("Data")] 
    [SerializeField] private int appleCount;
    
    [Header("UI")]
    [SerializeField] private TMP_Text tmpText;

    private void OnEnable()
    {
        GameSingleton.Instance.snakeBodyController.OnEatingApple += OnCountUpdate;
    }

    private void OnDisable()
    {
        GameSingleton.Instance.snakeBodyController.OnEatingApple -= OnCountUpdate;
    }

    private void OnCountUpdate()
    {
        appleCount++;
        tmpText.text = appleCount.ToString();
    }
}
