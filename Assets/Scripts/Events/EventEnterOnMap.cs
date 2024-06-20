using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventEnterOnMap : MonoBehaviour
{
    public GameObject Event;
private void OnCollisionEnter2D(Collision2D collision) {
    if (collision.collider.CompareTag("CollisionDebugger"))
    {
          Event.SetActive(true);
    }
}
}
