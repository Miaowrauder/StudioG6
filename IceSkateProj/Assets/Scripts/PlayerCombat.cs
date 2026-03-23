using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    public int health;
    [Header("Damage Attack")]
    public int strength;
    public float damageRange;
    [Header("Push Attack")]
    public int power;
    public float pushRange;
    
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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRange);
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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pushRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Enemy>() != null)
            {
                if (hitCollider.GetComponent<Collider>().TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    print("push "+rb);
                    rb.AddExplosionForce(power, transform.position, pushRange);
                }
            }
        }
    }
}
