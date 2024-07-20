using UnityEngine;
using UnityEngine.UI;

public class HealthBardeneme : MonoBehaviour
{
    public Unit unitsc;
    public Image fillImage; // Dolgu image'ı
    public int maxhealth;

    public int currentHP; 
    void Start()
    {
        maxhealth = unitsc.maxHP;
        UpdateHealthBar();
    }
    void UpdateHealthBar()
    {
        
        
        float fillAmount = (float)(currentHP / maxhealth);
        //fillImage.fillAmount = Mathf.Clamp(fillAmount, 0, unitsc.maxHP); // Dolgu miktarını 0 ile 1 arasında sınırla
        
    }

    void Update()
    {
        currentHP = unitsc.currentHP;
        if (Input.GetKeyDown(KeyCode.Space)) // Test için boşluk tuşuna basınca hasar al
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.H)) // Test için H tuşuna basınca iyileş
        {
            Heal(10);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0)
            currentHP = 0;
        UpdateHealthBar();
    }

    public void Heal(int healAmount)
    {
        currentHP += healAmount;
        if (currentHP > unitsc.maxHP)
            currentHP = unitsc.maxHP;
        UpdateHealthBar();
    }
}

