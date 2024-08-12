using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIAgent : MonoBehaviour
{
    public AIPath aipath;
    Vector2 direction;
    void Start()
    {
     
    }

    void Update()
    {
        faceVelocity();
    }

    void faceVelocity()
    {
        direction = aipath.desiredVelocity;
        transform.right = direction;
    }
}
