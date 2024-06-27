using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

public class StoryManagementScript : MonoBehaviour
{
    [System.Serializable]
    public class StoryNode
    {
        [TextArea(3, 10)]
        public string storyText;
        public string dialogueText;
        public string[] options;
        public int[] nextNodes;
        public string[] actions;
    }

    [System.Serializable]
    public class StoryNodeWrapper
    {
        public StoryNode[] nodes;
    }

    public TextMeshProUGUI storyTextUI;
    public Button[] optionButtons;
    public StoryNode[] storyNodes;

    public TextMeshProUGUI dialogueTextUI;
    public Image char_image;
    public string image_name;
    private int currentNode;
    private bool canSelectOption = true;
    public string jsonName;

    public GameObject fightpoint;

    FightPoint fpscript;
    
    public List<string> history = new List<string>();
    public GameObject historyPanel;
    public TextMeshProUGUI historyText;


    void Start()
    {
        LoadStoryNodesFromJSON();
        currentNode = 0;
        UpdateStory();
        
    }

    void Update()
    {
        if (dialogueTextUI.text == "")
        {
            char_image.gameObject.SetActive(false);
            dialogueTextUI.gameObject.SetActive(false);
        }
        else
        {
            char_image.gameObject.SetActive(true);
            dialogueTextUI.gameObject.SetActive(true);
        }
    }

    public void OnOptionSelected(int optionIndex)
    {
        if (!canSelectOption)
        {
            return;
        }

        if (optionIndex < 0 || optionIndex >= storyNodes[currentNode].nextNodes.Length)
        {
            Debug.LogError("Invalid option index: " + optionIndex);
            return;
        }

        StoryNode currentNodeData = storyNodes[currentNode];
        if (!string.IsNullOrEmpty(currentNodeData.storyText))
        {
            history.Add("OLAY: " + currentNodeData.storyText);
        }
        if (!string.IsNullOrEmpty(currentNodeData.dialogueText))
        {
            history.Add("O: " + currentNodeData.dialogueText);
        }
        history.Add("SEN: " + currentNodeData.options[optionIndex]);

        currentNode = storyNodes[currentNode].nextNodes[optionIndex];
        UpdateStory();
    }

    void UpdateStory()
    {
        canSelectOption = false; // Seçenekleri devre dışı bırak

        StoryNode node = storyNodes[currentNode];
        storyTextUI.text = node.storyText;
        dialogueTextUI.text = node.dialogueText;

        // Eylemleri gerçekleştir
        PerformActions(node.actions);

        // Mevcut düğümün son düğüm olup olmadığını kontrol et
        if (node.nextNodes.Length == 0)
        {
            // Seçenek düğmelerini devre dışı bırak
            foreach (var button in optionButtons)
            {
                button.gameObject.SetActive(false);
            }
            return;
        }

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < node.options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = node.options[i];
                int index = i; // Closure probleminden kaçınmak için
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }

        StartCoroutine(EnableOptionsAfterDelay(0.5f)); // Seçenekleri 0.5 saniye sonra yeniden etkinleştir
    }

    IEnumerator EnableOptionsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canSelectOption = true; // Seçenekleri tekrar etkinleştir
    }

    void LoadStoryNodesFromJSON()
    {
        TextAsset jsonText = Resources.Load<TextAsset>(jsonName);
        Debug.Log("Trying to load JSON file.");
        if (jsonText != null)
        {
            Debug.Log("JSON file loaded successfully: " + jsonText.text);
            StoryNodeWrapper wrapper = JsonUtility.FromJson<StoryNodeWrapper>(jsonText.text);
            storyNodes = wrapper.nodes;
            Debug.Log("Story nodes loaded successfully.");
        }
        else
        {
            Debug.LogError("JSON file not found or could not be loaded.");
        }
    }

    void PerformActions(string[] actions)
    {
        foreach (var action in actions)
        {
            if (string.IsNullOrEmpty(action)) continue;

            string[] parts = action.Split(':');
            if (parts.Length != 2)
            {
                Debug.LogError("Invalid action format: " + action);
                continue;
            }

            string actionType = parts[0];
            string actionValue = parts[1];

            switch (actionType)
            {
                case "giveItem":
                    GiveItem(actionValue);
                    break;
                case "decreaseHealth":
                    DecreaseHealth(int.Parse(actionValue));
                    break;
                case "increaseScore":
                    IncreaseScore(int.Parse(actionValue));
                    break;
                case "startBattle":
                    StartBattle();
                    break;
                case "startDialogue":
                    StartDialogue(actionValue);
                    break;
                default:
                    Debug.LogError("Unknown action type: " + actionType);
                    break;
            }
        }
    }

    void GiveItem(string itemName)
    {
        // Oyuncuya item verme kodunu buraya ekleyin
        Debug.Log("Item verildi: " + itemName);
    }

    void DecreaseHealth(int amount)
    {
        // Oyuncunun sağlığını azaltma kodunu buraya ekleyin
        Debug.Log("Health decreased by: " + amount);
    }

    void IncreaseScore(int amount)
    {
        Debug.Log("Score increased by: " + amount);
    }

    void StartBattle()
    {
        fpscript = fightpoint.GetComponent<FightPoint>();
        if (fpscript != null)
        {
            fpscript.SetupFightPoint();
        }
        else
        {
            Debug.LogError("FightPoint component not found on the fightpoint GameObject.");
        }
    }

    void StartDialogue(string kaynak)
    {
        image_name=kaynak;
        Sprite newSprite = Resources.Load<Sprite>(image_name);
        if (newSprite != null)
        {
            // Image bileşenine yeni Sprite'ı ata
            char_image.sprite = newSprite;
        }
        else
        {
            Debug.LogError("Sprite not found!");
        }
        
    }
    
}
