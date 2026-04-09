using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float moveSpeed, fallDelay, maxSpeed;
    public float checkSpeed, diagMaxSpeed;
    public bool canMove;


    public bool isTrailing; 
    private bool isFalling;
    private bool trailReset; //tracks when trailing for later uses, triggers trail reset
    private int currentPoint; //point in trail array
    public float trailPointDelay;
    public GameObject tempTrailMarker; //currently visible for debug, small colliders to detect when player overlaps trail, trigger cut
    private GameObject[] createdMarkers = new GameObject[99];
    private Rigidbody rb;
    public Vector2[] trailPoint; //saved trail points
    private iceCutter myCutter;
    public int passedStartPoint;
    private int castInt;
    public Transform[] castPos;
    public LayerMask layerMask;
    private bool[] isHole = new bool[4];
    public GameObject cutTrail;
    private GameObject spawnedCutTrail;
    public GameObject spriteAndCombo;
    private playerCombo plCombo;

    private int movingDirection, lastMovingDirection; // 0 up, 1 right, 2 down, 3 left
    public int movementState; //-1 = decelerating, 0 = steady pace , 1 = accelerating;
    private int flipflop; //for alternating velocity checks

    private float[] vel = new float[2]; //holds last frames velocity, and this frames


    RaycastHit hit;
                            
    // Start is called before the first frame update
    void Start()
    {
        diagMaxSpeed = maxSpeed * 0.68f;
        rb = GetComponent<Rigidbody>();
        myCutter = GetComponent<iceCutter>();
        plCombo = spriteAndCombo.GetComponent<playerCombo>();
        trailPoint = new Vector2[98];
        trailReset = true;
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
        if(Input.GetKeyDown(KeyCode.Mouse1) && (plCombo.isTricking == false))
        {
            isTrailing = true;

            if(trailReset)
            {
                currentPoint = 0;
                ClearTrailPoints();
                trailReset = false;
                InvokeRepeating("TrailGen",Time.deltaTime, trailPointDelay);

                Vector3 trailPos = new Vector3(this.transform.position.x, this.transform.position.y-3f, this.transform.position.z);
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

        if(canMove && !isFalling)
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

        if((inputs.x != 0f) && (inputs.y != 0f)) //are we detecting double inputs, i.e moving diagonally
        {
            movement *= 0.68f; //nearly eliminates diagonal speedup
        }

        rb.AddForce(movement * (moveSpeed + (25 - rb.velocity.magnitude)), ForceMode.Impulse);

        if((inputs == new Vector2(1,1))||(inputs == new Vector2(-1,1))||(inputs == new Vector2(1,1))||(inputs == new Vector2(-1,-1))) //are we detecting double inputs, i.e moving diagonally
        {
            checkSpeed = diagMaxSpeed;
        }
        else
        {
            checkSpeed = maxSpeed;
        }

        VelCheck();
    }

    private void VelCheck() //checks velocity again slightly later to ascertain whether speed up or speed down
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
        isFalling = true;
        rb.drag = 2;
        canMove = false;
        rb.velocity = new Vector3(rb.velocity.x, -25f, rb.velocity.z);
        gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
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
        }

    }
}
