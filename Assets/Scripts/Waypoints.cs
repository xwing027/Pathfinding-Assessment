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
    public Transform target;
    NavMeshAgent agent;
    Vector3 destination;
    
    Collection collection;

    public bool isAIMoving = true;

    #endregion

    void Start()
    {
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
        if (isAIMoving)
        {
            Find(currentGoal, speed); //if moving, go into find state
        }
    }

    void Find(GameObject goal, float currentSpeed)
    {
        Vector3 direction = (goal.transform.position - transform.position).normalized; //finds direction to the goal
        Vector3 position = transform.position;

        float distance = Vector3.Distance(transform.position, goal.transform.position); //finds distance to the goal

        destination = target.position; //sets the destination to the target's position
        agent.destination = target.position; //moves the agent towards the target
        
        if (distance < 0.5f || collection.colUnlock) //if close to object, or the door has been unlocked
        {
            NextGoal(); //go to the next goal
        }
    }

    public void NextGoal()
    {
        goalIndex++; //increase the index, moving to the next goal in the array
        currentGoal = goal[goalIndex]; //sets current goal to the new goal

        if (goalIndex > goal.Length -1) //if a the end of the array
        {
            return; //exit function
        } 
    }
}
