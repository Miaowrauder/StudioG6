using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] zombieTypes;
    private GameObject[] spawnLocations;
    private GameManager gameManager;
    public float spawnTime = 3; //Make curve later, adapt to wave system
    private int spawnPoints;


    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        spawnLocations = GameObject.FindGameObjectsWithTag("Spawn Point");
    }

    public void startWave(int spawnAbility)
    {
        spawnPoints = spawnAbility;
        StartCoroutine("SpawnZombie");
    }

    IEnumerator SpawnZombie()
    {
        while (spawnPoints > 0)
        {
            if (zombieTypes.Length == 0)
            {
                print("No valid zombies");
                break;
            }

            int spawnAt = Random.Range(0, spawnLocations.Length);
            int zombieSelected = -1;

            while (zombieSelected == -1)
            {
                zombieSelected = Random.Range(0, zombieTypes.Length);
                int spawnCost = zombieTypes[zombieSelected].GetComponent<Zombie>().spawnCost;
                if (spawnPoints - spawnCost < 0)
                {
                    zombieSelected = -1;
                }
                else
                {
                    spawnPoints -= spawnCost;
                }
            }

            Instantiate(zombieTypes[zombieSelected], spawnLocations[spawnAt].transform.position, spawnLocations[spawnAt].transform.rotation);
            gameManager.currentEnemies++;

            yield return new WaitForSeconds(spawnTime);
        }
    } 
}
