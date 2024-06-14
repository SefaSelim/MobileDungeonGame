using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryManagementScript : MonoBehaviour
{
    [System.Serializable]
    public class StoryNode
    {
        [TextArea(3, 10)]
        public string storyText;
        public string[] options;
        public int[] nextNodes;
    }

    [System.Serializable]
    public class StoryNodeWrapper
    {
        public StoryNode[] nodes;
    }

    public TextMeshProUGUI storyTextUI;
    public Button[] optionButtons;
    public StoryNode[] storyNodes;

    private int currentNode;
    private bool canSelectOption = true;

    void Start()
    {
        LoadStoryNodesFromJSON();
        currentNode = 0;
        UpdateStory();
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
        
        currentNode = storyNodes[currentNode].nextNodes[optionIndex];
        UpdateStory();
    }

    void UpdateStory()
    {
        canSelectOption = false; // Seçenekleri devre dışı bırak

        StoryNode node = storyNodes[currentNode];
        storyTextUI.text = node.storyText;

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

        StartCoroutine(EnableOptionsAfterDelay(1f)); // Seçenekleri 1 saniye sonra yeniden etkinleştir
    }

    IEnumerator EnableOptionsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canSelectOption = true; // Seçenekleri tekrar etkinleştir
    }

    void LoadStoryNodesFromJSON()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("storyNodes");
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
}
