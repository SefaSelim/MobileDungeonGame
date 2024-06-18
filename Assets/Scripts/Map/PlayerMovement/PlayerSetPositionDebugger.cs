using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSetPositionDebugger : MonoBehaviour
{
    public GameObject PlayerPosition;

    // Update is called once per frame
 

   public void SettingPlayersetPosition() {
         transform.position = PlayerPosition.transform.position;
        PlayerPosition.transform.position = transform.position; 
}
}
