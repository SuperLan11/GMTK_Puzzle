using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTriggerScript : MonoBehaviour
{
    public static bool crateGrabbable;
    [SerializeField] GameObject crateObj;

    // Start is called before the first frame update
    void Start()
    {
        crateGrabbable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }    

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == crateObj)        
            crateGrabbable = true;        
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject == crateObj)        
            crateGrabbable = false;                    
    }
}
