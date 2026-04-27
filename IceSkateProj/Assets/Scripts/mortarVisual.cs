using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mortarVisual : MonoBehaviour
{
    private Rigidbody rb;
    public float launchMult, receivedDistance;
    public bool initiate;
    
    void Update()
    {
        if(initiate)
        {
            initiate = false;
            rb = this.gameObject.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(rb.velocity.x, receivedDistance * launchMult, rb.velocity.z);
        }
    }
}
