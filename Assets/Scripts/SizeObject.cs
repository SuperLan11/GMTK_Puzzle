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
}