using UnityEngine;

public class SnakeBodyPiece : MonoBehaviour
{
    public Rigidbody rb;

    private void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
    }
}
