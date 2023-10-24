using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Food : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    
    [Header("States")]
    public bool isAbsorbed;

    private void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
    }

    private void OnDestroy()
    {
        GameSingleton.Instance.planetGravity.AffectedBodies.Remove(rb);
    }
}


