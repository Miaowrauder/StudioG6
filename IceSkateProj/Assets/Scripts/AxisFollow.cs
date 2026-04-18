using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisFollow : MonoBehaviour
{
    private GameObject pl;
    private Vector3 movePos;
    public float moveSpeed, zOffset;
    public bool followZ;
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
        this.transform.position = Vector3.Lerp(this.transform.position, movePos, moveSpeed * Time.deltaTime);
    }
}
