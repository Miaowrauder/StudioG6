using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

public class iceCutter : MonoBehaviour
{
    public Vector2[] passedPoints;
    private Vector3[] actualPoints;
    public int startPoint;
    public GameObject debugLoopIndicator;
    public bool triggerCut;
    public Material holeMat;
    public GameObject rinkFloor;

    ProBuilderMesh summonedMesh;
    // Start is called before the first frame update
    void Start()
    {   
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

    private void FormCutShape()
    {
        var temp = new GameObject();
        temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y-5, temp.transform.position.z); // move it down so it fully interesects ice

        summonedMesh = temp.gameObject.AddComponent<ProBuilderMesh>();
        temp.GetComponent<MeshRenderer>().material = holeMat; // temp for testing visibility
        MeshCollider tempColl = temp.gameObject.AddComponent<MeshCollider>();
        
        temp.tag = ("Hole");

        for(int i = 0; i < actualPoints.Length; i++) //size array to match usable (not 0,0) chain of points
        {
            if((actualPoints[i].x != 0) || (actualPoints[i].z != 0))
            {
                Debug.Log(actualPoints[i]);
            }
            else
            {
                System.Array.Resize(ref actualPoints, i);
                break;
            } 
        }

        Debug.Log(actualPoints);
        summonedMesh.CreateShapeFromPolygon(actualPoints, 7.1f, true); //creates the mesh

        Debug.Log("Produced a shape with " + actualPoints.Length + " points");
        
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
