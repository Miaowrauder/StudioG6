
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class PlayerCombat : MonoBehaviour
{
    private Animator animator;
    private bool animationFree;
    public playerCombo plCombo;
    private playerMovement plMove;
    public int health;
    [Header("Damage Attack")]
    public GameObject sliceBurstPrefab;
    [Header("Push Attack")]
    public int power;
    public float pushRange;
    public GameObject pushBurstPrefab;
    [Header("Teapy Attack")]
    public float damageRadius, comboSpendDelay;
    public bool teapyActive;
    private bool canLoop;
    public GameObject teapyPos;
    public GameObject teapyPrefab;
    private GameObject teapotSpawned;

    void Start()
    {
        animator = GetComponent<Animator>();
        plMove = GetComponent<playerMovement>();
    }
    
    void Update()
    {
        animationFree = animator.GetCurrentAnimatorStateInfo(0).IsTag("Free");

        if(Input.GetKeyDown(KeyCode.Mouse0) && animationFree)
        {
            TriggerPush();
        }

        if(Input.GetKeyDown(KeyCode.E) && animationFree) //set proper binds later
        {
            if((plCombo.comboCount >= 3) && !teapyActive)
            {
                plCombo.ComboSpend(-3);
                TriggerSlice();
            }
        }

        if(Input.GetKeyDown(KeyCode.Q)) //set animator logic and proper binds later
        {
            if((plCombo.comboCount >= 1) && !teapyActive && (plMove.movement.magnitude != 0f) && animationFree)
            {
                teapotSpawned = Instantiate(teapyPrefab, teapyPos.transform.position, Quaternion.identity);
                teapotSpawned.transform.SetParent(this.gameObject.transform);

                animator.SetBool("Teapot", true);
                teapyActive = true;
                canLoop = true;
            }
        }

        if((Input.GetKeyUp(KeyCode.Q)) || (plCombo.comboCount == 0) || (plMove.movement.magnitude == 0f)) //end teapy time
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
    }

    private void TriggerPush()
    {
        animator.SetBool("Push", true); 
    }

    private void TriggerSlice()
    {
        animator.SetBool("Slice", true);
    }

    IEnumerator TeapyTick()
    {
        plCombo.ComboSpend(-1);
        yield return new WaitForSeconds(comboSpendDelay);
        canLoop = true;
    }

    void FixedUpdate()
    {
        if(teapyActive)
        {
            TeapyPosUpdate();
        }
    }

    private void TeapyPosUpdate()
    {
        Vector3 teapyVector = (plMove.movement * 2);
        teapotSpawned.transform.position = transform.position + teapyVector;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        print(health);

        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Call game end interface
        }
    }

    public void Slice() //Add to the animation
    {
        GameObject temp = Instantiate(sliceBurstPrefab, transform.position, Quaternion.identity);
        temp.transform.SetParent(this.gameObject.transform);
        animator.SetBool("Slice", false);
    }

    public void PushAttack()
    {
        animator.SetBool("Push", false);

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
}
