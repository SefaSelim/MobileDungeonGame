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
    //public GameObject[] enemyPrefabs;
    public Transform[] enemyBattleStations;

    public TextMeshProUGUI[] enemyNames;
    public TextMeshProUGUI[] enemyLevels;
    public TextMeshProUGUI[] enemyHPs;
    public TextMeshProUGUI[] enemyACs;
    public Button[] attackButtons;

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
        GameObject playerGO = Instantiate(playerPrefab);
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
        playerHP.text = "HP " + playerUnit.currentHP;
        playerName.text = playerUnit.unitName;
        playerAC.text = "AC" + playerUnit.unitAC;

        for (int i = 0; i < enemyCount; i++)
        {
            if (i < enemyUnits.Count && enemyUnits[i] != null)
            {
                Unit enemy = enemyUnits[i];
                enemyNames[i].text = enemy.unitName;
                enemyLevels[i].text = "Lvl " + enemy.unitLevel;
                enemyHPs[i].text = "HP " + enemy.currentHP;
                enemyACs[i].text = "AC " + enemy.unitAC;
                attackButtons[i].gameObject.SetActive(true);
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

    void UpdateEnemyInfo(int index)
    {
        if (index < enemyUnits.Count && enemyUnits[index] != null)
        {
            Unit enemy = enemyUnits[index];
            enemyNames[index].text = enemy.unitName;
            enemyLevels[index].text = "Lvl " + enemy.unitLevel;
            enemyHPs[index].text = "HP " + enemy.currentHP;
            enemyACs[index].text = "AC " + enemy.unitAC;
            attackButtons[index].gameObject.SetActive(true);
        }
        else
        {
            enemyNames[index].text = "";
            enemyLevels[index].text = "";
            enemyHPs[index].text = "";
            enemyACs[index].text = "";
            attackButtons[index].gameObject.SetActive(false);
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
        }
        else if (state == BattleState.LOST)
        {
            UpdateUI();
            yield return StartCoroutine(ShowMessage("Kaybettiniz!", 2f));
        }

    }
}