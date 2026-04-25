
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 startPosition, endPosition, midPoint;
    private PlayerCombat player;
    private float completion = 0;
    public float speed = 3;
    public float holeScale, bakeDelay, randomDistance;
    public GameObject holePrefab, visualPrefab, shadowPrefab;
    private GameObject visualComponent, shadow;
    private float elapsedTime, journeyTime;
    private AxisFollow cam;


    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindObjectOfType<AxisFollow>();
        visualComponent = Instantiate(visualPrefab, transform.position, Quaternion.identity);
        shadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);
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
        visualComponent.transform.position = new Vector3(this.transform.position.x, visualComponent.transform.position.y, this.transform.position.z);
        elapsedTime += Time.deltaTime/journeyTime;
        transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime);
        shadow.transform.position = new Vector3(this.transform.position.x, -3.699f, this.transform.position.z);

        if (transform.position == endPosition)
        {
            cam.shakeLength = 0.1f;
            cam.shakeStrength = 0.3f;
            cam.TriggerShake();

            Vector3 holePos = new Vector3(transform.position.x, -4.22f, transform.position.z);
            GameObject hole = Instantiate(holePrefab, holePos, Quaternion.identity);
            hole.transform.localScale = new Vector3(holeScale, 1, holeScale);
            FindObjectOfType<GameManager>().GetComponent<GameManager>().RebakeNavmesh(bakeDelay);
            Destroy(visualComponent);
            Destroy(shadow);
            Destroy(gameObject);
        }
    }
}
