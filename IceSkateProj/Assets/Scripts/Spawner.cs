using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] zombieTypes;
    private GameObject[] spawnPoints;
    public int currentZombie, maxZombies, minZombieDifficulty, maxZombieDifficulty; //Work out difficulties in future
    public float spawnTime = 3; //Make curve later, adapt to wave system

    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawn Point");
        startWave();
    }

    public void startWave()
    {
        currentZombie = 0;
        StartCoroutine("SpawnZombie");
    }

    IEnumerator SpawnZombie()
    {
        while (currentZombie < maxZombies)
        {
            if (zombieTypes.Length == 0)
            {
                print("No valid zombies");
                break;
            }

            int spawnAt = Random.Range(0, spawnPoints.Length);
            int zombieSelected = Random.Range(0, zombieTypes.Length);

            Instantiate(zombieTypes[zombieSelected], spawnPoints[spawnAt].transform.position, spawnPoints[spawnAt].transform.rotation);
            currentZombie++;
            print("Current Zombie: "+currentZombie);

            yield return new WaitForSeconds(spawnTime);
        }
        
        print("Wave finished"); // Send signal back to wave/game manager
    } 
}
