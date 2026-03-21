using System.Collections;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemyTypes;
    private GameObject[] spawnLocations;
    private GameManager gameManager;
    public float spawnTime = 3, waveStartDelay = 2; //Make curve later, adapt to wave system
    private int spawnPoints;


    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        spawnLocations = GameObject.FindGameObjectsWithTag("Spawn Point");
    }

    public void startWave(int spawnAbility)
    {
        spawnPoints = spawnAbility;
        gameManager.waveFinished = false;
        StartCoroutine("SpawnEnemy");
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(waveStartDelay);

        while (spawnPoints > 0)
        {
            if (enemyTypes.Length == 0)
            {
                print("No valid enemies");
                break;
            }

            int spawnAt = Random.Range(0, spawnLocations.Length);
            int enemySelected = -1;

            while (enemySelected == -1)
            {
                enemySelected = Random.Range(0, enemyTypes.Length);
                int spawnCost = enemyTypes[enemySelected].GetComponent<Enemy>().spawnCost;
                int waveEnabled = enemyTypes[enemySelected].GetComponent<Enemy>().waveEnabled;
                int currentWave = gameManager.waveNumber;

                if (spawnPoints - spawnCost < 0 || waveEnabled > currentWave)
                {
                    enemySelected = -1;
                }
                else
                {
                    spawnPoints -= spawnCost;
                }
            }

            print("points: "+spawnPoints);
            Instantiate(enemyTypes[enemySelected], spawnLocations[spawnAt].transform.position, spawnLocations[spawnAt].transform.rotation);
            gameManager.currentEnemies++;

            yield return new WaitForSeconds(spawnTime);
        }

        gameManager.waveFinished = true;
    } 
}
