using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCombo : MonoBehaviour
{
    private GameObject player;
    private Animator animator;
    private PlayerCombat plC;
    private playerMovement plM;
    public int comboCount;
    [SerializeField] private GameObject[] comboVisuals;
    [SerializeField] private float trickDur, trickCooldown;
    [SerializeField] private float miniNoteDelay, miniNoteScale;
    [SerializeField] private ParticleSystem comboParticles;
    public AudioSource[] comboAudibles;
    public bool isTricking;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindWithTag("Player");

        plC = player.GetComponent<PlayerCombat>();
        plM = player.GetComponent<playerMovement>();

        rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        animator = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isTricking && Input.GetKeyDown(KeyCode.Space) && (plM.isTrailing == false) && (rb.velocity.magnitude > 5))
        {
            Invoke("DelaySpend",1f);
            isTricking = true;
            ComboVisual();
            Invoke("TrickCD", trickDur + trickCooldown);
        }
    }
    private void TrickCD()
    {
        isTricking = false;
    }

    private void DelaySpend()
    {
        ComboSpend(1);
    }

    public void ComboSpend(int amountChanged)
    {
        comboCount += amountChanged;

        
        if(comboCount > 5) //swapped the if round for that sweet cpu instruction pre-fetch optimisation
        {
            comboCount = 5;
        }
        else if(comboCount < 0)
        {
            comboCount = 0;
        }

        var em = comboParticles.emission; //cant declare emitter vars up top fsr...

        em.rateOverTime = (comboCount*1.5f);
        
        if(comboCount == 5)
        {
            em.rateOverTime = (comboCount*3f);
        }
        
        ComboNoteSpawn();
        
        if(comboCount > 0)
        {
            comboAudibles[comboCount - 1].Play();
        }
        
    }

    private void ComboNoteSpawn()
    {
        Vector3 notePos = new Vector3((transform.position.x + Random.Range(-1.5f, 1.5f)), transform.position.y +3.5f, (transform.position.z + Random.Range(-1.5f, 1.5f)));
        Instantiate(comboVisuals[comboCount], notePos, Quaternion.identity);
    }

    private void ComboVisual()
    {
        animator.SetBool("Flourish", true);
        animator.SetTrigger("Stop Current");
        plM.maxSpeed += 5f;
        plM.diagMaxSpeed += 3.4f;
        plM.moveSpeed += 2f;
    }

    private void RevertVisual()
    {
        plM.maxSpeed -= 5f;
        plM.diagMaxSpeed -= 3.4f;
        plM.moveSpeed -= 2f;
        animator.SetBool("Flourish", false);
    }
}
