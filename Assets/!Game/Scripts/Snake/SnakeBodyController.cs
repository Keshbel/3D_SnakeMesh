using System;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBodyController : MonoBehaviour
{
    public Action OnEatingApple = null;
    
    [Header("Objects")]
    public Transform prefabsParent;
    public SnakeBodyPiece bodyPrefab;
    public List<SnakeBodyPiece> bodyObjects;

    [Header("Options")] 
    [SerializeField] private float distance = 1;
    
    private void FixedUpdate()
    {
        for (var index = 0; index < bodyObjects.Count; index++)
        {
            var bodyObject = bodyObjects[index];
            var followObject = index-1 >= 0 ? bodyObjects[index-1].gameObject : gameObject;
            
            bodyObject.transform.LookAt(followObject.transform);
            if (Vector3.Distance(bodyObject.transform.position, followObject.transform.position) > distance) 
                bodyObject.rb.MovePosition(bodyObject.rb.position + bodyObject.transform.forward * (GameSingleton.Instance.snakeHeadController.MoveSpeed * Time.fixedDeltaTime));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food")) other.GetComponent<Food>().isAbsorbed = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Food")) FakeGravity(other.attachedRigidbody);
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Food")) other.GetComponent<Food>().isAbsorbed = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Food"))
        {
            // ideally use a prefab pool here, in this test this can be neglected
            Destroy(other.gameObject);
            GameSingleton.Instance.foodSpawner.RandomSpawn();
            
            OnEatingApple?.Invoke();
            GameSingleton.Instance.appleEatingSource.PlayOneShot(GameSingleton.Instance.appleEatingSource.clip);
            AddBodyPiece();
        }
    }

    public void AddBodyPiece()
    {
        var lastTransform = bodyObjects.Count > 0 ? bodyObjects[^1].transform : transform;
        
        var piece = Instantiate(bodyPrefab, prefabsParent, true);
        piece.transform.position = lastTransform.position -lastTransform.forward * distance;
        
        bodyObjects.Add(piece);
    }
    
    public void FakeGravity(Rigidbody rigidBody)
    {
        // set planet gravity direction for the object body
        Vector3 gravityDir = (rigidBody.position - transform.position).normalized;
        
        // apply gravity to objects rigidbody
        rigidBody.AddForce(gravityDir * GameSingleton.Instance.planetGravity.gravity * 2);
    }
}
