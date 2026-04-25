using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisFollow : MonoBehaviour
{
    private GameObject pl;
    private Vector3 movePos;
    [SerializeField] private float moveSpeed, zOffset;
    [SerializeField] private bool followZ;
    public float shakeLength, shakeStrength;
    [SerializeField] private bool isShaking;


    // Start is called before the first frame update
    void Start()
    {
        pl=GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movePos = new Vector3(pl.transform.position.x, pl.transform.position.y, pl.transform.position.z + zOffset);

        movePos.y = this.transform.position.y;

        if(!followZ)
        {
            movePos.z = this.transform.position.z;
        }

        if(transform.position != movePos)
        {
            Move();
        }
    }

    private void Move()
    {
        if(!isShaking)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, movePos, moveSpeed * Time.deltaTime);
        }
        else
        {
            this.transform.position = this.transform.position + (Random.insideUnitSphere * shakeStrength);
        }
        
    }

    public void TriggerShake()
    {
        isShaking = true;

        Invoke("StopShake", shakeLength);

    }

    private void StopShake()
    {
        isShaking = false;
    }
}
