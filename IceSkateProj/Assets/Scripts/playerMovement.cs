using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float moveSpeed, maxSpeed, fallDelay;
    private float checkSpeed, diagMaxSpeed; //changes between max and diag to adjust speed checks
    private float currentSpeed;
    private Rigidbody rb;
    public bool canMove;


    private bool isTrailing, trailReset; //tracks when trailing for later uses, triggers trail reset
    private int currentPoint; //point in trail array
    public float trailPointDelay;
    public GameObject tempTrailMarker; //currently visible for debug, small colliders to detect when player overlaps trail, trigger cut
    private GameObject[] createdMarkers = new GameObject[99];
    public Vector2[] trailPoint; //saved trail points
    private iceCutter myCutter;
    public int passedStartPoint;
    private int castInt;
    public Transform[] castPos;
    public LayerMask layerMask;
    private bool[] isHole = new bool[4];
    public GameObject cutTrail;
    private GameObject spawnedCutTrail;


    RaycastHit hit;
                            
    // Start is called before the first frame update
    void Start()
    {
        myCutter = GetComponent<iceCutter>();
        diagMaxSpeed = maxSpeed * 0.666f;
        trailPoint = new Vector2[98];
        trailReset = true;
        rb = GetComponent<Rigidbody>();
    }
    public void PassPoints()
    {
        for(int i = 0; i < (myCutter.passedPoints.Length - 1); i++)
        {
            if(trailPoint[i] != new Vector2(0,0))
            {
                myCutter.passedPoints[i] = trailPoint[passedStartPoint + i];
            }
        }

        EndTrail();
        myCutter.triggerCut = true;
        
    }

    private void EndTrail()
    {
        if(spawnedCutTrail != null)
        {
           Destroy(spawnedCutTrail); 
        }
        
        ClearMarkers();
        CancelInvoke("TrailGen");
        trailReset = true;
        isTrailing = false;
    }

    void Update() //fixed update gets funny when we need individual frame inputs from mouse up and down...
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            isTrailing = true;

            if(trailReset)
            {
                currentPoint = 0;
                ClearTrailPoints();
                trailReset = false;
                InvokeRepeating("TrailGen",Time.deltaTime, trailPointDelay);

                Vector3 trailPos = new Vector3(this.transform.position.x, this.transform.position.y-1.5f, this.transform.position.z);
                spawnedCutTrail = Instantiate(cutTrail, trailPos, Quaternion.identity);

                spawnedCutTrail.transform.SetParent(this.transform);
            }
            
            
        }
        else if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            EndTrail();
        }

        if(trailPoint[97] != new Vector2(0,0))
        {
            EndTrail();
        }
    }
    void FixedUpdate()
    {
        DownCast();

        if(canMove)
        {
            Move();
        }
        
    }

    private void TrailGen()
    {
        trailPoint[currentPoint] = new Vector2(transform.position.x, transform.position.z);

        createdMarkers[currentPoint] = Instantiate(tempTrailMarker, transform.position, Quaternion.identity);
        createdMarkers[currentPoint].GetComponent<TrailColliders>().pointNumber = currentPoint+1;

        currentPoint ++;

        if(currentPoint == trailPoint.Length)
        {
            CancelInvoke("TrailGen");
            trailReset = true;
            isTrailing = false;
        }
    }

    private void ClearTrailPoints()
    {
        for(int i = 0; i < trailPoint.Length; i++)
        {
            trailPoint[i] = new Vector2(0,0);
        }
    }

    private void ClearMarkers()
    {
        for(int i = 0; i < createdMarkers.Length; i++)
        {
            if(createdMarkers[i] != null)
            {
                Destroy(createdMarkers[i]);
            }
        }
    }

    private void Move()
    {
        Vector2 inputs = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 movement = new Vector3(inputs.x, 0, inputs.y);

        if((inputs == new Vector2(1,1))||(inputs == new Vector2(-1,1))||(inputs == new Vector2(1,1))||(inputs == new Vector2(-1,-1))) //are we detecting double inputs, i.e moving diagonally
        {
            checkSpeed = diagMaxSpeed;
        }
        else
        {
            checkSpeed = maxSpeed;
        }

        rb.AddForce(movement, ForceMode.Impulse);

        VelocityCheck();
    }

    private void VelocityCheck()
    {

        if(rb.velocity.x > checkSpeed)
        {
            rb.velocity = new Vector3(checkSpeed, rb.velocity.y, rb.velocity.z);
        }
        else if(rb.velocity.x < -checkSpeed)
        {
            rb.velocity = new Vector3(-checkSpeed, rb.velocity.y, rb.velocity.z);
        }

        if(rb.velocity.z > checkSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, checkSpeed);
        }
        else if(rb.velocity.z < -checkSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -checkSpeed);
        }

    }

    private void Fall()
    {
        canMove = false;
        rb.velocity = new Vector3(rb.velocity.x/2, -10, rb.velocity.z/2);
        gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
    }

    private void CancelFall()
    {
        canMove = true;
        gameObject.GetComponent<CapsuleCollider>().isTrigger = false;
    }

    void DownCast() 
    {
        castInt++; //does 1 corner per frame, cycling

        if(castInt >= 4)
        {
            castInt = 0;
        }

        if(Physics.Raycast(castPos[castInt].transform.position, Vector3.down, out hit, 999f, layerMask, QueryTriggerInteraction.Ignore));
        {
                if(hit.collider.gameObject.tag == ("Hole"))
                {
                    isHole[castInt] = true;
                }
                else
                {   
                    isHole[castInt] = false;
                }
        }

        if(isHole[0] && isHole[1] && isHole[2] && isHole[3])
        {
            Invoke("Fall", fallDelay);
        }
        else
        {
            CancelInvoke("Fall");
            //CancelFall();
        }

    }
}
