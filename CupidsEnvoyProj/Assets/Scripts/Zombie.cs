using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private NavMeshAgent navigation;
    public Player player;
    
    void Start()
    {
        navigation = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        navigation.destination = player.transform.position;
    }
}
