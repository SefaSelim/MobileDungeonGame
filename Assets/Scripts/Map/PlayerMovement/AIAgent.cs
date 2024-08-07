using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIAgent : MonoBehaviour
{
    private AIPath path;
    [SerializeField] private float movespeed;
    [SerializeField] private Transform target;
    void Start()
    {
        path = GetComponent<AIPath>();        
    }

    void Update()
    {
        path.maxSpeed = movespeed;
        path.destination = target.position;
    }
}
