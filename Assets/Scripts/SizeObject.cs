public interface SizeObject
{
    public int GetMaxSize();
    public int GetMinSize();

    public int size
    {
        get;
        set;
    }

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

    public void ResizeBy(int sizeDiff);
}