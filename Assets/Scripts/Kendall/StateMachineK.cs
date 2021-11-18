using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateK
{
    Wander,
    Find,
    Collect,
}

public class StateMachineK : MonoBehaviour
{
    public State state;
    public float findDistance = 5f;

    public WaypointsK waypointsR;
    private GameObject objects;

    public Transform closestObject;
    public float objectDistance;
    public float wayPointDistance;
    public float collectRange;

    private void Start()
    {
        waypointsR = GetComponent<WaypointsK>();
        NextState();
    }

    private IEnumerator WanderState()
    {
        Debug.Log("Wander state R");
        waypointsR.isAIMoving = true;
        WaypointsK.GetObjects();

        while (state == State.Wander)
        {
            waypointsR.isAIMoving = true;
            
            closestObject = null;
            objectDistance = 100;
            collectRange = 5;
            Debug.Log(WaypointsK.currentGoal);
            wayPointDistance = Vector3.Distance(WaypointsK.currentGoal.transform.position, transform.position);
            float distanceCollect;

            //search through all items in colObjects
            foreach (var item in WaypointsK.colObjects)
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
                closestObject = WaypointsK.currentGoal.transform;
                WaypointsK.target = closestObject;
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
            WaypointsK.target = closestObject;

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
