
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent navigation;
    private Animator animator;
    private GameObject player;
    private GameManager gameManager;
    public int health = 2;
    public int strength = 1;
    public int meleeRange = 3;
    public int spawnCost = 10;

    private int castInt;
    public LayerMask layerMask;
    public float fallDelay, fallSpeed;
    private bool[] isHole = new bool[4];
    public Transform[] castPos;
    RaycastHit hit;
    private bool isFalling, canAttack;
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        navigation = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isFalling)
        {
           navigation.destination = player.transform.position; 
        }
        

        if(Vector3.Distance(player.transform.position, transform.position) < meleeRange && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Attacking"))
        {
            animator.SetTrigger("Attack_Melee");
        }

        DownCast();

        if(isFalling)
        {
            navigation.enabled = false;

            this.transform.position = new Vector3(transform.position.x, transform.position.y-fallSpeed, transform.position.z);
            
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
        if((Vector3.Distance(player.transform.position, transform.position) < meleeRange) && canAttack)
        {
            player.GetComponent<PlayerCombat>().TakeDamage(strength);
        }
    }

    void DownCast() 
    {
        castInt++; //does 1 corner per frame, cycling

        if(castInt >= 4)
        {
            castInt = 0;
        }

        if(Physics.Raycast(castPos[castInt].transform.position, Vector3.down, out hit, 999f, layerMask, QueryTriggerInteraction.Collide));
        {
                if(hit.collider.gameObject.tag == ("Hole"))
                {
                    isHole[castInt] = true;
                }
                else
                {   
                    isHole[castInt] = false;
                }
        }

        if(isHole[0] && isHole[1] && isHole[2] && isHole[3])
        {
            Invoke("Fall", fallDelay);
        }
        else
        {
            CancelInvoke("Fall");
            //CancelFall();
        }

    }

    void Fall()
    {
        isFalling = true;

        Invoke("Death", 2f);
    }

    private void Death()
    {
        TakeDamage(999);
    }
}
