using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Crate : MonoBehaviour, SizeObject
{
    public GameObject crateObj;
    private float oldGravity;
    private bool isGrabbed;
    private List<Crate> stackedCrates;
    private List<Crate> boundCrates;
   
    // box position - player position when grabbed
    private Vector2 grabLocalPos;

    void Start()
    {
        stackedCrates = new List<Crate>();     

        size = (int)this.transform.localScale.x;
    }

    //called when a player grabs this crate
    public void EnterGrabbedState(Vector2 offset)
    {
        isGrabbed = true;
        oldGravity = GetComponent<Rigidbody2D>().gravityScale;
        //keep block from falling
        //GetComponent<Rigidbody2D>().gravityScale = 0;
        
        //keep block from sliding
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        
        transform.position += (Vector3) offset;
        
        boundCrates = new List<Crate>();
        boundCrates.AddRange(stackedCrates);
        foreach (Crate crate in boundCrates)
        {
            crate.EnterGrabbedState(offset);
            FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
            joint.connectedBody = crate.GetComponent<Rigidbody2D>();
            crate.transform.parent = transform;
        }
    }

    //called when a player releases this crate
    public void ExitGrabbedState()
    {
        isGrabbed = false;
        GetComponent<Rigidbody2D>().gravityScale = oldGravity;
        transform.parent = null;
        
        foreach (FixedJoint2D joint in GetComponents<FixedJoint2D>())
        {
            Destroy(joint);
        }
        
        foreach (Crate crate in boundCrates)
        {
            crate.ExitGrabbedState();
        }
    }

    public void ThrowAll(Vector2 force)
    {
        GetComponent<Rigidbody2D>().AddForce(force);

        foreach (Crate crate in boundCrates)
        {
            crate.ThrowAll(force);
        }
    }

    float GetBoxColliderHeight(GameObject obj)
    {
        BoxCollider2D boxCollider = obj.GetComponent<BoxCollider2D>();
        return obj.transform.TransformVector(new Vector2(0, boxCollider.size.y)).y;
    }

    public Vector2 GetPositionOffsetToTrackPlayer()
    {
        Vector2 pos = transform.parent.position +
                      new Vector3(0, (GetBoxColliderHeight(gameObject) + GetBoxColliderHeight(transform.parent.gameObject))/2, 0);
        return (Vector3) pos - transform.position;
    }

    public int GetMaxSize()
    {
        return 3;
    }

    public int GetMinSize()
    {
        return 1;
    }
    
    [SerializeField] private int internalSize;
    public int size
    {
        get => internalSize;
        set => internalSize = value;
    }

    public void ResizeBy(int sizeDiff)
    {
        size += sizeDiff;
        float scale = (float)(size) / (size - sizeDiff);
        transform.localScale *= scale;
        if (isGrabbed)
        {
            /*foreach (Crate crate in boundCrates)
            {
                crate.ResizeBy(sizeDiff);
            }*/
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Crate>() != null && other.contacts[0].normal.y < -0.9)
        {
            stackedCrates.Add(other.gameObject.GetComponent<Crate>());
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Crate>() != null)
        {
            stackedCrates.Remove(other.gameObject.GetComponent<Crate>());
        }
    }
    
    public List<Crate> GetAllCrates()
    {
        List<Crate> allCrates = new List<Crate>();
        allCrates.Add(this);
        foreach (Crate crate in stackedCrates)
        {
            allCrates.AddRange(crate.GetAllCrates());
        }
        return allCrates;
    }

    public void JumpAll(Vector2 velocity)
    {
        GetComponent<Rigidbody2D>().velocity += velocity;
        foreach (Crate crate in boundCrates)
        {
            crate.JumpAll(velocity);
        }
    }
}
