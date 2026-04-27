using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemyTypes;
    private GameObject[] spawnLocations;
    private GameManager gameManager;
    public AnimationCurve spawnTime;
    public float waveStartDelay;
    private int spawnPoints, enemyTotal;


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
        enemyTotal = 0;

        yield return new WaitForSeconds(waveStartDelay);
        while (spawnPoints >= 10) // Set to lowest spawn cost
        {
            if (enemyTypes.Length == 0)
            {
                print("No valid enemies");
                break;
            }

            int spawnAt = Random.Range(0, spawnLocations.Length);
            int enemySelected = -1;

            while (enemySelected == -1) // Randomly selects a goblin
            {
                enemySelected = Random.Range(0, enemyTypes.Length);
                int spawnCost = enemyTypes[enemySelected].GetComponent<Enemy>().spawnCost;
                int waveEnabled = enemyTypes[enemySelected].GetComponent<Enemy>().waveEnabled;
                int currentWave = gameManager.waveNumber;

                if (spawnPoints - spawnCost < 0 || waveEnabled > currentWave) // checks if the goblin is valid to spawn
                {
                    enemySelected = -1;
                }
                else
                {
                    spawnPoints -= spawnCost;
                    Instantiate(enemyTypes[enemySelected], spawnLocations[spawnAt].transform.position, spawnLocations[spawnAt].transform.rotation);
                    gameManager.currentEnemies++;
                }
            }

            yield return new WaitForSeconds(spawnTime.Evaluate(enemyTotal));
        }

        gameManager.waveFinished = true;
    } 
}
