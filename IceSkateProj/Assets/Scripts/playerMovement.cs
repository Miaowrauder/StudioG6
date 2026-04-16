using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float moveSpeed, maxSpeed;
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
}
