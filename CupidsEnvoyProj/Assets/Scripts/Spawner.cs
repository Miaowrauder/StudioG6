using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] zombieTypes;
    private GameObject[] spawnPoints;
    public float spawnTime = 3;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawn Point");
        InvokeRepeating("SpawnZombie", 0, spawnTime);
    }

    // Update is called once per frame
    private void SpawnZombie()
    {
        int spawnAt = Random.Range(0, spawnPoints.Length);
        print(spawnAt);
        Instantiate(zombieTypes[Random.Range(0, zombieTypes.Length)], spawnPoints[spawnAt].transform.position, spawnPoints[spawnAt].transform.rotation);
    } 
}
