using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatWallTrigger : MonoBehaviour
{    
    [SerializeField] GameObject ratObj;
    [SerializeField] bool isTriggerVisible;
    // Start is called before the first frame update
    void Start()
    {
        if(!isTriggerVisible)
            GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {       
        Vector3 flippedScale = ratObj.transform.localScale;
        flippedScale.x *= -1;
        ratObj.transform.localScale = flippedScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 flippedScale = ratObj.transform.localScale;
        flippedScale.x *= -1;
        ratObj.transform.localScale = flippedScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
