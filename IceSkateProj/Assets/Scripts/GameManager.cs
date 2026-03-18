using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class GameManager : MonoBehaviour
{
private Spawner spawner;
private bool playWave = true;
public int waveNumber = 0;
public int baseSpawnPoints = 50;
public int currentEnemies = 0;
private NavMeshSurface navmeshSurface;

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
            print("Wave "+waveNumber);
        }
    }

    public void RebakeNavmesh(float delay)
    {
        Invoke("Bake", delay);
    }

    private void Bake()
    {
        navmeshSurface = GameObject.Find("NavMesh").GetComponent<NavMeshSurface>();
        navmeshSurface.BuildNavMesh();

        Debug.Log("Rebaked The Navmesh!");
    }
}
