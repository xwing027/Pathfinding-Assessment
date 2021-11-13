using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
    public GameObject doors;
    public Waypoints waypoints;

    public int coinCount; //these check if the agent has collected enough items to unlock the door
    public int key1Count;
    public int key2Count;
    
    public bool colUnlock = false; //this allows the agent to move on after finding the door

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Waypoint"))
        {
            //Destroy(other.gameObject, 0.9f);
        }

        #region Collecting
        if (other.gameObject.CompareTag("Coin"))
        {
            coinCount++;
            //Destroy(other.gameObject, 0.5f);
        }
        if (other.gameObject.CompareTag("Key1"))
        {
            key1Count++;
            Destroy(other.gameObject, 0.5f);
        }
        if (other.gameObject.CompareTag("Key2"))
        {
            key2Count++;
            Destroy(other.gameObject, 0.5f);
        }
        #endregion

        #region Unlocking
        if (other.gameObject.CompareTag("Coin Door"))
        {
            if (coinCount ==5)
            {
                doors.GetComponent<SlidingDoors>()._isLocked = false;
                colUnlock = true;
            }
        }
        if (other.gameObject.CompareTag("Key1 Door"))
        {
            if (key1Count == 1)
            {
                doors.GetComponent<SlidingDoors>()._isLocked = false;
                colUnlock = true;
            }
        }
        if (other.gameObject.CompareTag("Key2 Door"))
        {
            if (key2Count == 1)
            {
                doors.GetComponent<SlidingDoors>()._isLocked = false;
                colUnlock = true;
            }
        }
        #endregion
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Coin Door"))
        {
            doors.GetComponent<SlidingDoors>()._isLocked = true;
        }
        if (other.gameObject.CompareTag("Key1 Door"))
        {
            doors.GetComponent<SlidingDoors>()._isLocked = true;
        }
        if (other.gameObject.CompareTag("Key2 Door"))
        {
            doors.GetComponent<SlidingDoors>()._isLocked = true;
        }
    }
}
