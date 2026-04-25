using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class iceCutter : MonoBehaviour
{
    public Vector2[] passedPoints;
    private Vector3[] actualPoints;
    public int startPoint;
    public bool triggerCut;
    [SerializeField] private Material holeEdgeMat, shaderMat, sheetMat, invisMat;
    [SerializeField] private float sheetDropSpeed, bakeDelay, holeYoffset;
    [SerializeField] private GameObject tinySplash;
    private AxisFollow cam;

    ProBuilderMesh[] summonedMesh = new ProBuilderMesh[4];
    GameObject[] generatedObject = new GameObject[4];
    private GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {   
        cam = FindObjectOfType<AxisFollow>();
        gameManager = FindObjectOfType<GameManager>();
        passedPoints = new Vector2[99];
    }

    // Update is called once per frame
    void Update()
    {
        if(triggerCut == true)
        {
            triggerCut = false;
            DebugHighlightLoop();
        }
    }

    void DebugHighlightLoop()
    {
        actualPoints = new Vector3[passedPoints.Length];

        for(int i = 0; i < passedPoints.Length; i++)
        {
            if(passedPoints[i] != new Vector2(0,0))
            {
                actualPoints[i] = new Vector3(passedPoints[i].x, this.transform.position.y-5f, passedPoints[i].y); //vector2 y = vector3 z
                //Instantiate(debugLoopIndicator, actualPoints[i], Quaternion.identity);
            }
            else
            {
                actualPoints[i] = actualPoints[0]; //set final point to be original for smooth close
                break;
            }
            
        }

        FormCutShape();
    }

    //bit of repetition below, but i prefer the finer control over each hole piece.
    private void GenerateHole()
    {
        generatedObject[0] = new GameObject(); //generates hole w/ flipped normals
        generatedObject[0].transform.position = new Vector3(generatedObject[0].transform.position.x, generatedObject[0].transform.position.y+1.4f+holeYoffset, generatedObject[0].transform.position.z); // move it down so it fully interesects ice
        summonedMesh[0] = generatedObject[0].gameObject.AddComponent<ProBuilderMesh>();
        generatedObject[0].GetComponent<MeshRenderer>().material = holeEdgeMat;
        generatedObject[0].tag = ("Spawned");  
    }

    private void GenerateStencil()
    {
        generatedObject[1] = new GameObject(); //generates stencil mask w/ unflipped normals
        generatedObject[1].transform.position = new Vector3(generatedObject[1].transform.position.x, generatedObject[1].transform.position.y+1.93f+holeYoffset, generatedObject[1].transform.position.z); // move it down so it fully interesects ice

        summonedMesh[1] = generatedObject[1].gameObject.AddComponent<ProBuilderMesh>();
        generatedObject[1].GetComponent<MeshRenderer>().material = shaderMat; 
        MeshCollider tempColl = generatedObject[1].gameObject.AddComponent<MeshCollider>();
        generatedObject[1].tag = ("Hole");
    }

    private void GenerateDropSheet()
    {
        generatedObject[2] = new GameObject(); //generates ice sheet w/ unflipped normals
        generatedObject[2].transform.position = new Vector3(generatedObject[2].transform.position.x, generatedObject[2].transform.position.y+2f+holeYoffset, generatedObject[2].transform.position.z); // move it down so it fully interesects ice

        summonedMesh[2] = generatedObject[2].gameObject.AddComponent<ProBuilderMesh>();
        generatedObject[2].GetComponent<MeshRenderer>().material = sheetMat;
        generatedObject[2].tag = ("Spawned"); 

        sheetDropper iceScript = generatedObject[2].AddComponent<sheetDropper>();

        iceScript.dropSpeed = sheetDropSpeed;
    }

    private void GenerateMeshBlocker()
    {
        generatedObject[3] = new GameObject(); //generates tall mesh to block navmesh baking
        generatedObject[3].transform.position = new Vector3(generatedObject[3].transform.position.x, generatedObject[3].transform.position.y, generatedObject[3].transform.position.z); // move it down so it fully interesects ice
        summonedMesh[3] = generatedObject[3].gameObject.AddComponent<ProBuilderMesh>();
        generatedObject[3].GetComponent<MeshRenderer>().material = invisMat;
        generatedObject[3].tag = ("Spawned");  
    }

    private void FormCutShape()
    {
        
        GenerateHole();

        GenerateStencil();

        GenerateDropSheet();

        GenerateMeshBlocker();

        for(int i = 0; i < actualPoints.Length; i++) //size array to match usable (not 0,0) chain of points
        {
            if((actualPoints[i].x != 0) || (actualPoints[i].z != 0))
            {
                //Debug.Log(actualPoints[i]);
            }
            else
            {
                System.Array.Resize(ref actualPoints, i);
                break;
            } 
        }

        Debug.Log(actualPoints);

        for(int i = 0; i < actualPoints.Length; i++)
        {
            GameObject temp = Instantiate(tinySplash, actualPoints[i], Quaternion.identity);
            temp.transform.Rotate(-90f, 0f, 0f);
        }

        //actually makes the meshes tangible
        summonedMesh[0].CreateShapeFromPolygon(actualPoints, 0.6f, true); //creates the hole mesh
        int faceCount = summonedMesh[0].faces.Count;

        bool success = true;
       
        while(faceCount == 0)
        {
            System.Array.Resize(ref actualPoints, actualPoints.Length-1); //remove last point of mesh

            Destroy(summonedMesh[0]);
            summonedMesh[0].CreateShapeFromPolygon(actualPoints, 0.6f, true); //recreate hole mesh to retest in next loop
            Debug.Log("Attempted Loop Shortening");
            faceCount = summonedMesh[0].faces.Count;

            if(actualPoints.Length <= 2f)
            {
                success = false;
                break;
            }
        }

        //only generates other meshes when first is verified as spawned succesfully
        if(success)
        {
            summonedMesh[1].CreateShapeFromPolygon(actualPoints, 0.1f, false); //creates the stencil mesh
            summonedMesh[2].CreateShapeFromPolygon(actualPoints, 0.3f, false);//creates the drop sheet mesh 

            summonedMesh[3].CreateShapeFromPolygon(actualPoints, 20f, false);//creates the temporary navmesh blocker mesh 
            generatedObject[3].AddComponent<NavMeshModifier>();

            Debug.Log("Produced a shape with " + actualPoints.Length + " points");

            gameManager.RebakeNavmesh(bakeDelay);

            cam.shakeLength = 0.2f;
            cam.shakeStrength = 0.3f;
            cam.TriggerShake();

        }
        else
        {
            Debug.Log("Cut Failed!");
        }
        

        
        
       // CutPiece();
    }

    private void ResetPoints()
    {
        for(int i = 0; i < actualPoints.Length; i++)
        {
            passedPoints[i] = new Vector2(0,0);
        }
        for(int i = 0; i < actualPoints.Length; i++)
        {
            actualPoints[i] = new Vector3(0,0,0);
        }
    }

    /*private void CutPiece() //theres no scripting API for the experimental probuildet features, so this script is VERY similar to someone elses as the only reference i had for the functions
    {
        //get our floor, its mesh, and its centerpoint
        var floorPbComponent = rinkFloor.GetComponent<pb_Object>();
        var floorMeshFilter = floor.GetComponent<floorMeshFilter>();
        var floorCenter = floorMeshFilter.sharedMesh.bounds.center;

        //create mesh where the floor and cut collider intersect
        var intersectionMesh = CSG.Intersect(rinkFloor, summonedMesh.gameObject);

        //create a probuilder object from the mesh
        var intersectionObject = Probuilderize(intersectionMesh);

        //cut out the interesection
        var newMesh = CSG.Subtract(rinkFloor, intersectionObject.gameObject);

        floorMeshFilter.sharedMesh = newMesh; //set floor mesh to new mesh

        //additional bits to reposition interesection to correct place
        var newCenter = floorMeshFilterMeshFilter.sharedMesh.bounds.center;
        var offset = originalCenter - newCenter;
        var indices = floorMeshFilter.sharedMesh.GetIndices(0);

        floorPbComponent.TranslateVertices(indices, offset);
        CenterPivot(sourcePB, indices);

        floorPbComponent.ToMesh();
        floorPbComponent.Refresh();
    }

    
    void MyCutPiece()
    {
        ProBuilderMesh rinkMesh = rinkFloor.GetComponent<ProBuilderMesh>();

        CSG.Subtract(rinkMesh, summonedMesh);

        rinkMesh.Rebuild();
        rinkMesh.Refresh();
    }
    */

}
