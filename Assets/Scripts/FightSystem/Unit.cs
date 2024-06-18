using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int playerexp;
    public int expToLevelUP;
    public string unitName;     
    public int unitLevel;
    public int damage;
    public int maxHP;
    public int currentHP;
    public int unitAttackRoll; //20lik zara ekleniyor.
    public int unitAC;
    public int exptobegiven;

    public int weapondamage;


    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            currentHP = 0;
            return true; 
        }
        return false; 
    }

    public void Heal(int amount)
    {
        currentHP += amount;

        if (currentHP > maxHP)
        {
            currentHP = maxHP; 
        }
    }

    public void TakeExp(int expamount)
    {
        playerexp += expamount;
        if(playerexp >= expToLevelUP )
        {
            playerexp -= expToLevelUP;
            unitLevel++;
            expToLevelUP = Mathf.FloorToInt(expToLevelUP * 1.5f);
        }
    }   
}
