using System.Collections.Generic;
using UnityEngine;

public class SnakeBodyController : MonoBehaviour
{
    private GameObject HeadObject => GameSingleton.Instance.snakeHeadController.gameObject;
    
    [Header("Objects")]
    public Transform prefabsParent;
    public SnakeBodyPiece bodyPrefab;
    public List<SnakeBodyPiece> bodyObjects;

    [Header("Options")] 
    [SerializeField] private float distance = 1;

    private void OnEnable()
    {
        GameSingleton.Instance.snakeHeadController.onEatingApple += AddBodyPiece;
    }

    private void OnDisable()
    {
        GameSingleton.Instance.snakeHeadController.onEatingApple -= AddBodyPiece;
    }

    private void FixedUpdate()
    {
        // If you need optimization, then first of all go here. Caching and a separate update system will help.
        for (var index = 0; index < bodyObjects.Count; index++)
        {
            var bodyObject = bodyObjects[index];
            var followObject = index-1 >= 0 ? bodyObjects[index-1].gameObject : HeadObject;
            
            bodyObject.transform.LookAt(followObject.transform);
            if (Vector3.Distance(bodyObject.transform.position, followObject.transform.position) > distance) 
                bodyObject.rb.MovePosition(bodyObject.rb.position + bodyObject.transform.forward * (GameSingleton.Instance.snakeHeadController.MoveSpeed * Time.fixedDeltaTime));
        }
    }

    public void AddBodyPiece()
    {
        var lastTransform = bodyObjects.Count > 0 ? bodyObjects[^1].transform : HeadObject.transform;
        
        var piece = Instantiate(bodyPrefab, prefabsParent, true);
        piece.transform.position = lastTransform.position -lastTransform.forward * distance;
        
        bodyObjects.Add(piece);
    }
}
