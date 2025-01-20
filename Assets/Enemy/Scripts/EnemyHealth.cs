using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;  // Maximální zdraví nepøítele
    [SerializeField] private int currentHealth;                    // Aktuální zdraví nepøítele

    [Header("Animation")]
    [SerializeField] private Animator animator;   // Animátor pro nepøítele

    private bool isDead = false;                  // Kontrola, zda nepøítel již zemøel

    void Start()
    {
        // Nastavení aktuálního zdraví na maximum pøi spuštìní
        currentHealth = maxHealth;

        // Pokud není animátor ruènì pøiøazen, pokusí se jej najít
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(20);
        }
    }

    /// <summary>
    /// Snižuje zdraví nepøítele o zadanou hodnotu.
    /// </summary>
    /// <param name="damage">Množství poškození.</param>
    public void TakeDamage(int damage)
    {
        if (isDead) return; // Pokud už je mrtvý, ignoruj další poškození

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Logika pro smrt nepøítele.
    /// </summary>
    private void Die()
    {
        if (isDead) return; // Zabránìní opakovanému volání

        isDead = true;
        Debug.Log($"{gameObject.name} has died.");

        // Spustí animaci smrti nastavením DeathTrigger
        if (animator != null)
        {
            animator.SetTrigger("DeathTrigger");
        }

        // Zabrání dalším interakcím (napø. pohyb, útoky)
        DisableEnemyBehavior();
    }

    /// <summary>
    /// Zakáže chování nepøítele (pohyb, útok).
    /// </summary>
    private void DisableEnemyBehavior()
    {
        // Pokud má nepøítel NavMeshAgent, deaktivuj ho
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }

        // Pokud má jiné scripty, které je tøeba deaktivovat, pøidejte zde logiku
    }

    /// <summary>
    /// Obnova zdraví nepøítele na maximum.
    /// </summary>
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;

        if (animator != null)
        {
            animator.ResetTrigger("DeathTrigger");
        }

        Debug.Log($"{gameObject.name} health has been reset to {maxHealth}.");
    }
}
