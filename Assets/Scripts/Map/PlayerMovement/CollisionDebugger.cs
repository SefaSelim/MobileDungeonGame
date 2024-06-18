using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDebugger : MonoBehaviour
{
    public PlayerMovement playerMovement;
    private void OnCollisionEnter2D(Collision2D other) {
        playerMovement.CollisionDetected();    
    }



}
