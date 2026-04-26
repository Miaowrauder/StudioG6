using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comboNote : MonoBehaviour
{
    public float lifespan, speed, scalePower;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("Death", lifespan);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position += Vector3.up * speed * Time.deltaTime;

        this.transform.Rotate(0, 1, 0);
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
