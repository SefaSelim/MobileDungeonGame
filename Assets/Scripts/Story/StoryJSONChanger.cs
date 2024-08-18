using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public class DialogCondition
{
  public string stringValue; 
}


public class StoryJSONChanger : MonoBehaviour
{
  public Unit unit;
  public QuestArrow questArrow;
  public GameObject nextTargetPointToQuestPointer;
  public StoryManagementScript storyManagementScript;
  public string completedDialogName;
  public string notCompletedDialogName;
  public string locationDialogName;
  public List<DialogCondition> dialogConditions;
  public QuestManager questManager;
  int currentquestnumber = 0;
  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.collider.CompareTag("CollisionDebugger"))
    {
      if (nextTargetPointToQuestPointer.transform == transform)
      {
        if (questManager.IsQuestCompleted(questManager.GetQuestByName(dialogConditions[0+currentquestnumber].stringValue)))
        {
          questArrow.target = nextTargetPointToQuestPointer.transform;
          storyManagementScript.jsonName = completedDialogName;
          storyManagementScript.nextDialoge();

          currentquestnumber++;
          nextTargetPointToQuestPointer.SetActive(true);
        }
        else
        {
          storyManagementScript.jsonName = notCompletedDialogName;
          storyManagementScript.nextDialoge();
        }
      }
      else
      {
        storyManagementScript.jsonName = locationDialogName;
        storyManagementScript.nextDialoge();
      }
    }
   
  }
 





}
