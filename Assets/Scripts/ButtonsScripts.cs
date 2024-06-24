using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using JetBrains.Annotations;

public class ButtonsScripts : MonoBehaviour
{
    public GameObject historyPanel;
    public TextMeshProUGUI historyText;

    private StoryManagementScript storyManager;

    void Start()
    {
        storyManager = FindObjectOfType<StoryManagementScript>();
    }
    // public GameObject inventoryPanel;
    public void GoToMainMenu()
    {
        Debug.Log("Ana Menüye gidildi.");
        // SceneManager.LoadScene("MainMenu");
    }

    public void OpenInventory()
    {
        Debug.Log("Envanter açıldı");
        // bool isActive = inventoryPanel.activeSelf;
        // inventoryPanel.SetActive(!isActive);
    }

    public void ShowHistory()
    {
    if (storyManager != null)
    {
        historyPanel.SetActive(true);
        historyText.text = string.Join("\n", storyManager.history);
    }
    else
    {
        Debug.LogWarning("StoryManagementScript not found!");
    }
    }
    public void CloseHistory()
    {
        historyPanel.SetActive(false);
    }
}
