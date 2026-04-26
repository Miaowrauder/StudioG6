using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Logic Settings")]
    private NavMeshAgent navigation;
    private Animator animator;
    private PlayerCombat player;
    private ComboUI comboUI;
    private Rigidbody rigidbody;
    public Projectile projectile;
    [Header("Customise Settings")]
    public int spawnCost;
    [SerializeField] private float multiplier;
    public int waveEnabled;
    [SerializeField] private int health;
    [SerializeField] private bool damageImmune, isRanged, isFree;
    [SerializeField] private int strength;
    [SerializeField] private int meleeRange, rangedRange, retreatRange;
    [SerializeField] private Transform shootPos;
    [SerializeField] private GameObject attackPrefab;
    [Header("Falling Settings")]
    private int castInt;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float fallDelay, fallSpeed;
    private bool[] isHole = new bool[4];
    [SerializeField] private Transform[] castPos;
    RaycastHit hit;
    private bool isFalling, retreating;
    [SerializeField] private GameObject splashPrefab;
    private Vector2 movementVector;

    [Header("Death Settings")]
    [SerializeField] private float deathDelay;
    [SerializeField] private SpriteRenderer mySprite;
    [SerializeField] private GameObject deathBurstPrefab;
    private GameManager gameManager;

    [Header("SFX")]
    [SerializeField] private AudioSource[] dmgSFX;
    [SerializeField] private AudioSource hittingSFX;
    [SerializeField] private AudioSource laughSFX;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerCombat>();
        rigidbody = GetComponent<Rigidbody>();
        comboUI = FindObjectOfType<ComboUI>();
        gameManager = FindObjectOfType<GameManager>();

        navigation = GetComponent<NavMeshAgent>();
        navigation.speed = Random.Range(navigation.speed-3, navigation.speed+3);

        Instantiate(deathBurstPrefab, transform.position, Quaternion.identity); //dont take this out, its meant to be here!!!
        

        InvokeRepeating("UpdateFunctions", 0, 0.1f);
    }

    void FixedUpdate()
    {
        DownCast();

        if(isFalling)
        {
            navigation.enabled = false;
            animator.SetTrigger("Cancel Current");

            transform.position = new Vector3(transform.position.x, transform.position.y-fallSpeed, transform.position.z);
        }
    }

    // Update is called every 0.1 seconds for performance
    void UpdateFunctions()
    {
        // Determines if the enemy should be able to move and how
        isFree = animator.GetCurrentAnimatorStateInfo(0).IsTag("Free");

        if (!isFree || isFalling)
        {
            navigation.enabled = false;
        }
        else if (isRanged)
        {
            navigation.enabled = true;

            if (retreatRange > Vector3.Distance(player.transform.position, transform.position))
            {
                navigation.destination = transform.position - (player.transform.position - transform.position);
                retreating = true;
            }
            else
            {
                navigation.destination = player.transform.position;
                retreating = false;
            }
        }
        else
        {
            navigation.enabled = true;
            navigation.destination = player.transform.position;
        }

        // Checks whether the enemy is ranged or melee
        if(isFree)
        {
            if (!isRanged)
            {
                if (Vector3.Distance(player.transform.position, transform.position) < meleeRange && isFree)
                {
                    animator.SetBool("Attack Queued", true);
                    shootPos.transform.LookAt(player.transform, Vector3.forward); //aims the attack at the player on animation start as opposed to attack moement, better matches with sprite and easier to dodge
                }
            }
            else
            {
                if (Vector3.Distance(player.transform.position, transform.position) < rangedRange && isFree && !retreating)
                {
                    animator.SetBool("Attack Queued", true);
                }
            }
        }

        //Determines direction facing for animations
        movementVector = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.z - transform.position.z);
        
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
    }

    public void TakeDamage(int damage)
    {
        if(!damageImmune)
        {
            health -= damage;
        }
        else
        {
            animator.SetTrigger("Cancel Current");
            InvertLock();
            Invoke("InvertLock", 0.5f);
        }

        if (health <= 0)
        {
            if (!animator.GetBool("Action Lock"))
            {
                InvertLock();
            }

            animator.SetBool("Attack Queued", false);
            animator.SetTrigger("Cancel Current");
            navigation.enabled = false;
            mySprite.color = new Color(1,0.6f,0.6f,1);
            Invoke("Destroy", deathDelay);
        }
        foreach (AudioSource sfx in dmgSFX)
        {
            sfx.Play();
        }
    }

    public void Destroy()
    {
        gameManager.currentEnemies --;

        comboUI.Score += (100*spawnCost) * (1 + (comboUI.ScoreMult * 2));

        Instantiate(deathBurstPrefab, transform.position, Quaternion.identity);

        if (!isFree)
        {
            animator.SetTrigger("Cancel Current");
        }

        Destroy(gameObject);
    }

    // Attacks the player melee
    public void Attack()
    {
        animator.SetBool("Attack Queued", false);
        Invoke("Attack2", 0.01f);
    }

    private void Attack2() //adds a tiny little delay before attacking to feel in time w/ animation & vfx
    {
        GameObject temp = Instantiate(attackPrefab, shootPos.position, shootPos.rotation);
        hittingSFX.Play();
    }

    
    // Attacks the player ranged
    public void ProjectileThrow()
    {
        animator.SetBool("Attack Queued", false);
        Projectile spawnedProjectile = Instantiate(projectile, shootPos.transform.position, transform.rotation);
    }

    // Triggered by player push to start goblin wobble
    public void StartWobble()
    {
        animator.SetBool("Wobble Queued", true);
        animator.SetBool("Attack Queued", false);

        if (!isFree)
        {
            animator.SetTrigger("Cancel Current");
        }
    }

    // Triggered by goblin animation to stop
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

    // inverts the ability to perform an action in the animator
    private void InvertLock()
    {
        animator.SetBool("Action Lock", !animator.GetBool("ActionLock"));
        animator.SetBool("Attack Queued", false);
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
        }
    }

    void Fall()
    {
        isFalling = true;
        GameObject temp = Instantiate(splashPrefab, transform.position, Quaternion.identity);
        temp.transform.Rotate(-90f, 0f, 0f);
        Invoke("Destroy", 1f);
    }
}
