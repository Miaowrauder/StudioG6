using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private NavMeshAgent navigation;
    public Player player;
    public int health = 2;
    public int strength = 1;
    
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

    public IEnumerator TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        yield return new WaitForSeconds(1);
    }

    //Once in range, attack animation
}
