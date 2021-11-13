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
    public float collectRange;

    private void Start()
    {
        waypoints = GetComponent<Waypoints>();
        NextState();   
    }

    private IEnumerator WanderState()
    {
        waypoints.isAIMoving = true;
        
        while (state == State.Wander)
        {
            closestObject = null;
            objectDistance = 100;
            collectRange = 5;
            //search through all items in colObjects
            foreach (var item in waypoints.colObjects)
            {
                //check how far that object is
                float distance2 = Vector3.Distance(item.transform.position, transform.position);
                //if we dont have an object or this item is closer than the last item 
                if (closestObject == null || distance2 < objectDistance)
                {
                    //set as new item
                    closestObject = item;
                    objectDistance = waypoints.distance;
                }
            }
            if (objectDistance <= collectRange)
            {
                state = State.Collect;
            }
            yield return null;
        }
        NextState();
    }

    private IEnumerator CollectState()
    {
        waypoints.target = closestObject;

        waypoints.destination = waypoints.target.position; //sets the destination to the target's position
        waypoints.agent.destination = waypoints.target.position; //moves the agent towards the target
        yield return null;
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
