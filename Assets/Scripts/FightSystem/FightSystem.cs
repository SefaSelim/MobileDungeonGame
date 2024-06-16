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
   
    public TextMeshProUGUI enemyName, enemyLevel, enemyHP, playerHP, playerLevel, playerName;
    public TextMeshProUGUI dialogueText;

    public Button attackButton;
    public Button healButton;

    Unit playerUnit;
    Unit enemyUnit;

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
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

        playerLevel.text = "Lvl " + playerUnit.unitLevel;
        playerHP.text = "HP " + playerUnit.currentHP;
        playerName.text = playerUnit.unitName;

        enemyName.text = enemyUnit.unitName;
        enemyLevel.text = "Lvl " + enemyUnit.unitLevel;
        enemyHP.text = "HP " + enemyUnit.currentHP;

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

        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        enemyHP.text = "HP " + Mathf.Max(0, enemyUnit.currentHP);

        yield return StartCoroutine(ShowMessage("Saldırdınız ve " + playerUnit.damage + " zarar verdiniz!", 2f));

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerHeal()
    {
        attackButton.interactable = false;
        healButton.interactable = false;

        playerUnit.Heal(5);
        playerHP.text = "HP " + playerUnit.currentHP;

        yield return StartCoroutine(ShowMessage("5 HP iyileştiniz!", 2f));

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = "Rakip saldırıyor!";
        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHP.text = "HP " + Mathf.Max(0, playerUnit.currentHP);

        yield return StartCoroutine(ShowMessage(enemyUnit.unitName + " size " + enemyUnit.damage + " zarar verdi!", 2f));

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator ShowMessage(string message, float delay)
    {
        dialogueText.text = message;
        yield return new WaitForSeconds(delay);
    }

    void EndBattle()
    {
        attackButton.interactable = false;
        healButton.interactable = false;

        if (state == BattleState.WON)
        {
            dialogueText.text = "Kazandınız!";
            Debug.Log("YOU WON");
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "Kaybettiniz!";
            Debug.Log("YOU LOST");
        }
    }
}
