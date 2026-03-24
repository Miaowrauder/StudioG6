

using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 startPosition, endPosition, midPoint;
    private PlayerCombat player;
    private float completion = 0;
    public float speed = 3;
    public float holeScale, bakeDelay, randomDistance;
    public GameObject holePrefab, visualPrefab;
    private GameObject visualComponent;
    private float elapsedTime, journeyTime;


    // Start is called before the first frame update
    void Start()
    {
        visualComponent = Instantiate(visualPrefab, transform.position, Quaternion.identity);
        player = FindObjectOfType<PlayerCombat>();

        startPosition = transform.position;
        float r = Random.Range(-randomDistance,randomDistance);
        float r1 = Random.Range(-randomDistance,randomDistance);
        endPosition = new Vector3(player.transform.position.x+r, player.transform.position.y, player.transform.position.z+r1);
        journeyTime = Vector3.Distance(startPosition, endPosition) / speed;

        visualComponent.GetComponent<mortarVisual>().receivedDistance = journeyTime;
        visualComponent.GetComponent<mortarVisual>().initiate = true;
    }


    void FixedUpdate()
    {
        visualComponent.transform.position = new Vector3(transform.position.x, visualComponent.transform.position.y, transform.position.z);
        elapsedTime += Time.deltaTime/journeyTime;
        transform.position = Vector3.Slerp(startPosition, endPosition, elapsedTime);

        if (transform.position == endPosition)
        {
            Vector3 holePos = new Vector3(transform.position.x, -4.22f, transform.position.z);
            GameObject hole = Instantiate(holePrefab, holePos, Quaternion.identity);
            hole.transform.localScale = new Vector3(holeScale, 1, holeScale);
            FindObjectOfType<GameManager>().GetComponent<GameManager>().RebakeNavmesh(bakeDelay);
            Destroy(visualComponent);
            Destroy(gameObject);
        }
    }
}
