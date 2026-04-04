using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Collections;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
private Spawner spawner;
public bool waveFinished = true;
public int waveNumber, waveSpawnPoints;
public float waveMultiplier;
public int currentEnemies;
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
        waveSpawnPoints = (int)(waveSpawnPoints*waveMultiplier);
        spawner.startWave(waveSpawnPoints);
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
