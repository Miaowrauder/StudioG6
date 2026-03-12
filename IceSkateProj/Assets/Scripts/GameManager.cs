using UnityEngine;

public class GameManager : MonoBehaviour
{
private Spawner spawner;
private bool playWave = true;
public int waveNumber = 0;
public int baseSpawnPoints = 50;
public int currentEnemies = 0;

    void Start()
    {
        spawner = FindAnyObjectByType<Spawner>();
    }

    void FixedUpdate()
    {
        if (currentEnemies == 0)
        {
            waveNumber++;
            int waveSpawnPoints = baseSpawnPoints*waveNumber;
            spawner.startWave(waveSpawnPoints); //Make scale better, maybe 
        }
    }
}
