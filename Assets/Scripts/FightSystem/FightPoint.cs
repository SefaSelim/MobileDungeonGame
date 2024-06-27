using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightPoint : MonoBehaviour
{
    
    public GameObject BattleSystem;
    public GameObject Enemy1;
     public GameObject Enemy2;
      public GameObject Enemy3;
    FightSystem fightSystem;
    
    private void OnCollisionEnter2D(Collision2D collision) {
    if (collision.collider.CompareTag("CollisionDebugger"))
    {
          SetupFightPoint();
    }
}
    // Update is called once per frame
    public void SetupFightPoint()
    {
          fightSystem = BattleSystem.GetComponent<FightSystem>();
          fightSystem.enemyPrefabs[0]  = Enemy1;
           fightSystem.enemyPrefabs[1]  = Enemy2;
            fightSystem.enemyPrefabs[2]  = Enemy3;
          StartCoroutine(fightSystem.SetupBattle());
          Debug.Log("SAVAŞ BAŞLADI");

    }
}
