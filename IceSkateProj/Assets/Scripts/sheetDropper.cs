using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sheetDropper : MonoBehaviour
{
    public float dropSpeed;
    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = new Vector3(transform.position.x, transform.position.y-dropSpeed, transform.position.z);
    }
}
