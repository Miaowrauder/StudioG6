using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent navigation;
    private Animator animator;
    private PlayerCombat player;
    private GameManager gameManager;
    public int health = 2;
    public int strength = 1;
    public int meleeRange = 3;
    public int spawnCost = 10;
    
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
        navigation.destination = player.transform.position;

        if (Vector3.Distance(player.transform.position, transform.position) < meleeRange && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Attacking"))
        {
            animator.SetTrigger("Attack_Melee");
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
}
