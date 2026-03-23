using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Collections;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
private Spawner spawner;
public bool waveFinished = true;
public int waveNumber, baseSpawnPoints, currentEnemies;
private NavMeshSurface navmeshSurface;

    void Start()
    {
        spawner = FindAnyObjectByType<Spawner>();
    }

    void FixedUpdate()
    {
        if (currentEnemies == 0 && waveFinished)
        {
            NewWave();
        }
    }

    void NewWave()
    {
        waveNumber++;
        int waveSpawnPoints = baseSpawnPoints*waveNumber;
        spawner.startWave(waveSpawnPoints); //Make scale better, maybe 
        print("Wave "+waveNumber);
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
