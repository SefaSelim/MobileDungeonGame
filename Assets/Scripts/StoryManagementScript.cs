using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public TextMeshProUGUI storyTextUI;
    public Button[] optionButtons;

    // Eğer bu değişken zaten tanımlıysa, tekrar tanımlamadan devam edebilirsiniz.
    // Aşağıdaki satırı kontrol edin ve eğer zaten varsa yeni bir tanım eklemeyin.
    public StoryNode[] storyNodes;

    private int currentNode;

    void Start()
    {
        currentNode = 0;
        UpdateStory();
    }

    public void OnOptionSelected(int optionIndex)
    {
        currentNode = storyNodes[currentNode].nextNodes[optionIndex];
        UpdateStory();
    }

    void UpdateStory()
    {
        StoryNode node = storyNodes[currentNode];
        storyTextUI.text = node.storyText;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < node.options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = node.options[i];
                int index = i; // Avoid closure problem
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void Awake()
    {
        // Örnek hikaye düğümlerini burada tanımlayın.
        storyNodes = new StoryNode[]
        {
            new StoryNode
            {
                storyText = "Bir gün ormanda yürürken uzaktan Turanı gördünüz",
                options = new string[] {"Turana yaklaş", "Geri dön"},
                nextNodes = new int[] {1, 2}
            },
            new StoryNode
            {
                storyText = "Ormanın derinliklerine ilerlediniz ve Turanı gördünüz.",
                options = new string[] {"Turana yaklaş", "Kaç"},
                nextNodes = new int[] {3, 4}
            },
            new StoryNode
            {
                storyText = "Geri döndünüz ve evinize gittiniz.",
                options = new string[] {},
                nextNodes = new int[] {}
            },
            new StoryNode
            {
                storyText = "Turan sizi fark etti ve size doğru koşmaya başladı",
                options = new string[] {"AMMMINAKEE", "Kaçanzi ol"},
                nextNodes = new int[] {5, 6}
            },
            new StoryNode
            {
                storyText = "Kaçtınız ve ormanın çıkışını buldunuz.",
                options = new string[] {},
                nextNodes = new int[] {}
            },
            new StoryNode
            {
                storyText = "AMINAKE vuruşu yaptınız ve Turan yere iki seksen uzandı",
                options = new string[] {},
                nextNodes = new int[] {}
            },
            new StoryNode
            {
                storyText = "Hızlıca kaçtınız ve Turan sizi yakalayamadı. GÜVENDESİNİZ",
                options = new string[] {},
                nextNodes = new int[] {}
            }
        };
    }

    // Mevcut fonksiyonlar burada olacaktır.
}
