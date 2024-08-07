using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class FightSystem : MonoBehaviour
{
    private bool isAttacking = false;
    private bool isAttackingEnemy = false;
    public GameObject FightScreen;
    public BattleState state;
    public GameObject playerPrefab;
    public GameObject playerGO;
    //public GameObject[] enemyPrefabs;
    public Transform[] enemyBattleStations;

    public TextMeshProUGUI[] enemyNames;
    public TextMeshProUGUI[] enemyLevels;
    public TextMeshProUGUI[] enemyHPs;
    public TextMeshProUGUI[] enemyACs;
    public Button[] attackButtons;

      public GameObject[] Fillers;

    public TextMeshProUGUI playerHP, playerLevel, playerName, dialogueText, playerAC, playerCurrentEXP, playerMaxExp;
    public Button mainAttackButton;
    public Button healButton;


    Unit playerUnit;
    List<Unit> enemyUnits = new List<Unit>();
    int selectedEnemyIndex = -1;
    int enemyCount;
    int totalExp = 0;

    void Start()
    {


    }

    public IEnumerator SetupBattle(GameObject[] enemyPrefabs)
    {
        state = BattleState.START;

        enemyCount = Mathf.Min(enemyPrefabs.Length, enemyBattleStations.Length, attackButtons.Length);
        Debug.Log($"Enemy count set to: {enemyCount}");
        Debug.Log("enemyPrefabs Length: " + enemyPrefabs.Length);
        Debug.Log("enemyBattleStations Length: " + enemyBattleStations.Length);
        Debug.Log("attackButtons Length: " + attackButtons.Length);


        for (int i = 0; i < attackButtons.Length; i++)
        {
            int index = i;
            attackButtons[i].onClick.AddListener(() => OnSelectButton(index));
            attackButtons[i].gameObject.SetActive(i < enemyCount);
        }

        mainAttackButton.onClick.AddListener(OnAttackButton);
        healButton.onClick.AddListener(OnHealButton);
        Debug.Log("Start method completed. Enemy count: " + enemyCount);
        FightScreen.SetActive(true);
        playerUnit = playerGO.GetComponent<Unit>();

        Debug.Log($"Player created: {playerUnit.unitName}");

        enemyUnits.Clear();
        totalExp = 0;
        Debug.Log($"Enemy count before creation: {enemyCount}");
        Debug.Log($"Enemy prefabs count: {enemyPrefabs.Length}");
        Debug.Log($"Enemy battle stations count: {enemyBattleStations.Length}");

        for (int i = 0; i < enemyCount; i++)
        {
            if (i < enemyBattleStations.Length)
            {
                GameObject enemyGO = Instantiate(enemyPrefabs[i], enemyBattleStations[i]);
                Unit enemyUnit = enemyGO.GetComponent<Unit>();
                if (enemyUnit != null)
                {
                    enemyUnits.Add(enemyUnit);
                    totalExp += enemyUnit.exptobegiven;
                    Debug.Log($"Enemy {i} created: {enemyUnit.unitName}");
                }
                else
                {
                    Debug.LogError($"Enemy {i} prefab does not have a Unit component!");
                }
            }
            else
            {
                Debug.LogWarning($"Not enough battle stations for enemy {i}");
                break;
            }
        }

        Debug.Log($"Enemies created. Enemy count: {enemyUnits.Count}");
        if (enemyCount == 1)
        {

            RectTransform rectTransform0 = attackButtons[0].GetComponent<RectTransform>();
            Vector2 Positionrect0 = new Vector2(400, 220);
            rectTransform0.anchoredPosition = Positionrect0;
        }

        if(enemyCount == 2)
        {

            RectTransform rectTransform0 = attackButtons[0].GetComponent<RectTransform>();
            Vector2 Positionrect0 = new Vector2(200, 220);
            rectTransform0.anchoredPosition = Positionrect0;

            RectTransform rectTransform1 = attackButtons[1].GetComponent<RectTransform>();
            Vector2 Positionrect1 = new Vector2(-50, 370);
            rectTransform1.anchoredPosition = Positionrect1;

        }
        if(enemyCount == 3)
        {
            RectTransform rectTransform0 = attackButtons[0].GetComponent<RectTransform>();
            Vector2 Positionrect0 = new Vector2(200, 400);
            rectTransform0.anchoredPosition = Positionrect0;

            RectTransform rectTransform1 = attackButtons[1].GetComponent<RectTransform>();
            Vector2 Positionrect1 = new Vector2(-50, 550);
            rectTransform1.anchoredPosition = Positionrect1;

             RectTransform rectTransform2 = attackButtons[2].GetComponent<RectTransform>();
            Vector2 Positionrect2 = new Vector2(0, 90);
            rectTransform2.anchoredPosition = Positionrect2;
        }
        UpdateUI();


        dialogueText.text = "Düşmanlarla karşılaştınız!";
        yield return new WaitForSeconds(2f);

        Debug.Log("SetupBattle completed. Moving to PlayerTurn.");
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void UpdateUI()
    {
        if (playerUnit == null)
        {
            Debug.LogError("Player unit is null!");
            return;
        }

        playerCurrentEXP.text = "EXP " + playerUnit.playerexp;
        playerMaxExp.text = "/ " + playerUnit.expToLevelUP;
        playerLevel.text = "Lvl " + playerUnit.unitLevel;
        playerHP.text = "Hp" + playerUnit.currentHP;
        playerName.text = playerUnit.unitName;
        playerAC.text = "Ac" + playerUnit.unitAC;

        for (int i = 0; i < enemyCount; i++)
        {
            if (i < enemyUnits.Count && enemyUnits[i] != null)
            {
                Unit enemy = enemyUnits[i];
                enemyNames[i].text = enemy.unitName;
                enemyLevels[i].text = "Lvl " + enemy.unitLevel;
                enemyHPs[i].text = "" + enemy.currentHP;
                enemyACs[i].text = "" + enemy.unitAC;
                attackButtons[i].gameObject.SetActive(true);
                Image fillImage = Fillers[i].GetComponent<Image>();
                fillImage.fillAmount =(float)((float)enemy.currentHP / (float)enemy.maxHP);
            }
            else
            {
                enemyNames[i].text = "";
                enemyLevels[i].text = "";
                enemyHPs[i].text = "";
                enemyACs[i].text = "";
                attackButtons[i].gameObject.SetActive(false);
            }
        }
    }
    void PlayerTurn()
    {
        dialogueText.text = "Sıra sizde. Kime saldıracaksınız?";
        for (int i = 0; i < enemyCount; i++)
        {
            if (i < enemyUnits.Count && enemyUnits[i] != null)
            {
                attackButtons[i].interactable = true;
            }
            else
            {
                attackButtons[i].interactable = false;
            }
        }
        healButton.interactable = true;
        mainAttackButton.interactable = false;
        Debug.Log("PlayerTurn started. Active enemy count: " + enemyUnits.Count(e => e != null));
        UpdateUI();
    }

    public void OnSelectButton(int index)
    {
        if (index < enemyUnits.Count && enemyUnits[index] != null)
        {
            selectedEnemyIndex = index;
            mainAttackButton.interactable = true;
            for (int i = 0; i < enemyCount; i++)
            {
                if (i < enemyUnits.Count && enemyUnits[i] != null)
                {
                    attackButtons[i].interactable = i != index;
                }
                else
                {
                    attackButtons[i].interactable = false;
                }
            }
        }
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN || selectedEnemyIndex == -1 || isAttacking)
        {
            return;
        }
        isAttacking = true;
        StartCoroutine(PlayerAttack());
    }
    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        StartCoroutine(PlayerHeal());
    }

    IEnumerator PlayerAttack()
    {
        Unit targetEnemy = enemyUnits[selectedEnemyIndex];

        for (int i = 0; i < enemyCount; i++)
        {
            attackButtons[i].interactable = false;
        }
        healButton.interactable = false;
        mainAttackButton.interactable = false;

        int attackRoll = Random.Range(1, 21) + playerUnit.unitAttackRoll;
        bool isCritical = attackRoll - playerUnit.unitAttackRoll == 20;

        if (isCritical)
        {
            yield return StartCoroutine(ShowMessage("KRİTİK VURUŞ!!!", 2f));
        }
        else
        {
            yield return StartCoroutine(ShowMessage("Saldırı zarınız " + attackRoll, 2f));
        }

        if (attackRoll >= targetEnemy.unitAC)
        {
            int damage = Random.Range(1, playerUnit.damage + 1) + playerUnit.weapondamage;
            if (isCritical)
            {
                damage *= 2;
            }
            Debug.Log($"Player attacking {targetEnemy.unitName}. Damage: {damage}, Enemy HP before: {targetEnemy.currentHP}");
            bool isDead = targetEnemy.TakeDamage(damage);
            Debug.Log($"Enemy HP after: {targetEnemy.currentHP}, isDead: {isDead}");
            UpdateUI();

            yield return StartCoroutine(ShowMessage(targetEnemy.unitName + "'e " + damage + " zarar verdiniz!", 2f));

            if (isDead)
            {
                enemyUnits[selectedEnemyIndex] = null;
                yield return StartCoroutine(ShowMessage(targetEnemy.unitName + " öldü!", 2f));

                bool allEnemiesDead = enemyUnits.All(e => e == null);
                if (allEnemiesDead)
                {
                    state = BattleState.WON;
                    StartCoroutine(EndBattle());
                }
                else
                {
                    StartCoroutine(EnemyTurn());
                }
            }
            else
            {
                StartCoroutine(EnemyTurn());
            }
        }
        else
        {
            yield return StartCoroutine(ShowMessage("Iskaladınız!", 2f));
            StartCoroutine(EnemyTurn());
        }

        isAttacking = false;
    }
    IEnumerator PlayerHeal()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            attackButtons[i].interactable = false;
        }
        healButton.interactable = false;

        int healAmount = Random.Range(1, 6); // 1 to 5
        playerUnit.Heal(healAmount);
        UpdateUI();

        yield return StartCoroutine(ShowMessage(healAmount + " HP iyileştiniz!", 2f));

        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        UpdateUI();
        isAttacking = false;
        state = BattleState.ENEMYTURN;

        for (int i = 0; i < enemyUnits.Count; i++)
        {
            if (enemyUnits[i] != null)
            {
                yield return StartCoroutine(EnemyAttack(enemyUnits[i]));

                if (state == BattleState.LOST)
                {
                    yield break; // Oyun kaybedildiyse döngüyü sonlandır
                }
            }
        }

        if (state != BattleState.LOST)
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator EnemyAttack(Unit enemy)
    {
        if (isAttackingEnemy)
        {
            yield break;
        }

        isAttackingEnemy = true;

        dialogueText.text = enemy.unitName + " saldırıyor!";
        yield return new WaitForSeconds(1f);

        int attackRoll = Random.Range(1, 21) + enemy.unitAttackRoll;
        bool isCritical = attackRoll - enemy.unitAttackRoll == 20;

        if (isCritical)
        {
            yield return StartCoroutine(ShowMessage("RAKİP KRİTİK VURDU!!! ", 2f));
        }
        else
        {
            yield return StartCoroutine(ShowMessage("Rakibin saldırı zarı " + attackRoll, 2f));
        }

        if (attackRoll >= playerUnit.unitAC)
        {
            int damage = Random.Range(1, enemy.damage + 1) + enemy.weapondamage;
            if (isCritical)
            {
                damage *= 2;
            }
            bool isDead = playerUnit.TakeDamage(damage);
            UpdateUI();

            yield return StartCoroutine(ShowMessage(enemy.unitName + " size " + damage + " zarar verdi!", 2f));

            if (isDead)
            {
                state = BattleState.LOST;
                StartCoroutine(EndBattle());
            }
        }
        else
        {
            yield return StartCoroutine(ShowMessage("Rakip ıskaladı!", 2f));
        }

        isAttackingEnemy = false;
    }
    IEnumerator ShowMessage(string message, float delay)
    {
        dialogueText.text = message;
        yield return new WaitForSeconds(delay);
    }

    IEnumerator EndBattle()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            attackButtons[i].interactable = false;
            attackButtons[i].gameObject.SetActive(false);
        }
        healButton.interactable = false;
        mainAttackButton.interactable = false;

        if (state == BattleState.WON)
        {
            if (playerUnit != null)
            {
                playerUnit.TakeExp(totalExp);
                UpdateUI();
            }

            yield return StartCoroutine(ShowMessage("TEBRİKLER! Tüm düşmanları yenerek " + totalExp + " EXP KAZANDINIZ.", 2f));
            FightScreen.SetActive(false);

        }
        else if (state == BattleState.LOST)
        {
            UpdateUI();
            yield return StartCoroutine(ShowMessage("Kaybettiniz!", 2f));
        }

    }
}