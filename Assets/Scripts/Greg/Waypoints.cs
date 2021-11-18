using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Waypoints : MonoBehaviour
{
    #region Variables
    public float speed = 10f;
    public GameObject[] goal;
    public static GameObject currentGoal;
    public int goalIndex;
    public float distance;
    public State state;

    public static List<Transform> colObjects;
    
    public static Transform target;
    public NavMeshAgent agent;
    public Vector3 destination;
    
    Collection collection;

    public bool isAIMoving = true;

    Transform closestObject;
    float objectDistance;
    float collectRange;
    #endregion

    void Start()
    {
        GetObjects();

        currentGoal = goal[goalIndex]; //sets the current goal to be the next in the array
        
        agent = gameObject.GetComponent<NavMeshAgent>(); 
        collection = GetComponent<Collection>();
        target = currentGoal.gameObject.transform; //sets the destination target to the current goal
    }
    public static void GetObjects()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Coin"); //find the tagged objects and store in a temporary array
        colObjects = new List<Transform>(); //make a list

        for (int i = 0; i < temp.Length; i++) //for each item in the temp array
        {
            if (temp.Length >= 0)
            {
                colObjects.Add(temp[i].GetComponent<Transform>()); //add to the list
            }
        }
    }
    void Update()
    {
        if (isAIMoving == false)
        {
            return; //if not moving, exit function
        }
            if (isAIMoving)
            {
                Wander(currentGoal, speed); //default wander behaviour
            }
    }


    void Wander(GameObject goal, float currentSpeed)
    {
        Vector3 direction = (goal.transform.position - transform.position).normalized; //finds direction to the goal
        Vector3 position = transform.position;

        distance = Vector3.Distance(transform.position, goal.transform.position); //finds distance to the goal

        destination = target.position; //sets the destination to the target's position
        agent.destination = target.position; //moves the agent towards the target

        if (distance < 0.8f || collection.colUnlock) //if close to object, or the door has been unlocked
        {
            NextGoal(); //go to the next goal
        }
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
