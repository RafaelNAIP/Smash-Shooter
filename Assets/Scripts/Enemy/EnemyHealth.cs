using UnityEngine;

// LEGADO: substituído por HealthSystem (Combat/). Troque o componente nos prefabs de inimigo
// e depois delete este script.
public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    void Start(){
        currentHealth = maxHealth;
    }
    public void TakeDamage(float amount){
        currentHealth -= amount;
        if (currentHealth <= 0f)  {
            Die();
        }
    }
    void Die(){
        gameObject.SetActive(false);
        
    }
}

