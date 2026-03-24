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
    void Update()
    {
        this.transform.position += Vector3.up * speed * Time.deltaTime;
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
