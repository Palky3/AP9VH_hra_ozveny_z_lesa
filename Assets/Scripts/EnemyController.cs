using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemyController : MonoBehaviour
{
    [Header("Patrolling and Chasing")]
    public Transform[] patrolPoints;   // Body pro patrolování
    public Transform player;           // Odkaz na hráèe
    public float followRange = 10f;    // Vzdálenost pro sledování hráèe
    public float attackRange = 2f;     // Vzdálenost pro útok
    public float chassingSpeed = 1f;
    public float patrolSpeed = 0.5f;

    [Header("Health Settings")]
    public int maxHealth = 100;        // Maximální zdraví nepøítele
    public int currentHealth;         // Aktuální zdraví nepøítele

    private NavMeshAgent agent;        // NavMeshAgent pro výpoèet cesty
    private ThirdPersonCharacter character; // Tøída pro pohyb a animace
    private int currentPatrolIndex;    // Aktuální patrolovací bod
    private bool isFollowingPlayer;    // Sleduje nepøítel hráèe?

    //[Header("Animation")]
    private Animator animator;          // Animátor pro nepøítele

    private bool isDead = false;       // Stav, zda je nepøítel mrtvý

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();
        animator = GetComponent<Animator>();

        // Zabrání rotaci agenta - rotaci øeší animace
        agent.updateRotation = false;

        // Nastavení zdraví na maximum
        currentHealth = maxHealth;

        // Nastavení prvního patrolovacího bodu
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[0].position);
        }
    }

    void Update()
    {
        if (isDead) return; // Pokud je mrtvý, nepokraèuje

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Pøepnutí do režimu sledování hráèe, pokud je hráè v dosahu
        if (distanceToPlayer <= followRange)
        {
            isFollowingPlayer = true;
            agent.SetDestination(player.position);
            agent.speed = chassingSpeed;
        }
        else
        {
            isFollowingPlayer = false;

            // Pokraèuje v patrolování
            Patrol();
        }

        // Zastavení a útok, pokud je hráè v dosahu útoku
        if (isFollowingPlayer && distanceToPlayer <= attackRange)
        {
            agent.speed = patrolSpeed;
            AttackPlayer();
        }
        else
        {
            // Pohyb podle NavMeshAgent
            MoveCharacter(agent.desiredVelocity);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(20);
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        // Pøepne na další patrolovací bod, pokud dosáhl aktuálního
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    private void MoveCharacter(Vector3 velocity)
    {
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            // Pohyb smìrem k cíli
            character.Move(velocity, false, false);
        }
        else
        {
            // Zastavení
            character.Move(Vector3.zero, false, false);
        }
    }

    private void AttackPlayer()
    {
        // Nepøítel pøestane pohybovat a spustí útok
        agent.SetDestination(transform.position);
        character.Move(Vector3.zero, false, false);

        //Debug.Log("Enemy attacks the player!");
        // Zde mùžete pøidat logiku útoku (napø. snížení hráèova zdraví).
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Pokud už je mrtvý, ignoruje další zásahy

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Spustí animaci zranìní
            if (animator != null)
            {
                //animator.SetTrigger("HurtTrigger");
            }
        }
    }

    private void Die()
    {
        isDead = true;

        agent.SetDestination(transform.position);
        character.Move(Vector3.zero, false, false);

        Debug.Log($"{gameObject.name} has died.");

        // Spustí animaci smrti
        if (animator != null)
        {
            animator.SetTrigger("DeathTrigger");
        }

        // Vypne NavMeshAgent
        if (agent != null)
        {
            agent.enabled = false;
        }

        // Vypne collider
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Vypne animátor (po animaci smrti)
        StartCoroutine(DisableAnimatorAfterDeath());

        // Vypne pohybové komponenty
        if (character != null)
        {
            character.enabled = false;
        }

        // Nastaví rigidbody na kinematický (pokud existuje)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    private IEnumerator DisableAnimatorAfterDeath()
    {
        // Poèkejte, dokud neskonèí animace smrti
        yield return new WaitForSeconds(4f);

        if (animator != null)
        {
            animator.enabled = false;
        }
    }


    void OnDrawGizmosSelected()
    {
        // Vizualizace dosahu sledování a útoku
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
