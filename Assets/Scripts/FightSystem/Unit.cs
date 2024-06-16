using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;
    public int damage;
    public int maxHP;
    public int currentHP;

    /// <summary>
    /// Takes damage and returns true if the unit's health drops to zero or below.
    /// </summary>
    /// <param name="dmg">Amount of damage to take</param>
    /// <returns>True if the unit's health drops to zero or below, false otherwise</returns>
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
}
