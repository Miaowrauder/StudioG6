using System.Threading;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] zombieTypes;
    public float spawnTime = 3;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnZombie", 0, spawnTime);
    }

    // Update is called once per frame
    private void SpawnZombie()
    {
        Instantiate(zombieTypes[Random.Range(0, zombieTypes.Length)], transform.position, transform.rotation);
    } 
}
