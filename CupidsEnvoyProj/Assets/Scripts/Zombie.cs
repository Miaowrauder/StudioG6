using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private NavMeshAgent navigation;
    private Animator animator;
    private Player player;
    public int health = 2;
    public int strength = 1;
    public int meleeRange = 3;
    
    void Start()
    {
        navigation = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
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
