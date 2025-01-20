using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;  // Maxim�ln� zdrav� nep��tele
    [SerializeField] private int currentHealth;                    // Aktu�ln� zdrav� nep��tele

    [Header("Animation")]
    [SerializeField] private Animator animator;   // Anim�tor pro nep��tele

    private bool isDead = false;                  // Kontrola, zda nep��tel ji� zem�el

    void Start()
    {
        // Nastaven� aktu�ln�ho zdrav� na maximum p�i spu�t�n�
        currentHealth = maxHealth;

        // Pokud nen� anim�tor ru�n� p�i�azen, pokus� se jej naj�t
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
    /// Sni�uje zdrav� nep��tele o zadanou hodnotu.
    /// </summary>
    /// <param name="damage">Mno�stv� po�kozen�.</param>
    public void TakeDamage(int damage)
    {
        if (isDead) return; // Pokud u� je mrtv�, ignoruj dal�� po�kozen�

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Logika pro smrt nep��tele.
    /// </summary>
    private void Die()
    {
        if (isDead) return; // Zabr�n�n� opakovan�mu vol�n�

        isDead = true;
        Debug.Log($"{gameObject.name} has died.");

        // Spust� animaci smrti nastaven�m DeathTrigger
        if (animator != null)
        {
            animator.SetTrigger("DeathTrigger");
        }

        // Zabr�n� dal��m interakc�m (nap�. pohyb, �toky)
        DisableEnemyBehavior();
    }

    /// <summary>
    /// Zak�e chov�n� nep��tele (pohyb, �tok).
    /// </summary>
    private void DisableEnemyBehavior()
    {
        // Pokud m� nep��tel NavMeshAgent, deaktivuj ho
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }

        // Pokud m� jin� scripty, kter� je t�eba deaktivovat, p�idejte zde logiku
    }

    /// <summary>
    /// Obnova zdrav� nep��tele na maximum.
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
