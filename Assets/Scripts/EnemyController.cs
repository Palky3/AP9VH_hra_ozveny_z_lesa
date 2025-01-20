using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemyController : MonoBehaviour
{
    [Header("Patrolling and Chasing")]
    public Transform[] patrolPoints;   // Body pro patrolov�n�
    public Transform player;           // Odkaz na hr��e
    public float followRange = 10f;    // Vzd�lenost pro sledov�n� hr��e
    public float attackRange = 2f;     // Vzd�lenost pro �tok
    public float chassingSpeed = 1f;
    public float patrolSpeed = 0.5f;

    [Header("Health Settings")]
    public int maxHealth = 100;        // Maxim�ln� zdrav� nep��tele
    public int currentHealth;         // Aktu�ln� zdrav� nep��tele

    private NavMeshAgent agent;        // NavMeshAgent pro v�po�et cesty
    private ThirdPersonCharacter character; // T��da pro pohyb a animace
    private int currentPatrolIndex;    // Aktu�ln� patrolovac� bod
    private bool isFollowingPlayer;    // Sleduje nep��tel hr��e?

    //[Header("Animation")]
    private Animator animator;          // Anim�tor pro nep��tele

    private bool isDead = false;       // Stav, zda je nep��tel mrtv�

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();
        animator = GetComponent<Animator>();

        // Zabr�n� rotaci agenta - rotaci �e�� animace
        agent.updateRotation = false;

        // Nastaven� zdrav� na maximum
        currentHealth = maxHealth;

        // Nastaven� prvn�ho patrolovac�ho bodu
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[0].position);
        }
    }

    void Update()
    {
        if (isDead) return; // Pokud je mrtv�, nepokra�uje

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // P�epnut� do re�imu sledov�n� hr��e, pokud je hr�� v dosahu
        if (distanceToPlayer <= followRange)
        {
            isFollowingPlayer = true;
            agent.SetDestination(player.position);
            agent.speed = chassingSpeed;
        }
        else
        {
            isFollowingPlayer = false;

            // Pokra�uje v patrolov�n�
            Patrol();
        }

        // Zastaven� a �tok, pokud je hr�� v dosahu �toku
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

        // P�epne na dal�� patrolovac� bod, pokud dos�hl aktu�ln�ho
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
            // Pohyb sm�rem k c�li
            character.Move(velocity, false, false);
        }
        else
        {
            // Zastaven�
            character.Move(Vector3.zero, false, false);
        }
    }

    private void AttackPlayer()
    {
        // Nep��tel p�estane pohybovat a spust� �tok
        agent.SetDestination(transform.position);
        character.Move(Vector3.zero, false, false);

        //Debug.Log("Enemy attacks the player!");
        // Zde m��ete p�idat logiku �toku (nap�. sn�en� hr��ova zdrav�).
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Pokud u� je mrtv�, ignoruje dal�� z�sahy

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Spust� animaci zran�n�
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

        // Spust� animaci smrti
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

        // Vypne anim�tor (po animaci smrti)
        StartCoroutine(DisableAnimatorAfterDeath());

        // Vypne pohybov� komponenty
        if (character != null)
        {
            character.enabled = false;
        }

        // Nastav� rigidbody na kinematick� (pokud existuje)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    private IEnumerator DisableAnimatorAfterDeath()
    {
        // Po�kejte, dokud neskon�� animace smrti
        yield return new WaitForSeconds(4f);

        if (animator != null)
        {
            animator.enabled = false;
        }
    }


    void OnDrawGizmosSelected()
    {
        // Vizualizace dosahu sledov�n� a �toku
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
