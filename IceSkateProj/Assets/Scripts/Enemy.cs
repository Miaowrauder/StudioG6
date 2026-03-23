using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Logic Settings")]
    private NavMeshAgent navigation;
    private Animator animator;
    private PlayerCombat player;
    private GameManager gameManager;
    public bool canMove = true;
    public Projectile projectile;
    [Header("Customise Settings")]
    public int spawnCost;
    public int waveEnabled;
    public int health;
    public bool isRanged;
    public int strength;
    public int meleeRange;
    public int rangedRange;
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        navigation = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerCombat>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check that an attack animation is playing, if so, stop moving
        bool isFree = animator.GetCurrentAnimatorStateInfo(0).IsTag("Free");
        if (!isFree)
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }

        if (canMove)
        {
            navigation.destination = player.transform.position;
        }
        else
        {
            navigation.destination = transform.position;
        }


        if (!isRanged)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < meleeRange && isFree)
            {
                animator.SetTrigger("Attack_Melee");
            }
        }
        else
        {
            if (Vector3.Distance(player.transform.position, transform.position) < rangedRange && isFree)
            {
                animator.SetTrigger("Attack_Ranged");
            }

            if (Vector3.Distance(transform.position, player.transform.position) <= rangedRange)
            {
                canMove = false;
            }
            else
            {
                canMove = true;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            gameManager.GetComponent<GameManager>().currentEnemies--;
            Destroy(gameObject);
        }
    }

    private void Attack()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < meleeRange)
        {
            player.TakeDamage(strength);
        }
    }

    public void ProjectileThrow()
    {
        Projectile spawnedProjectile = Instantiate(projectile, transform.position, transform.rotation);
    }
}
