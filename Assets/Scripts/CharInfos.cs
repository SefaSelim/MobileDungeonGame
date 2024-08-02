using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharInfos : MonoBehaviour
{

    public Unit unit;
    public int charisma = 5;
    public int dexterity = 5;
    public int strength = 5;
    public int intelligence = 5;


    //statlar burdan



    //a buraya kadar yazılıack
    [SerializeField]
    GameObject panel;
    public Button CharismaAddButton;
    public Button IntelligenceAddButton;
    public Button StrengthAddButton;
    public Button DexterityAddButton;


    public TextMeshProUGUI Charismatext;
    public TextMeshProUGUI Dexteritytext;
    public TextMeshProUGUI Intelligencetext;
    public TextMeshProUGUI Strengthtext;
    public TextMeshProUGUI Leveltext;
    public TextMeshProUGUI StatPointtext;

    public void TogglePanel()
    {
        UpdateInfos();
        panel.SetActive(!panel.activeSelf);
    }


    void UpdateInfos()
    {
        Intelligencetext.text = Convert.ToString(intelligence);
        Strengthtext.text = Convert.ToString(strength);
        Dexteritytext.text = Convert.ToString(dexterity);
        Charismatext.text = Convert.ToString(charisma);
        Leveltext.text = "Level : " + Convert.ToString(unit.unitLevel);
        StatPointtext.text = "Stat Point : " + Convert.ToString(unit.statpoint);
    }

    public void AddStatCharisma()
    {
        if (unit.statpoint > 0)
        {
            charisma++;
            unit.statpoint--;
        }
        UpdateInfos();
    }
    public void AddStatIntelligence()
    {
        if (unit.statpoint > 0)
        {
            intelligence++;
            unit.statpoint--;
        }
        UpdateInfos();
    }
    public void AddStatDexterity()
    {
        if (unit.statpoint > 0)
        {
            dexterity++;
            unit.statpoint--;
        }
        UpdateInfos();
    }
    public void AddStatStrength()
    {
        if (unit.statpoint > 0)
        {
            strength++;
            unit.statpoint--;
        }
        UpdateInfos();
    }
    public void LevelUpButton()
    {
        unit.LevelUp();
        UpdateInfos();
    }
}
