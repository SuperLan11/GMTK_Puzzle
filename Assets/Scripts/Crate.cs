using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Crate : MonoBehaviour, SizeObject
{
    public GameObject crateObj;
    private float oldGravity;
    private bool isGrabbed;
   
    // box position - player position when grabbed
    private Vector2 grabLocalPos;

    void Start()
    {
        size = 1;
    }

    //called when a player grabs this crate
    public void EnterGrabbedState()
    {
        isGrabbed = true;
        grabLocalPos = transform.localPosition;
        oldGravity = GetComponent<Rigidbody2D>().gravityScale;
        //keep block from falling
        GetComponent<Rigidbody2D>().gravityScale = 0;
        
        //keep block from sliding
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        // keep it from hitting things
        GetComponent<Collider2D>().enabled = false;
    }

    //called when a player releases this crate
    public void ExitGrabbedState()
    {
        isGrabbed = false;
        GetComponent<Rigidbody2D>().gravityScale = oldGravity;
        GetComponent<Collider2D>().enabled = true;
    }

    float GetBoxColliderHeight(GameObject obj)
    {
        BoxCollider2D boxCollider = obj.GetComponent<BoxCollider2D>();
        return obj.transform.TransformVector(new Vector2(0, boxCollider.size.y)).y;
    }

    void Update()
    {
        if (isGrabbed)
        {
            Vector2 pos = transform.parent.position +
                          new Vector3(0, (GetBoxColliderHeight(gameObject) + GetBoxColliderHeight(transform.parent.gameObject))/2, 0);
            transform.position = pos;
        }
    }

    public int GetMaxSize()
    {
        return 3;
    }

    public int GetMinSize()
    {
        return 1;
    }

    public int size { get; set; }
    public void ResizeBy(int sizeDiff)
    {
        size += sizeDiff;
        float scale = (float)(size) / (size - sizeDiff);
        transform.localScale *= scale;
    }
}
