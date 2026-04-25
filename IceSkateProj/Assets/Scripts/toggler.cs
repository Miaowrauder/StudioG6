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
    [SerializeField] private float SFXInterval = 0.60f;
    [SerializeField] private AudioSource skateSFX;
    [SerializeField] private AudioClip[] variations = new AudioClip[5];
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

        /*if (SFXInterval > 0f)
        {
            SFXInterval -= Time.deltaTime;
        }
        else
        {
            
        }*/
    }

    // Update is called once per frame
    private IEnumerator ToggleState()
    {
        if(flip)
        {
            particle.Stop();
            trail.emitting = false;

            flip = false;
            yield return new WaitForSeconds(toggleSpeed + rb.velocity.magnitude/100f);
        }
        else if(!flip)
        {
            particle.Play();
            PlaySkateFX();
            trail.emitting = true;

            flip = true;
            yield return new WaitForSeconds((toggleSpeed + rb.velocity.magnitude/100f)/2f);
        }

        
        canLoop = true;
        
    }
    private void PlaySkateFX()
    {
        float pitch = Random.Range(1f, 1.5f);
        AudioClip FX = variations[Random.Range(0, variations.Length - 1)];

        skateSFX.clip = FX;
        skateSFX.pitch = pitch;
        skateSFX.Play();
    }
}
