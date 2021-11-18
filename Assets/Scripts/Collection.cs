using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
    public GameObject doors;

    public int coinCount; //these check if the agent has collected enough items to unlock the door
    public int key1Count;
    public int key2Count;
    
    public bool colUnlock = false; //this allows the agent to move on after finding the door

    public void OnTriggerEnter(Collider other)
    {
        #region Collecting
        //depending on what item is being collected, add to the relevant counter and then destroy the gameobject
        if (other.gameObject.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject, 1f);
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
        //depending on the door type, unlock if the count is the correct number
        if (other.gameObject.CompareTag("Coin Door"))
        {
            if (coinCount ==4)
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
}
