using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Wander,
    Find,
    Collect,
}

public class StateMachine : MonoBehaviour
{
    public State state;
    public float findDistance = 5f;

    public Waypoints waypoints;
    private GameObject objects;

    public Transform closestObject;
    public float objectDistance;
    public float wayPointDistance;
    public float collectRange;

    private void Start()
    {
        waypoints = GetComponent<Waypoints>();
        NextState();
    }

    private IEnumerator WanderState()
    {
        waypoints.isAIMoving = true;
        Waypoints.GetObjects();

        while (state == State.Wander)
        {
            waypoints.isAIMoving = true;
            
            closestObject = null; //the closest collection object
            objectDistance = 100; //the distance to the object
            collectRange = 5; //the range which we can then collect the object within
            wayPointDistance = Vector3.Distance(Waypoints.currentGoal.transform.position, transform.position); //distance from the waypoint
            float distanceCollect;

            //search through all items in colObjects
            foreach (var item in Waypoints.colObjects)
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
            if (wayPointDistance < objectDistance) //if the collection object is further than the waypoints
            {
                closestObject = Waypoints.currentGoal.transform; //set as the current goal
                Waypoints.target = closestObject; //set target 
            }
            if (objectDistance < wayPointDistance) //if the object is closer than the waypoints
            {
                state = State.Collect; //go collect
            }
            yield return null;
        }
        NextState();
    }

    private IEnumerator CollectState()
    {
        while (state == State.Collect)
        {
            Waypoints.target = closestObject; //the target is the closest collection object

            if (closestObject == null) //if there is no collection object (aka has been collected, as they are then deleted)
            {
                state = State.Wander; //go back to wander
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
