using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 startPosition, endPosition, midPoint;
    private PlayerCombat player;
    private float completion = 0;
    public float speed = 3;
    private float elapsedTime, journeyTime;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerCombat>();

        startPosition = transform.position;
        endPosition = player.transform.position;
        journeyTime = Vector3.Distance(startPosition, endPosition) / speed;
    }


    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime/journeyTime;
        transform.position = Vector3.Slerp(startPosition, endPosition, elapsedTime);

        if (transform.position == endPosition)
        {
            //Cut hole (put code here)
            Destroy(gameObject);
        }
    }
}
