using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]

public class SlidingDoors : MonoBehaviour
{
    //fields 
    private Vector3 startingPoint;
    private Vector3 openPosition;

    [SerializeField] private Vector3 direction;
    [SerializeField] private float distance;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float waitTime = 3f;
    private float nextTimeDoorMoves;
    private bool isOpen = false;
    Coroutine running;

    NavMeshObstacle obstacle;

    public bool _isLocked = false;

    //property that is for the above field (uses it)
    public bool IsLocked
    {
        set
        {
            _isLocked = value;

            if (value) //(value == true)
            {
                if (running != null) StopCoroutine(running);
                if (obstacle != null) obstacle.carving = true;
            }
            else
            {
                if (obstacle != null) obstacle.carving = false;
            }
        }
        get
        {
            return _isLocked;
        }
    }

    private void OnValidate()
    {
        if (IsLocked) //(IsLocked == true)
        {
            if (running != null) StopCoroutine(running);
            if (obstacle != null) obstacle.carving = true;
        }
        else
        {
            if (obstacle != null) obstacle.carving = false;
        }
    }

    private void Awake()
    {
        obstacle = GetComponent<NavMeshObstacle>();
    }

    void Start()
    {
        startingPoint = transform.position;
        openPosition = transform.position + transform.rotation * (direction.normalized * distance);

        nextTimeDoorMoves = waitTime;
    }

    void Update()
    {
        OpenCloseDoor();
    }

    void OpenCloseDoor()
    {
        if (IsLocked)
        {
            if (running != null) StopCoroutine(running);
            return;
        }

        if (nextTimeDoorMoves <= Time.time) //Time.time - start of the game is 0 seconds, then keeps track for how many seconds since start of the game
        {
            nextTimeDoorMoves = Time.time + waitTime; //every 3 seconds will print open door

            Vector3 nextPosition = transform.position + (direction * distance);
            if (isOpen)
            {
                if (running != null) StopCoroutine(running);
                running = StartCoroutine(MoveDoor(startingPoint));
                isOpen = false;
            }
            else
            {
                isOpen = true;
                if (running != null) StopCoroutine(running);
                running = StartCoroutine(MoveDoor(openPosition));
            }
        }

        //wait x seconds
        //move door to open position or back to close position
    }

    IEnumerator MoveDoor(Vector3 position) //coroutine
    {
        //t in lerp can only go between 0-1 (the third value)
        //0 picks the first value: in (0,10) it would be 0. 1 picks the second and 0.5 would be halfway between the two and so on
        //Mathf.Lerp(0,10,0);

        while (Vector3.Distance(transform.position, position) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * speed);
            yield return null; //wait one frame
        }
    }
}
