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
public int points;
private NavMeshSurface navmeshSurface;
public GameObject falseIcePrefab;
private GameObject spawnedIce;
private bool risingIce;

    void Start()
    {
        spawner = FindAnyObjectByType<Spawner>();
    }

    void FixedUpdate()
    {
        if (currentEnemies <= 0 && waveFinished)
        {
            NewWave();
        }

        if((risingIce = true) && (spawnedIce != null))
        {
            if(spawnedIce.transform.position.y <= -3.71f) //not above current ice
            {
               spawnedIce.transform.position = new Vector3(spawnedIce.transform.position.x, spawnedIce.transform.position.y + 0.01f , spawnedIce.transform.position.z); 
            }
            else
            {
                risingIce = false;
                ResetIce();
                Destroy(spawnedIce);
            }
            
            
        }
    }

    void NewWave()
    {
        waveNumber++;
        waveSpawnPoints = (int)(waveSpawnPoints*waveMultiplier);
        spawner.startWave(waveSpawnPoints);
        FalseIce(); //Replace this method later
    }

    private void FalseIce()
    {
        Vector3 icePos = new Vector3(-3.06f,-4.2f,-12.46f);
        spawnedIce = Instantiate(falseIcePrefab, icePos, Quaternion.identity);
        risingIce = true;
    }

    public void RebakeNavmesh(float delay)
    {
        Invoke("Bake", delay);
    }

    private void ResetIce()
    {
        GameObject[] holes = GameObject.FindGameObjectsWithTag("Hole");

        foreach (GameObject hole in holes)
        {
            Destroy(hole);
        }

        holes = GameObject.FindGameObjectsWithTag("Spawned");

        foreach (GameObject hole in holes)
        {
            Destroy(hole);
        }

        RebakeNavmesh(0.01f);
    }

    private void Bake()
    {
        navmeshSurface = GameObject.Find("NavMesh").GetComponent<NavMeshSurface>();
        navmeshSurface.BuildNavMesh();

        Debug.Log("Rebaked The Navmesh!");
    }
}
