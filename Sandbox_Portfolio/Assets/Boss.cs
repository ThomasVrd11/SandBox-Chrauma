using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.Pool;

using TMPro;

public class Boss : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float startingHealth;
    public float currentHealth;
    public GameObject lifeDropPrefab;

    // * Patrol settings
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // * Attack settings
    public float timeBetweenAttacks;
    public float attackCooldown = 8f;
    bool alreadyAttacked;

    // * Detection ranges
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    // * Animator
    private Animator animator;

    // * debug
    [SerializeField] TMP_Text hptext;
    [SerializeField] TMP_Text hptext2;
    public bool debugHP = false;
    [SerializeField] ParticleSystem bloodSplatter;
    [SerializeField] GameObject deathSmoke;
    private Vector3 currentPos;
    private Vector3 oldPos;
    private bool isWalking;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        // *set current hp when spawn
        currentHealth = startingHealth;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentPos = gameObject.transform.position;
        oldPos = currentPos;

    }

    private void Update()
    {
        currentPos = transform.position;
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange && !alreadyAttacked) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

        if (currentPos != oldPos)
        {
            isWalking = true;
            oldPos = currentPos;
        }
        else
        {
            if (isWalking) isWalking = false;
        }
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isIdle", !isWalking);
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1) walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        if (!alreadyAttacked)
        {
            int attackIndex = Random.Range(1, 4);
            animator.SetInteger("AttackIndex", attackIndex);
            animator.SetTrigger("Attack");

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (debugHP) Debug.Log(gameObject.name + " hp:" + currentHealth);
        bloodSplatter.Play();
        if (debugHP) DebugHP(damage);
        if (currentHealth <= 0) EnemyDies();
    }

    private void EnemyDies()
    {
        Instantiate(deathSmoke, transform.position, Quaternion.identity);
        for (int i = 0; i < startingHealth / 10; i++)
        {
            var go = Instantiate(lifeDropPrefab, transform.position + new Vector3(0, Random.Range(0, 2)), Quaternion.identity);
            var goscript = go.GetComponent<FollowLifeDrop>();
            goscript.Target = GameObject.FindGameObjectWithTag("LifeDropTarget").transform;
            goscript.StartFollowing();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    private void DebugHP(int damage)
    {
        hptext.text = "" + currentHealth;
        hptext2.text = "" + damage;
    }
}
