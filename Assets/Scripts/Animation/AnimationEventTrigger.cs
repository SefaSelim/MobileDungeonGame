using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventTrigger : MonoBehaviour
{
   public StoryManagementScript storyManagementScript;
   public int index;

   public void ButtonAnim()
   {
    storyManagementScript.OnOptionSelected(index);
   }
}
