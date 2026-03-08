using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailColliders : MonoBehaviour
{
    public int pointNumber;
    private bool triggerTwo; //stops it triggering coll on spawn

    private void OnTriggerEnter(Collider coll) 
    {
      if(coll.tag == "Player")
        {
            if(triggerTwo)
            {
                Debug.Log("closed loop!");
                coll.GetComponent<playerMovement>().passedStartPoint = pointNumber; //the point the player colliders with - makes it so we only save points from the formed loop
                coll.GetComponent<playerMovement>().PassPoints();
            }
            else if(!triggerTwo)
            {
                triggerTwo = true;
            }
        }   
    }
}
