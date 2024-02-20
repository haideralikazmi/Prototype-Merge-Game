using UnityEngine;
using System.Collections.Generic;

public class Cell : MonoBehaviour
{
    private int gridX;
    private int gridY;
    private Vector3 cellPosition;
    private List<Tile> tileBlock = new List<Tile>();

    public void AddTile(Tile tile)
    {
        tileBlock.Add(tile);
    }

    public void RemoveTile(Tile tile)
    {
        tileBlock.Remove(tile);
    }

    public void SetXCoordinate(int value){
        gridX = value;
    }

     public void SetYCoordinate(int value){
        gridY = value;
    }

     public void SetCellPosition(Vector3 value){
        cellPosition = value;
    }


}