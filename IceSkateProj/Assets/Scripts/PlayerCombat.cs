using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    public int health = 10;
    public int strength = 1;
    public float power = 5;
    public float range = 10;
    public float modifier = 1;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            //DamageAttack(); //Change to be animation trigger
            PushAttack();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        print(health);

        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Call game end interface
        }
    }

    public void DamageAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Enemy>() != null)
            {
                hitCollider.SendMessage("TakeDamage", strength);
            }
        }
    }

    public void PushAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Enemy>() != null)
            {
                if (GetComponent<Collider>().TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    rb.AddExplosionForce(power, transform.position, range, 1, ForceMode.Force); //Use AddForce if method doesn't work
                }
            }
        }
    }
}
