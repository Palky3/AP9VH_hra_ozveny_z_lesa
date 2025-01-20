using StarterAssets;
using System.Collections;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public GameObject player; // Reference na hráèùv controller
    private ThirdPersonController playerController;
    private bool attackProcessed = false;

    private void Start()
    {
        playerController = player.GetComponent<ThirdPersonController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: " + other);
        if (playerController.IsAttacking && !attackProcessed && other.CompareTag("Enemy"))
        {
            attackProcessed = true;

            // Najdeme komponentu nepøítele a zavoláme jeho funkci
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(20);
            }
        }
    }

    private void Update()
    {
        if (!playerController.IsAttacking)
        {
            attackProcessed = false;
        }
        
    }

}
