using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public int speed = 5;
    public int health = 10;
    public int strength = 1;
    public LayerMask m_LayerMask;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey("w"))
        {
            transform.position += Vector3.forward * speed * Time.deltaTime;
        }

        if (Input.GetKey("s"))
        {
            transform.position += Vector3.back * speed * Time.deltaTime;
        }

        if (Input.GetKey("a"))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        if (Input.GetKey("d"))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Basic attack animation, get the zombies in the direction facing.
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
            print("Dead");
        }
    }
}
