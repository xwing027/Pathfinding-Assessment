using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Waypoints : MonoBehaviour
{
    #region Variables
    public float speed = 10f;
    public GameObject[] goal;
    public GameObject currentGoal;
    public int goalIndex;

    private List<Transform> colObjects;
    
    public Transform target;
    NavMeshAgent agent;
    Vector3 destination;

    public GameObject objects;
    
    Collection collection;

    public bool isAIMoving = true;

    Transform closestObject;
    float objectDistance;
    float collectRange;
    #endregion

    void Start()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Coin");
        colObjects = new List<Transform>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp.Length >=0)
            {
                colObjects.Add(temp[i].GetComponent<Transform>());
            }          
        }
        //Debug.Log(colObjects[0]);
        //colObjects.RemoveAt(0);
        currentGoal = goal[goalIndex]; //sets the current goal to be the next in the array
        
        agent = gameObject.GetComponent<NavMeshAgent>();
        collection = GetComponent<Collection>();     
    }

    void Update()
    {
        target = currentGoal.gameObject.transform; //sets the destination target to the current goal

        if (isAIMoving == false)
        {
            return; //if not moving, exit function
        }
        if (objects != null)
        {
            if (isAIMoving)
            {
                Wander(currentGoal, speed); //if moving, go into wander state
            }
        }
    }

    void Wander(GameObject goal, float currentSpeed)
    {
        #region Waypoint Movement
        Vector3 direction = (goal.transform.position - transform.position).normalized; //finds direction to the goal
        Vector3 position = transform.position;

        float distance = Vector3.Distance(transform.position, goal.transform.position); //finds distance to the goal

        destination = target.position; //sets the destination to the target's position
        agent.destination = target.position; //moves the agent towards the target
        
        if (distance < 0.5f || collection.colUnlock) //if close to object, or the door has been unlocked
        {
            NextGoal(); //go to the next goal
        }
        #endregion

        #region Object Finding
        closestObject = null;
        objectDistance = 100;
        collectRange = 10;
        //search through all items in colObjects
        foreach (var item in colObjects)
        {
            //check how far that object is
            float distance2 = Vector3.Distance(item.transform.position, transform.position);
            //if we dont have an object or this item is closer than the last item 
            if (closestObject == null || distance2 < objectDistance)
            {
                //set as new item
                closestObject = item;
                objectDistance = distance;
            }
        }
        if (objectDistance <= collectRange)
        {
            Collect();
        }
        #endregion
    }

    void Collect()
    {
        target = closestObject;
        /* target = objects.gameObject.transform;

         Vector2 direction = (objects.transform.position - transform.position).normalized; //finds the direction to the objects
         Vector2 position = transform.position;*/

        destination = target.position; //sets the destination to the target's position
        agent.destination = target.position; //moves the agent towards the target

    }

    public void NextGoal()
    {
        goalIndex++; //increase the index, moving to the next goal in the array
        currentGoal = goal[goalIndex]; //sets current goal to the new goal

        if (goalIndex > goal.Length - 1) //if a the end of the array
        {
            return; //exit function
        }
    }
}
