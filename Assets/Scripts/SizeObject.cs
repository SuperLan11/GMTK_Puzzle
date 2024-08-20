using UnityEngine;

public abstract class SizeObject : MonoBehaviour
{
    public abstract int GetMaxSize();
    public abstract int GetMinSize();

    public virtual void ThrowAll(Vector2 force)
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().AddForce(force * GetComponent<Rigidbody2D>().mass);
    }

    public int size;

    public bool CanExpand()
    {
        return size < GetMaxSize();
    }
    
    public bool CanShrink()
    {
        return size > GetMinSize();
    }
    
    public bool CanResizeBy(int sizeDiff)
    {
        return size + sizeDiff <= GetMaxSize() && size + sizeDiff >= GetMinSize();
    }

    public abstract void ResizeBy(int sizeDiff);

    public virtual void JumpAll(Vector2 velocity)
    {
        GetComponent<Rigidbody2D>().velocity += velocity;
    }

    public virtual void EnterGrabbedState(Vector2 offset)
    {
        transform.position += (Vector3) offset;
    }

}