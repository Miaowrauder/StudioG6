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
    public bool canMove = true;
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
        // Check that an attack animation is playing, if so, stop moving - tweaked this, messed with fall logic
        bool isFree = animator.GetCurrentAnimatorStateInfo(0).IsTag("Free");
        if (!isFree || isFalling)
        {
            canMove = false;
        }
        else if(isFree)
        {
            canMove = true;
        }

        if(navigation.enabled && !isFalling)
        {
            if(canMove)
            {
                navigation.destination = player.transform.position;
            }
            else
            {
                navigation.destination = transform.position;
            }
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

    public void Pushed(float stopTime)
    {
        ChangeMovementMethod();
        Invoke("ChangeMovementMethod", stopTime);
    }

    private void ChangeMovementMethod()
    {
        rigidbody.velocity = Vector3.zero;
        navigation.enabled = !navigation.enabled;
    }

    public void ProjectileThrow()
    {
        Projectile spawnedProjectile = Instantiate(projectile, transform.position, transform.rotation);
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
