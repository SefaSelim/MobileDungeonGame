using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharInfos : MonoBehaviour
{
    public int karizma = 5;
    //statlar burdan



    //a buraya kadar yazılıack
    [SerializeField]
    GameObject panel; 
    [SerializeField]
    TextMeshProUGUI karizmatext;
    public void TogglePanel()
    {   
        UpdateInfos();
        panel.SetActive(!panel.activeSelf);
    }

    void UpdateInfos()
    {
        karizmatext.text = Convert.ToString(karizma);
    }
}
