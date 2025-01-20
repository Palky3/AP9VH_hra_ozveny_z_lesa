using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] int health = 100;
    [SerializeField] int currentHealth;
    [SerializeField] GameObject hitVFX;
    //[SerializeField] GameObject ragdoll;

    public HealthBar healthBar;

    private Character character;

    Animator animator;
    void Start()
    {
        currentHealth = health;
        healthBar.SetMaxHealth(health);
        animator = GetComponent<Animator>();
        character = GetComponent<Character>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmount;
            healthBar.SetHealth(currentHealth);
            //health -= damageAmount;
            animator.SetTrigger("damage");
            //CameraShake.Instance.ShakeCamera(2f, 0.2f);
        }

        if (currentHealth <= 0 && !character.isDead)
        {
            Die();
        }
    }

    void Die()
    {
        character.isDead = true;
        animator.SetTrigger("death");

        //Instantiate(ragdoll, transform.position, transform.rotation);
        //Destroy(this.gameObject);
    }
    /*public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 3f);

    }*/
}
