using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 3;
    //[SerializeField] GameObject hitVFX;
    //[SerializeField] GameObject ragdoll;

    [Header("Combat")]
    [SerializeField] float attackCD = 3f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float aggroRange = 4f;
    [SerializeField] GameObject spawnNPC;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject enemyDealer;
    [SerializeField] GameObject bossPelvisCapsuleCollider;

    [Header("Patroling")]
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] float patrolWaitTime = 2f;

    [Header("Audio")]
    [SerializeField] AudioClip[] hitSounds;
    [SerializeField] AudioClip[] takeHitSounds;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip weaponSound;

    AudioSource audioSource;
    GameObject player;
    NavMeshAgent agent;
    Animator animator;
    Character playerCharacter;
    CapsuleCollider capsuleCollider;
    float timePassed;
    float newDestinationCD = 0.5f;
    bool isDead = false;
    //bool isPatrolling = true; // Sleduje, zda se nepøítel patroluje
    int currentPatrolIndex = 0; // Aktuální bod patrolování
    float waitTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerCharacter = player.GetComponent<Character>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();

        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCharacter.isDead || isDead)
        {
            return;
        }

        animator.SetFloat("speed", agent.velocity.magnitude / agent.speed);

        if (Vector3.Distance(player.transform.position, transform.position) <= aggroRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (waitTime <= 0)
            {
                // Posuò na další bod patrolování
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);
                waitTime = patrolWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    void ChasePlayer()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
        {
            if (timePassed >= attackCD)
            {
                animator.SetTrigger("attack");
                timePassed = 0;
            }
        }
        else
        {
            agent.SetDestination(player.transform.position);
        }

        timePassed += Time.deltaTime;
        transform.LookAt(player.transform);
    }

    void Die()
    {
        //Instantiate(ragdoll, transform.position,transform.rotation);
        //print(this.gameObject.tag);
        if (string.Equals(this.gameObject.tag, "Boss"))
        {
            spawnNPC.SetActive(true);
            gameManager.showVictoryText();
            gameManager.ChangeQuestText();

            CapsuleCollider[] colliders = GetComponentsInChildren<CapsuleCollider>();
            foreach (CapsuleCollider col in colliders)
            {
                col.enabled = false;
            }

        }
        //Destroy(this.gameObject);
        if (deathSound != null)
            audioSource.PlayOneShot(deathSound, 0.5f);
        
        animator.SetBool("death", true);
        capsuleCollider.enabled = false;
        agent.enabled = false;

        isDead = true;
        
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        animator.SetTrigger("damage");
        //CameraShake.Instance.ShakeCamera(2f, 0.2f);

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

        if (health <= 0)
        {
            Die();
        }
    }
   public void StartDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>().StartDealDamage();
        enemyDealer.GetComponent<EnemyDamageDealer>().StartDealDamage();

    }
    public void EndDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>().EndDealDamage();
        enemyDealer.GetComponent<EnemyDamageDealer>().EndDealDamage();
    }

    /*public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 3f);
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);

        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            Gizmos.color = Color.blue;
            foreach (var point in patrolPoints)
            {
                Gizmos.DrawSphere(point.position, 0.2f);
            }
        }

    }

    public void PlaySwordSound()
    {
        audioSource.PlayOneShot(weaponSound, 0.5f);
    }

}
