using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightPoint : MonoBehaviour
{
    
    public GameObject BattleSystem;
    public GameObject Enemy;
    FightSystem fightSystem;
    private void OnCollisionEnter2D(Collision2D collision) {
    if (collision.collider.CompareTag("CollisionDebugger"))
    {
          SetupFight();
    }
}
    // Update is called once per frame
    void SetupFight()
    {
          fightSystem = BattleSystem.GetComponent<FightSystem>();
          fightSystem.enemyPrefab  = Enemy;

    }
}
