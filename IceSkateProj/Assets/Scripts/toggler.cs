using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggler : MonoBehaviour
{
    public float toggleSpeed;
    public TrailRenderer trail;
    public ParticleSystem particle;
    private bool flip;
    private bool canLoop = true;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
    }
    void Update()
    {
        if(canLoop && (rb.velocity.magnitude > 1))
        {
            canLoop = false;
            StartCoroutine(ToggleState());
        }
    }

    // Update is called once per frame
    private IEnumerator ToggleState()
    {
        if(flip)
        {
            particle.Stop();
            trail.emitting = false;

            flip = false;
            yield return new WaitForSeconds(toggleSpeed);
        }
        else if(!flip)
        {
            particle.Play();
            trail.emitting = true;

            flip = true;
            yield return new WaitForSeconds(toggleSpeed/2f);
        }

        
        canLoop = true;
        
    }
}
