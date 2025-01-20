using StarterAssets;
using System.Collections;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public GameObject player; // Reference na hr���v controller
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

            // Najdeme komponentu nep��tele a zavol�me jeho funkci
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
