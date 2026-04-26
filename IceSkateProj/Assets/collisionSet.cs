using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var col = ps.collision;
        col.enabled = true;
        col.type = ParticleSystemCollisionType.Planes;
        col.mode = ParticleSystemCollisionMode.Collision3D;
        col.dampen = 1f;

        col.AddPlane(GameObject.FindWithTag("Ground").transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
