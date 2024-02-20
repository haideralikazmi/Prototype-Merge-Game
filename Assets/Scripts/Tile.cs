using UnityEngine;

public class Tile : MonoBehaviour
{ 
    public HexType hexType;
    
    public virtual void SetAppearance(Color color)
    {
        
    }
}

public enum HexType
{
    Empty,
    Red,
    Green,
    Blue
}