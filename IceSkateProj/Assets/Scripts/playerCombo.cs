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
    public GameObject[] comboVisuals;
    public AudioSource[] comboAudibles;
    public float trickDur, trickCooldown;
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
            ComboSpend(1);
            isTricking = true;
            ComboVisual();
            Invoke("TrickCD", trickDur + trickCooldown);
        }
    }
    private void TrickCD()
    {
        isTricking = false;
    }

    public void ComboSpend(int amountChanged)
    {
        comboCount += amountChanged;

        if(comboCount < 0)
        {
            comboCount = 0;
        }
        else if(comboCount > 5)
        {
            comboCount = 5;
        }
        Instantiate(comboVisuals[comboCount], transform.position, Quaternion.identity);
        comboAudibles[comboCount - 1].Play();
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
