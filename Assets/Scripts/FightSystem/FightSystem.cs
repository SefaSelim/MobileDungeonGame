using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMY1TURN, ENEMY2TURN, ENEMY3TURN, WON, LOST }

public class FightSystem : MonoBehaviour
{
    int enemyIndex;
    public GameObject FightScreen;
    public BattleState state;
    public GameObject playerPrefab;
    public GameObject[] enemyPrefabs;
    public Transform[] enemyBattleStations;

    public TextMeshProUGUI enemyName1, enemyName2, enemyName3;
    public TextMeshProUGUI enemyLevel1, enemyLevel2, enemyLevel3;
    public TextMeshProUGUI enemyHP1, enemyHP2, enemyHP3;

    public TextMeshProUGUI enemyAC1, enemyAC2, enemyAC3;
    public TextMeshProUGUI playerHP, playerLevel, playerName, dialogueText, playerAC, playerCurrentEXP, playerMaxExp;

    public Button mainattackButton,attackButton1, attackButton2, attackButton3;
    public Button healButton;

    Unit playerUnit;
    List<Unit> enemyUnits = new List<Unit>();

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
        attackButton1.onClick.AddListener(() => OnSelectButton(attackButton1));
        attackButton2.onClick.AddListener(() => OnSelectButton(attackButton2));
        attackButton3.onClick.AddListener(() => OnSelectButton(attackButton3));
    }

    public IEnumerator SetupBattle()
    {
        FightScreen.SetActive(true);
        GameObject playerGO = Instantiate(playerPrefab);
        playerUnit = playerGO.GetComponent<Unit>();

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            GameObject enemyGO = Instantiate(enemyPrefabs[i], enemyBattleStations[i]);
            Unit enemyUnit = enemyGO.GetComponent<Unit>();
            enemyUnits.Add(enemyUnit);
        }

        UpdateUI();

        dialogueText.text = "Düşmanlarla karşılaştınız!";
        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void UpdateUI()
    {
        playerCurrentEXP.text = "EXP " + playerUnit.playerexp;
        playerMaxExp.text = "/ " + playerUnit.expToLevelUP;
        playerLevel.text = "Lvl " + playerUnit.unitLevel;
        playerHP.text = "HP " + playerUnit.currentHP;
        playerName.text = playerUnit.unitName;
        playerAC.text = "AC" + playerUnit.unitAC;

        for (int i = 0; i < enemyUnits.Count; i++)
        {
            TextMeshProUGUI enemyName = (TextMeshProUGUI)GetType().GetField("enemyName" + (i + 1)).GetValue(this);
            TextMeshProUGUI enemyLevel = (TextMeshProUGUI)GetType().GetField("enemyLevel" + (i + 1)).GetValue(this);
            TextMeshProUGUI enemyHP = (TextMeshProUGUI)GetType().GetField("enemyHP" + (i + 1)).GetValue(this);
      
            enemyName.text = enemyUnits[i].unitName;
            enemyLevel.text = "Lvl " + enemyUnits[i].unitLevel;
            enemyHP.text = "HP " + enemyUnits[i].currentHP;
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Sıra sizde. Kime saldıracaksınız?";
        attackButton1.interactable = enemyUnits.Count > 0;
        attackButton2.interactable = enemyUnits.Count > 1;
        attackButton3.interactable = enemyUnits.Count > 2;
        healButton.interactable = true;
    }

    public void OnSelectButton(Button SelectedEnemyButton){
        
        SelectedEnemyButton.interactable=false; 
            if (SelectedEnemyButton != attackButton1)
        {
            attackButton1.interactable = true;
            enemyIndex = 0;
        }

        if (SelectedEnemyButton != attackButton2)
        {
            attackButton2.interactable = true;
            enemyIndex = 1;
        }

        if (SelectedEnemyButton != attackButton3)
        {
            attackButton3.interactable = true;
            enemyIndex = 2;
        }

    }
    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        StartCoroutine(PlayerAttack(enemyIndex));
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        StartCoroutine(PlayerHeal());
    }

    IEnumerator PlayerAttack(int enemyIndex)
    {
        attackButton1.interactable = false;
        attackButton2.interactable = false;
        attackButton3.interactable = false;
        healButton.interactable = false;

        Unit targetEnemy = enemyUnits[enemyIndex];

        int attackRoll = Random.Range(1, 21);
        attackRoll += playerUnit.unitAttackRoll;
        if (attackRoll - playerUnit.unitAttackRoll == 20)
        {
            yield return StartCoroutine(ShowMessage("KRİTİK VURUŞ!!!", 2f));
        }
        else
        {
            yield return StartCoroutine(ShowMessage("Saldırı zarınız " + attackRoll, 2f));
        }

        if (attackRoll >= targetEnemy.unitAC)
        {
            int damage = Random.Range(1, playerUnit.damage + 1);
            damage += playerUnit.weapondamage;
            if (attackRoll - playerUnit.unitAttackRoll == 20)
            {
                damage *= 2;
            }
            bool isDead = targetEnemy.TakeDamage(damage);
            UpdateUI();

            yield return StartCoroutine(ShowMessage(targetEnemy.unitName + "'e " + damage + " zarar verdiniz!", 2f));

            if (isDead)
            {
                enemyUnits.RemoveAt(enemyIndex);
                if (enemyUnits.Count == 0)
                {
                    state = BattleState.WON;
                    StartCoroutine(EndBattle());
                }
                else
                {
                    StartCoroutine(NextTurn());
                }
            }
            else
            {
                StartCoroutine(NextTurn());
            }
        }
        else
        {
            yield return StartCoroutine(ShowMessage("Iskaladınız!", 2f));
            StartCoroutine(NextTurn());
        }
    }

    IEnumerator PlayerHeal()
    {
        attackButton1.interactable = false;
        attackButton2.interactable = false;
        attackButton3.interactable = false;
        healButton.interactable = false;

        int healAmount = Random.Range(1, 6); // 1 to 5
        playerUnit.Heal(healAmount);
        UpdateUI();

        yield return StartCoroutine(ShowMessage(healAmount + " HP iyileştiniz!", 2f));

        StartCoroutine(NextTurn());
    }

    IEnumerator NextTurn()
    {
        if (state == BattleState.PLAYERTURN)
        {
            state = BattleState.ENEMY1TURN;
        }
        else if (state == BattleState.ENEMY1TURN)
        {
            state = enemyUnits.Count > 1 ? BattleState.ENEMY2TURN : BattleState.PLAYERTURN;
        }
        else if (state == BattleState.ENEMY2TURN)
        {
            state = enemyUnits.Count > 2 ? BattleState.ENEMY3TURN : BattleState.PLAYERTURN;
        }
        else if (state == BattleState.ENEMY3TURN)
        {
            state = BattleState.PLAYERTURN;
        }

        if (state == BattleState.PLAYERTURN)
        {
            PlayerTurn();
        }
        else
        {
            StartCoroutine(EnemyTurn());
        }

        yield return null;
    }

    IEnumerator EnemyTurn()
    {
        int enemyIndex = 0;
        if (state == BattleState.ENEMY2TURN) enemyIndex = 1;
        else if (state == BattleState.ENEMY3TURN) enemyIndex = 2;

        Unit currentEnemy = enemyUnits[enemyIndex];

        dialogueText.text = currentEnemy.unitName + " saldırıyor!";
        yield return new WaitForSeconds(1f);

        int attackRoll = Random.Range(1, 21);
        attackRoll += currentEnemy.unitAttackRoll;
        if (attackRoll - currentEnemy.unitAttackRoll == 20)
        {
            yield return StartCoroutine(ShowMessage("RAKİP KRİTİK VURDU!!! ", 2f));
        }
        else
        {
            yield return StartCoroutine(ShowMessage("Rakibin saldırı zarı " + attackRoll, 2f));
        }

        if (attackRoll >= playerUnit.unitAC)
        {
            int damage = Random.Range(1, currentEnemy.damage + 1);
            damage += currentEnemy.weapondamage;
            if (attackRoll - currentEnemy.unitAttackRoll == 20)
            {
                damage = damage * 2;
            }
            bool isDead = playerUnit.TakeDamage(damage);
            UpdateUI();

            yield return StartCoroutine(ShowMessage(currentEnemy.unitName + " size " + damage + " zarar verdi!", 2f));

            if (isDead)
            {
                state = BattleState.LOST;
                StartCoroutine(EndBattle());
            }
            else
            {
                StartCoroutine(NextTurn());
            }
        }
        else
        {
            yield return StartCoroutine(ShowMessage("Rakip ıskaladı!", 2f));
            StartCoroutine(NextTurn());
        }
    }

    IEnumerator ShowMessage(string message, float delay)
    {
        dialogueText.text = message;
        yield return new WaitForSeconds(delay);
    }

    IEnumerator EndBattle()
    {
        attackButton1.interactable = false;
        attackButton2.interactable = false;
        attackButton3.interactable = false;
        healButton.interactable = false;

        if (state == BattleState.WON)
        {
            int totalExp = 0;
            foreach (Unit enemy in enemyUnits)
            {
                totalExp += enemy.exptobegiven;
            }

            playerUnit.TakeExp(totalExp);
            UpdateUI();

            yield return StartCoroutine(ShowMessage("TEBRİKLER! Tüm düşmanları yenerek " + totalExp + " EXP KAZANDINIZ.", 2f));


        }
        else if (state == BattleState.LOST)
        {
            yield return StartCoroutine(ShowMessage("Kaybettiniz!", 2f));
        }

    }

    

}
