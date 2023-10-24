using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    [SerializeField, Tooltip("Amount of gravity to be applied to objects")]
    public float gravity = -10;
    public float objRotSpeed = 50;
    
    //privates
    public HashSet<Rigidbody> AffectedBodies = new HashSet<Rigidbody>();
    private Rigidbody _rigidbody;
    private float _distance;
    private float _force;

    private void Awake()
    {
        if (!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        foreach (var body in AffectedBodies)
        {
            if (!body) return;
            
            FakeGravity(body);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.attachedRigidbody) return;
        if (other.CompareTag("Food") && other.GetComponent<Food>().isAbsorbed) return;
        
        AffectedBodies.Add(other.attachedRigidbody);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody) AffectedBodies.Remove(other.attachedRigidbody);
    }
    
    public void FakeGravity(Rigidbody rigidBody)
    {
        // set planet gravity direction for the object body
        Vector3 gravityDir = (rigidBody.position - transform.position).normalized;
        Vector3 bodyUp = rigidBody.transform.up;
        
        // apply gravity to objects rigidbody
        rigidBody.AddForce(gravityDir * gravity);
        
        // update the objects rotation in relation to the planet
        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityDir) * rigidBody.rotation;
        
        // smooth rotation
        rigidBody.rotation = Quaternion.Slerp(rigidBody.rotation, targetRotation, objRotSpeed * Time.deltaTime);
    }
}
