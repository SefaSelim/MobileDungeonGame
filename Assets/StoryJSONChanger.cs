using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storyJSONChanger : MonoBehaviour
{
    public StoryManagementScript storyManagementScript;
    public string dialogeName;

private void OnCollisionEnter2D(Collision2D collision) {
    if (collision.collider.CompareTag("CollisionDebugger"))
    {

          storyManagementScript.jsonName = dialogeName;
    }
}
}
