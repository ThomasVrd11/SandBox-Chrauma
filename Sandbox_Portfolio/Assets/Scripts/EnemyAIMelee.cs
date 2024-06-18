using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.Pool;


public class Enemy : MonoBehaviour
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
    bool alreadyAttacked;

    // * Detection ranges
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
	EnemyAnimator enemyAnimator;
	PlayerStats playerStats;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
		enemyAnimator = GetComponentInChildren<EnemyAnimator>();
		timeBetweenAttacks = 1.0f;

        // *set current hp when spawn
        currentHealth = startingHealth;


    }

    GameObject _LifeDropTarget;

	private void Start()
	{
		playerStats = player.GetComponent<PlayerStats>();
        _LifeDropTarget = GameObject.FindGameObjectWithTag("LifeDropTarget");
	}

    private void Update()
    {
        
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        
        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange && !alreadyAttacked) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        // * assez proche ??
        if (distanceToWalkPoint.magnitude < 1)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
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
            // * attack ici
            // Debug.Log("Melee attack!");
            enemyAnimator.startAttackAnimation();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
		// if (playerInAttackRange)
		// {
		// 	playerStats.TakeDamage(10);
		// }
        enemyAnimator.stopAttackAnimation();
        alreadyAttacked = false;
    }

    // public void TakeDamage(int damage)
    // {
    //     health -= damage;
    //     if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    // }
    private IObjectPool<Enemy> enemyPool;


    public void SetPool(IObjectPool<Enemy> pool)
    {
        enemyPool = pool;
    }

    private void EnemyDies()
    {
        // * ici condition de mort de l'ennemi

        //* il va drop un item (topaz)
        enemyPool.Release(this);

        // * il va drop la LifeDrop
        for (int i = 0; i > startingHealth/10; i++)
        {
            var go = Instantiate(lifeDropPrefab, transform.position + new Vector3(0, Random.Range(0, 2)), Quaternion.identity);
            go.GetComponent<FollowLifeDrop>().Target = _LifeDropTarget.transform;
        }
    }

    private void OnDrawGizmosSelected_()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
