using System.Collections;
using UnityEngine;
using TMPro; // Nutné pro práci s TextMeshPro

public class HealthSystem : MonoBehaviour
{
    [SerializeField] int health = 100;
    [SerializeField] int currentHealth;
    [SerializeField] GameObject hitVFX;

    [Header("Audio")]
    [SerializeField] AudioClip[] hitSounds;
    [SerializeField] AudioClip[] takeHitSounds;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip weaponSound;

    public HealthBar healthBar;
    public TextMeshProUGUI respawnText; // Odkaz na UI Text pro odpoèet
    public Transform spawnPoint; // Místo, kde se postava respawne

    Character character;
    Animator animator;
    AudioSource audioSource;

    void Start()
    {
        currentHealth = health;
        healthBar.SetMaxHealth(health);
        animator = GetComponent<Animator>();
        character = GetComponent<Character>();
        audioSource = GetComponent<AudioSource>();

        respawnText.enabled = false; // Skryjeme text pøi startu
        respawnText.gameObject.SetActive(false);
    }

    public void TakeDamage(int damageAmount)
    {
        if (takeHitSounds.Length > 0 && audioSource != null)
        {
            AudioClip clip = takeHitSounds[Random.Range(0, takeHitSounds.Length)];
            audioSource.PlayOneShot(clip, 0.5f);
        }

        if (hitSounds.Length > 0 && audioSource != null)
        {
            AudioClip clip = hitSounds[Random.Range(0, hitSounds.Length)];
            audioSource.PlayOneShot(clip, 0.5f);
        }

        if (currentHealth > 0)
        {
            currentHealth -= damageAmount;
            healthBar.SetHealth(currentHealth);
            animator.SetTrigger("damage");
        }

        if (currentHealth <= 0 && !character.isDead)
        {
            Die();
        }
    }

    void Die()
    {
        if (deathSound != null)
            audioSource.PlayOneShot(deathSound, 0.5f);

        character.isDead = true;
        animator.SetTrigger("death");
        StartCoroutine(RespawnCountdown()); // Spustíme odpoèet
    }

    IEnumerator RespawnCountdown()
    {
        respawnText.gameObject.SetActive(true);
        respawnText.enabled = true; // Zobrazíme text
        int countdown = 5;

        while (countdown > 0)
        {
            respawnText.text = $"Oživení za {countdown}";
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        respawnText.gameObject.SetActive(false);
        respawnText.enabled = false; // Skryjeme text
        Spawn(); // Zavoláme respawn
    }

    void Spawn()
    {
        transform.position = spawnPoint.position; // Pøesun na spawn point
        currentHealth = health;
        healthBar.SetHealth(currentHealth);
        character.isDead = false;
        animator.SetBool("death", false);
        animator.SetTrigger("respawn");
    }

    public void PlaySwordSound()
    {
        audioSource.PlayOneShot(weaponSound, 0.5f);
    }
}

