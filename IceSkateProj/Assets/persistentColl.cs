using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class persistentColl : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "Enemy")
        {
            coll.GetComponent<Enemy>().TakeDamage(999);
        }
    }
}
