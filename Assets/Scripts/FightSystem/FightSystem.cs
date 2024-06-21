using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class FightSystem : MonoBehaviour
{
    public BattleState state;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public Transform enemyBattleStation;

    public TextMeshProUGUI enemyName, enemyLevel, enemyHP, playerHP, playerLevel, playerName, dialogueText, enemyAC, playerAC, playerCurrentEXP, playerMaxExp;


    public Button attackButton;
    public Button healButton;

    Unit playerUnit;
    Unit enemyUnit;

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab);
        playerUnit = playerGO.GetComponent<Unit>();
        if (playerUnit == null)
        {
            Debug.LogError("Player Unit component not found!");
            yield break;
        }

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();
        playerCurrentEXP.text = "EXP " + playerUnit.playerexp;
        playerMaxExp.text = "/ " + playerUnit.expToLevelUP;
        playerLevel.text = "Lvl " + playerUnit.unitLevel;
        playerHP.text = "HP " + playerUnit.currentHP;
        playerName.text = playerUnit.unitName;
        playerAC.text = "AC" + playerUnit.unitAC;

        enemyName.text = enemyUnit.unitName;
        enemyLevel.text = "Lvl " + enemyUnit.unitLevel;
        enemyHP.text = "HP " + enemyUnit.currentHP;
        enemyAC.text = "AC" + enemyUnit.unitAC;

        dialogueText.text = enemyUnit.unitName + " ile karşılaştınız!";
        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        dialogueText.text = "Sıra sizde. Ne Yapacaksınız?";
        attackButton.interactable = true;
        healButton.interactable = true;
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
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
        attackButton.interactable = false;
        healButton.interactable = false;

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

        if (attackRoll >= enemyUnit.unitAC)
        {
            int damage = Random.Range(1, playerUnit.damage + 1);
            damage += playerUnit.weapondamage;
            if (attackRoll - playerUnit.unitAttackRoll == 20)
            {
                damage *= 2;
            }
            bool isDead = enemyUnit.TakeDamage(damage);
            enemyHP.text = "HP " + Mathf.Max(0, enemyUnit.currentHP);

            yield return StartCoroutine(ShowMessage("Saldırdınız ve " + damage + " zarar verdiniz!", 2f));

            if (isDead)
            {
                state = BattleState.WON;
                StartCoroutine(EndBattle());
            }
            else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        }
        else
        {
            yield return StartCoroutine(ShowMessage("Iskaladınız!", 2f));
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerHeal()
    {
        attackButton.interactable = false;
        healButton.interactable = false;

        int healAmount = Random.Range(1, 6); // 1 to 5
        playerUnit.Heal(healAmount);
        playerHP.text = "HP " + playerUnit.currentHP;

        yield return StartCoroutine(ShowMessage(healAmount + " HP iyileştiniz!", 2f));

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }



    IEnumerator EnemyTurn()
    {
        dialogueText.text = "Rakip saldırıyor!";
        yield return new WaitForSeconds(1f);

        int attackRoll = Random.Range(1, 21);
        attackRoll += enemyUnit.unitAttackRoll;
        if (attackRoll - enemyUnit.unitAttackRoll == 20)
        {
            yield return StartCoroutine(ShowMessage("RAKİP KRİTİK VURDU!!! ", 2f));
        }
        else
        {
            yield return StartCoroutine(ShowMessage("Rakibin saldırı zarı " + attackRoll, 2f));
        }
        if (attackRoll >= playerUnit.unitAC)
        {

            int damage = Random.Range(1, enemyUnit.damage + 1);
            damage += enemyUnit.weapondamage;
            if (attackRoll - enemyUnit.unitAttackRoll == 20)
            { damage = damage * 2; }
            bool isDead = playerUnit.TakeDamage(damage);
            playerHP.text = "HP " + Mathf.Max(0, playerUnit.currentHP);

            yield return StartCoroutine(ShowMessage(enemyUnit.unitName + " size " + damage + " zarar verdi!", 2f));

            if (isDead)
            {
                state = BattleState.LOST;
                StartCoroutine(EndBattle());
            }
            else
            {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
        }
        else
        {
            yield return StartCoroutine(ShowMessage("Rakip ıskaladı!", 2f));
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator ShowMessage(string message, float delay)
    {
        dialogueText.text = message;
        yield return new WaitForSeconds(delay);
    }

    IEnumerator EndBattle()
    {
        attackButton.interactable = false;
        healButton.interactable = false;

        if (state == BattleState.WON)
        {
            Debug.Log("YOU WON");
            playerUnit.TakeExp(enemyUnit.exptobegiven);
             playerCurrentEXP.text = "EXP " + playerUnit.playerexp;
            playerMaxExp.text = " / " + playerUnit.expToLevelUP;
            playerLevel.text = "Lvl " + playerUnit.unitLevel;
            playerUnit.unitAC++;
            playerAC.text = "AC " + playerUnit.unitAC;

            yield return StartCoroutine(ShowMessage("TEBRİKLER " + enemyUnit.unitName + "'I YENEREK " + enemyUnit.exptobegiven + " EXP KAZANDINIZ.", 2f));
        
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "Kaybettiniz!";
            Debug.Log("YOU LOST");
        }
    }
}
