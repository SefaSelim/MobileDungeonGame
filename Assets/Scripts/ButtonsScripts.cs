using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonsScripts : MonoBehaviour
{
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
}
