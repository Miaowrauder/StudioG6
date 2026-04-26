using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadowFollow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject shadowPrefab;
    private GameObject spawnedShadow;
    [SerializeField] private float yOffset;

    void Start()
    {
        spawnedShadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 shadowPos = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);

        if(spawnedShadow.transform.position != shadowPos)
        {
            
            spawnedShadow.transform.position = shadowPos;
        }
    }
}
