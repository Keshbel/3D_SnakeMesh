using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SnakeHeadController : MonoBehaviour
{
    public Action onEatingApple = null;
    
    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private VariableJoystick variableJoystick;
    [SerializeField] private Transform playerMesh;
    [SerializeField] private Transform cameraTransform;

    [Header("Options")] 
    public readonly float MoveSpeed = 5;

    //cache
    private Vector3 _moveVector;

    private void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!playerMesh) playerMesh = transform.GetChild(0).transform;

        _moveVector = transform.forward;
    }

    private void Update()
    {
        // moveVector:
        if (variableJoystick.Direction.magnitude > 0)
        {
            _moveVector = new Vector3(variableJoystick.Horizontal, 0, variableJoystick.Vertical).normalized;
            
            // rotate player to face the right direction
            RotateForward();
        }
    }

    private void FixedUpdate()
    {
        // update movement
        rb.MovePosition(rb.position + playerMesh.forward * (MoveSpeed * Time.fixedDeltaTime)); // + transform.TransformDirection(_moveVector * (moveSpeed * Time.fixedDeltaTime)));
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food")) other.GetComponent<Food>().isAbsorbed = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Food")) FakeGravityForFood(other.attachedRigidbody);
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
            
            onEatingApple?.Invoke();
            GameSingleton.Instance.appleEatingSource.PlayOneShot(GameSingleton.Instance.appleEatingSource.clip);
        }
    }
    
    /// <summary>
    /// Rotate player to face direction of movement
    /// </summary>
    private void RotateForward()
    {
        // camera anomaly
        if (Math.Abs(cameraTransform.forward.z - (-1)) < 0.02f) return;
        
        Vector3 direction = _moveVector;
        
        // calculate angle and rotation
        float angleDirection = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        
        //camera angle difference
        var camDir = transform.InverseTransformDirection(cameraTransform.up);
        float camAngle = Mathf.Atan2(camDir.x,camDir.z) * Mathf.Rad2Deg;
        float angle = camAngle + angleDirection;
        
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up);
        
        // only update rotation if direction greater than zero
        if (Vector3.Magnitude(variableJoystick.Direction) > 0.0f)
        {
            playerMesh.localRotation = targetRotation;
        }
    }
    
    public void FakeGravityForFood(Rigidbody rigidBody)
    {
        // set planet gravity direction for the object body
        Vector3 gravityDir = (rigidBody.position - transform.position).normalized;
        
        // apply gravity to objects rigidbody
        rigidBody.AddForce(gravityDir * GameSingleton.Instance.planetGravity.gravity * 2);
    }
}
