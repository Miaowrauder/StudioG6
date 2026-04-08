using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Logic Settings")]
    private NavMeshAgent navigation;
    private Animator animator;
    private PlayerCombat player;
    private GameManager gameManager;
    private Rigidbody rigidbody;
    public Projectile projectile;
    [Header("Customise Settings")]
    public int spawnCost;
    public int waveEnabled;
    public int health;
    public bool damageImmune;
    public bool isRanged;
    public int strength;
    public int meleeRange;
    public int rangedRange;
    public Transform shootPos;
    [Header("Falling Settings")]
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
        player = FindObjectOfType<PlayerCombat>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Determines if the enemy should be able to move
        bool isFree = animator.GetCurrentAnimatorStateInfo(0).IsTag("Free");
        if (!isFree || isFalling)
        {
            navigation.enabled = false;
        }
        else
        {
            navigation.enabled = true;
            navigation.destination = player.transform.position;
        }

        // Checks whether the enemy is ranged or melee
        if (!isRanged)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < meleeRange && isFree)
            {
                animator.SetBool("Melee Queued", true);
                animator.SetTrigger("Stop Running");
            }
        }
        else
        {
            if (Vector3.Distance(player.transform.position, transform.position) < rangedRange && isFree)
            {
                animator.SetBool("Ranged Queued", true);
                animator.SetTrigger("Stop Running");
            }

            if (Vector3.Distance(transform.position, player.transform.position) <= rangedRange)
            {
                navigation.enabled = false;
            }
            else
            {
                navigation.enabled = true;
            }
        }

        //Determines direction facing for animations
        Vector2 movementVector = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.z - transform.position.z);
        
        if (Mathf.Abs(movementVector.x) < Mathf.Abs(movementVector.y))
        {
            if (movementVector.y > 0)
            {
                // North-facing 
                animator.SetInteger("Direction", 0);
            }
            else
            {
                // South-facing
                animator.SetInteger("Direction", 2);
            }
        }
        else
        {
            if (movementVector.x > 0)
            {
                // East-facing
                animator.SetInteger("Direction", 1);
            }
            else
            {
                // West-facing
                animator.SetInteger("Direction", 3);
            }
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
        if (!damageImmune)
        {
            health -= damage;
        }

        if (health <= 0)
        {
            gameManager.GetComponent<GameManager>().points += spawnCost;
            gameManager.GetComponent<GameManager>().currentEnemies--;
            Destroy(gameObject);
        }
    }

    // Attacks the player melee
    public void Attack()
    {
        animator.SetBool("Melee Queued", false);

        if (Vector3.Distance(player.transform.position, transform.position) < meleeRange)
        {
            player.TakeDamage(strength);
        }
    }
    
    // Attacks the player ranged
    public void ProjectileThrow()
    {
        animator.SetBool("Ranged Queued", false);
        Projectile spawnedProjectile = Instantiate(projectile, shootPos.transform.position, transform.rotation);
    }

    public void StartWobble()
    {
        animator.SetBool("Wobble Queued", true);
        animator.SetBool("Melee Queued", false);
        animator.SetBool("Ranged Queued", false);

        animator.SetTrigger("Stop Running");
    }

    public void EndWobble()
    {
        animator.SetBool("Wobble Queued", false);
    }

    // Prevents the ai trying to move while push physics applied
    private void ChangeMovementMethod()
    {
        rigidbody.velocity = Vector3.zero;
        navigation.enabled = !navigation.enabled;
    }

    // Detect if the enemy is above a hole
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
