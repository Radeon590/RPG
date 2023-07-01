using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootWindowContainer : MonoBehaviour
{
    public static List<GameObject> NearestObj = new List<GameObject>();
    public static GameObject LookingObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*Debug.Log($"lookingObj: {LookingObj}");
        Debug.Log($"count of nearestObj: {NearestObj.Count}");*/
    }
}
