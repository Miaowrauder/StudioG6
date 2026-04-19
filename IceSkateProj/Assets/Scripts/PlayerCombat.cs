using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    private Animator animator;
    private bool animationFree;
    [SerializeField] private playerCombo plCombo;
    private playerMovement plMove;
    public int health;
    [SerializeField] private SpriteRenderer mySprite;
    [Header("Damage Attack")]
    [SerializeField] private GameObject sliceBurstPrefab;
    [Header("Push Attack")]
    [SerializeField] private int power;
    [SerializeField] private float pushRange;
    [SerializeField] private GameObject pushBurstPrefab;
    [Header("Teapy Attack")]
    public float damageRadius, comboSpendDelay;
    public bool teapyActive;
    private bool canLoop;
    [SerializeField] private  GameObject teapyPos;
    [SerializeField] private  GameObject teapyPrefab;
    private GameObject teapotSpawned;

    void Start()
    {
        animator = GetComponent<Animator>();
        plMove = GetComponent<playerMovement>();
    }
    
    void Update()
    {
        animationFree = animator.GetCurrentAnimatorStateInfo(0).IsTag("Free");

        // Triggers push
        if(Input.GetKeyDown(KeyCode.Mouse1) && animationFree)
        {
            animator.SetBool("Push", true);
            if (animationFree)
            {
                animator.SetTrigger("Stop Current");
            }
        }

        // Triggers slice
        if(Input.GetKeyDown(KeyCode.E) && animationFree) //set proper binds later
        {
            if((plCombo.comboCount >= 3) && !teapyActive)
            {
                plCombo.ComboSpend(-3);
                animator.SetBool("Slice", true);

                if (animationFree)
                {
                    animator.SetTrigger("Stop Current");
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Mouse2)) //set animator logic and proper binds later
        {
            if((plCombo.comboCount >= 1) && !teapyActive && (plMove.movement.magnitude != 0f) /*&& animationFree*/)
            {
                teapotSpawned = Instantiate(teapyPrefab, teapyPos.transform.position, Quaternion.identity);
                teapotSpawned.transform.SetParent(this.gameObject.transform);

                if (animationFree)
                {
                    animator.SetTrigger("Stop Current");
                }

                animator.SetBool("Teapot", true);
                teapyActive = true;
                canLoop = true;
            }
        }

        if((Input.GetKeyUp(KeyCode.Mouse2)) || (plCombo.comboCount == 0) || (plMove.movement.magnitude == 0f)) //end teapy time
        {
            animator.SetBool("Teapot", false);
            teapyActive = false;
            Destroy(teapotSpawned);
        }

        if(teapyActive && canLoop)
        {
            canLoop = false;
            StartCoroutine(TeapyTick());
        }

        if(teapyActive)
        {
            TeapyPosUpdate();
        }
    }

    IEnumerator TeapyTick()
    {
        plCombo.ComboSpend(-1);
        yield return new WaitForSeconds(comboSpendDelay);
        canLoop = true;
    }

    private void TeapyPosUpdate()
    {
        Vector3 teapyVector = (plMove.movement * 3);
        teapotSpawned.transform.position = transform.position + teapyVector;
    }

    private void Death()
    {
        MenuManager mm = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        mm.OnFail();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        print(health);

        if (health <= 0)
        {
            mySprite.color = new Color(1,0.6f,0.6f,1);
            Invoke("Death",1f);
        }
    }

    private void Slice()
    {
        GameObject temp = Instantiate(sliceBurstPrefab, transform.position, Quaternion.identity);
        temp.transform.SetParent(this.gameObject.transform);
    }

    private void Push()
    {
        Instantiate(pushBurstPrefab, transform.position, Quaternion.identity);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pushRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Enemy>() != null)
            {
                if (hitCollider.GetComponent<Collider>().TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    hitCollider.GetComponent<Enemy>().StartWobble();
                    rb.AddExplosionForce(power, transform.position, pushRange);
                }
            }
        }
    }

    public void EndPush()
    {
        animator.SetBool("Push", false);
    }

    public void EndSlice()
    {
        animator.SetBool("Slice", false);
    }
}
