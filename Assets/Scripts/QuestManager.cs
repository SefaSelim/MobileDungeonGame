using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestCondition
{
    public string conditionType; 
    public int requiredValue; 
}

[System.Serializable]
public class Quest
{
    public string questName;
    public bool isCompleted;
    public List<QuestCondition> conditions; 
}
public class QuestManager : MonoBehaviour
{
    public List<Quest> quests;
   public Unit unit;

    private void Awake()
    {
        //  başlangıç
        if (quests.Count == 0)
        {
            quests.Add(new Quest { questName = "Görev1", isCompleted = false, conditions = new List<QuestCondition> { new QuestCondition { conditionType = "Level", requiredValue = 3 } } });
            quests.Add(new Quest { questName = "Görev2", isCompleted = false, conditions = new List<QuestCondition> { new QuestCondition { conditionType = "Level", requiredValue = 5 } } });
        }
    }

    private void Update()
    {
        
        foreach (var quest in quests)
        {
            if (!quest.isCompleted)
            {
                CheckQuestCompletion(quest);
            }
        }
    }

    public void CheckQuestCompletion(Quest quest)
    {
        foreach (var condition in quest.conditions)
        {
            if (condition.conditionType == "Level" && unit.unitLevel >= condition.requiredValue)
            {
                quest.isCompleted = true;
                Debug.Log($"Görev '{quest.questName}' tamamlandı!");
                // görev tamamlandığında
            }
        }
    }
    public Quest GetQuestByName(string questName)
    {
        foreach (var quest in quests)
        {
            if (quest.questName == questName)
            {
                return quest;
            }
        }


        Debug.LogWarning($"Görev '{questName}' bulunamadı!");
        return null;
    }
    public bool IsQuestCompleted(Quest quest)
    {
        if (quest.isCompleted == true)
        {
            return true;
        }
        else { return false; }

    }



}
