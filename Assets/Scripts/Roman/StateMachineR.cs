using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateR
{
    Wander,
    Find,
    Collect,
}

public class StateMachineR : MonoBehaviour
{
    public State state;
    public float findDistance = 5f;

    public WaypointsR waypointsR;
    private GameObject objects;

    public Transform closestObject;
    public float objectDistance;
    public float wayPointDistance;
    public float collectRange;

    private void Start()
    {
        waypointsR = GetComponent<WaypointsR>();
        NextState();
    }

    private IEnumerator WanderState()
    {
        Debug.Log("Wander state R");
        waypointsR.isAIMoving = true;
        WaypointsR.GetObjects();

        while (state == State.Wander)
        {
            waypointsR.isAIMoving = true;
            
            closestObject = null;
            objectDistance = 100;
            collectRange = 5;
            Debug.Log(WaypointsR.currentGoal);
            wayPointDistance = Vector3.Distance(WaypointsR.currentGoal.transform.position, transform.position);
            float distanceCollect;

            //search through all items in colObjects
            foreach (var item in WaypointsR.colObjects)
            {
                //check how far that object is
                distanceCollect = Vector3.Distance(item.transform.position, transform.position);
                //if we dont have an object or this item is closer than the last item 
                if (closestObject == null || distanceCollect < objectDistance)
                {
                    //set as new item
                    closestObject = item;
                    objectDistance = distanceCollect;
                }
            }
            if (wayPointDistance < objectDistance)
            {
                closestObject = WaypointsR.currentGoal.transform;
                WaypointsR.target = closestObject;
            }
            if (objectDistance < wayPointDistance)
            {
                state = State.Collect;
            }
            yield return null;
        }
        NextState();
    }

    private IEnumerator CollectState()
    {
        Debug.Log("Collecting");
        while (state == State.Collect)
        {
            WaypointsR.target = closestObject;

           // waypoints.destination = Waypoints.target.position; //sets the destination to the target's position
                                                               //waypoints.agent.destination = waypoints.target.position; //moves the agent towards the target
            if (closestObject == null)
            {
                state = State.Wander;
            }
            yield return null;
        }
     
        NextState();
    }  

    void NextState()
    {
        switch (state)
        {
            case State.Wander:
                StartCoroutine(WanderState());
                break;
            case State.Collect:
                StartCoroutine(CollectState());
                break;
            default:
                break;
        }
    }
}
