using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Wander,
    Find,
}

public class StateMachine : MonoBehaviour
{
    public State state;
    public float findDistance = 5f;

    private Waypoints waypoints = null;
    private GameObject objects;

    private void Start()
    {
        state = State.Wander;    
    }

    private IEnumerator WanderState()
    {
        waypoints.isAIMoving = true;
        
        while (state == State.Wander)
        {
            float distance = Vector3.Distance(transform.position, objects.transform.position); //finds distance to object
            if (distance <findDistance)
            {
                state = State.Find;
            }
            yield return null;
        }
        NextState();
    }

    private IEnumerator FindState()
    {
        while (state == State.Find)
        {
            //object becomes target
            float distance = Vector3.Distance(transform.position, objects.transform.position);
            
            if (distance <waypoints.speed *Time.deltaTime)
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
            case State.Find:
                StartCoroutine(FindState());
                break;
            default:
                break;
        }
    }
}
