using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Crate : SizeObject
{
    public GameObject crateObj;
    private float oldGravity;
    private bool isGrabbed;
    private List<SizeObject> stackedCrates;
    private List<SizeObject> boundCrates;
   
    // box position - player position when grabbed
    private Vector2 grabLocalPos;

    void Start()
    {
        stackedCrates = new List<SizeObject>();     

        size = (int)Math.Round(this.transform.localScale.x);
    }

    /*private void Update()
    {
        if (isGrabbed && WouldCollide(transform.position, GetComponent<BoxCollider2D>().size))
        {
            Debug.Log("yeeeeeeeet");
        }
    }*/

    //called when a player grabs this crate
    public override void EnterGrabbedState(Vector2 offset)
    {
        isGrabbed = true;
        oldGravity = GetComponent<Rigidbody2D>().gravityScale;
        //keep block from falling
        //GetComponent<Rigidbody2D>().gravityScale = 0;
        
        //keep block from sliding
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        transform.position += (Vector3)offset;
        LayerMask layerMask = LayerMask.GetMask("Default");

        Vector3 halfExtents = GetComponent<Renderer>().bounds.extents;
        bool isColliding = Physics.CheckBox(transform.position, halfExtents, transform.rotation, layerMask);
        // don't grab box if not enough room
        if (isColliding)
        {
            Debug.Log("box would collide");
            transform.position -= (Vector3)offset;
        }

        boundCrates = new List<SizeObject>();
        boundCrates.AddRange(stackedCrates);
        foreach (SizeObject crate in boundCrates)
        {
            crate.EnterGrabbedState(offset);
            if (crate is Crate)
            {
                FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
                joint.connectedBody = crate.GetComponent<Rigidbody2D>();
            }

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
        
        foreach (SizeObject crate in boundCrates)
        {
            if (crate is Crate crate1)
                crate1.ExitGrabbedState();
        }
    }

    public override void ThrowAll(Vector2 force)
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().AddForce(force);

        foreach (SizeObject crate in boundCrates)
        {
            crate.ThrowAll(force);
        }
    }
    
    private bool WouldCollide(Vector2 center, Vector2 size)
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(
            center,
            size,
            0,
            Vector2.zero,
            0,
            ~(1 << LayerMask.NameToLayer("player"))
        );
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject != gameObject)
            {
                Debug.Log(hit.collider.gameObject.name);
                return true;
            }
        }

        return false;
    }

    public bool CanMoveToBase(float y)
    {
        Vector2 pos = GetPositionWithBase(y);
        return !WouldCollide(pos, transform.TransformVector(GetComponent<BoxCollider2D>().size));
    }

    private Vector2 GetPositionWithBase(float y)
    {
        Vector2 pos = new Vector2(transform.position.x, y + GetBoxColliderHeight(gameObject) / 2);
        return pos;
    }

    public Vector2 GetPositionOffsetToTrackPlayer()
    {
        Vector2 pos = transform.parent.position +
                      new Vector3(0, (GetBoxColliderHeight(gameObject) + GetBoxColliderHeight(transform.parent.gameObject))/2, 0);
        return (Vector3) pos - transform.position;
    }

    public override int GetMaxSize()
    {
        return 3;
    }

    public override int GetMinSize()
    {
        return 1;
    }
    
    public override void ResizeBy(int sizeDiff)
    {
        float scale = (float)(size + sizeDiff) / (size);
        bool resized = false;
        if (((SizeObject)this).CanResizeBy(sizeDiff))
        {
            size += sizeDiff;
            transform.localScale *= scale;
            resized = true;
        }

        if (isGrabbed)
        {
            foreach (SizeObject crate in boundCrates)
            {
                if (resized)
                {
                    crate.transform.localScale /= scale;
                }

                crate.ResizeBy(sizeDiff);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<SizeObject>() != null && other.contacts[0].normal.y < -0.9)
        {
            stackedCrates.Add(other.gameObject.GetComponent<SizeObject>());
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<SizeObject>() != null)
        {
            stackedCrates.Remove(other.gameObject.GetComponent<SizeObject>());
        }
    }
    
    public List<Crate> GetAllCrates()
    {
        List<Crate> allCrates = new List<Crate>();
        allCrates.Add(this);
        foreach (SizeObject crate in stackedCrates)
        {
            if (crate is Crate) 
                allCrates.AddRange(((Crate)crate).GetAllCrates());
        }   
        return allCrates;
    }

    public override void JumpAll(Vector2 velocity)
    {
        Debug.Log(GetComponent<Rigidbody2D>().velocity);
        GetComponent<Rigidbody2D>().velocity += velocity;
        foreach (SizeObject crate in boundCrates)
        {
            crate.JumpAll(velocity);
        }
    }
}
