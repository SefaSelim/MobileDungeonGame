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
          SetupFightPoint();
    }
}
    // Update is called once per frame
    void SetupFightPoint()
    {
          fightSystem = BattleSystem.GetComponent<FightSystem>();
          fightSystem.enemyPrefab  = Enemy;
          StartCoroutine(fightSystem.SetupBattle());

    }
}
