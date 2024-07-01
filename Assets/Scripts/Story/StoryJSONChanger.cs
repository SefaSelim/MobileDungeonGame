using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storyJSONChanger : MonoBehaviour
{
    public QuestArrow questArrow;
    public GameObject nextTargetPointToQuestPointer;
    public StoryManagementScript storyManagementScript;
    public string dialogeName;

private void OnCollisionEnter2D(Collision2D collision) {
    if (collision.collider.CompareTag("CollisionDebugger"))
    {
            questArrow.target = nextTargetPointToQuestPointer.transform;
          storyManagementScript.jsonName = dialogeName;
            storyManagementScript.nextDialoge();




            gameObject.SetActive(false);
            nextTargetPointToQuestPointer.SetActive(true);

    }
}
}
