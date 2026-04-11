
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class PlayerCombat : MonoBehaviour
{
    private Animator animator;
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
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
             TriggerPush();
             
        }

        if(Input.GetKeyDown(KeyCode.E)) //set animator logic and proper binds later
        {
            if((plCombo.comboCount >= 3) && !teapyActive)
            {
                plCombo.ComboSpend(-3);
                TriggerSlice();
            }

        }

        if(Input.GetKeyDown(KeyCode.Q)) //set animator logic and proper binds later
        {
            if((plCombo.comboCount >= 1) && !teapyActive && (plMove.movement.magnitude != 0f))//and animator is free
            {
                teapotSpawned = Instantiate(teapyPrefab, teapyPos.transform.position, Quaternion.identity);
                teapotSpawned.transform.SetParent(this.gameObject.transform);

                teapyActive = true;
                canLoop = true;
            }
            
            //set anim state
        }

        if((Input.GetKeyUp(KeyCode.Q)) || (plCombo.comboCount == 0) || (plMove.movement.magnitude == 0f)) //end teapy time
        {
            teapyActive = false;
            Destroy(teapotSpawned);
            //set anim state!
        }

        if(teapyActive && canLoop)
        {
            canLoop = false;
            StartCoroutine(TeapyTick());
        }
    }

    private void TriggerPush()
    {
        animator.SetTrigger("Push_Attack");
        
    }

    private void TriggerSlice()
    {
        
        animator.SetTrigger("Damage_Attack");
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

    public void DamageAttack()
    {
        GameObject temp = Instantiate(sliceBurstPrefab, transform.position, Quaternion.identity);
        temp.transform.SetParent(this.gameObject.transform);

    }

    public void PushAttack()
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
}
