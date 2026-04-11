using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class persistentColl : MonoBehaviour
{
    public float lifetime;
    // Start is called before the first frame update

    void Start()
    {
        if(lifetime > 0)
        {
            Invoke("Death", lifetime);
        }
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "Enemy")
        {
            coll.GetComponent<Enemy>().TakeDamage(999);
        }
    }
}
