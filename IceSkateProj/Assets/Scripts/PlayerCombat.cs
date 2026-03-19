using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    public int health = 10;
    public int strength = 1;

    void Start()
    {

    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.GetComponent<Enemy>() != null)
                {
                    hitCollider.SendMessage("TakeDamage", strength);
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        print(health);

        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Temp 
        }
    }
}
