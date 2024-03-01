using UnityEngine;
using System.Collections.Generic;

namespace Grid
{
    /*
        The cell class represents an individual cell on the grid.
        It is resposible for maitaining the stack of tiles placed on the cell.
    */

    public class Cell : MonoBehaviour
    {
        private Vector3 cellPosition;
        private Stack<Tile> tileStack = new Stack<Tile>();

        public void AddTile(Tile tile)
        {
            tileStack.Push(tile);
        }

        public bool CanAddNewTile()
        {
            if (tileStack.Count > 3)
            {
                return false;
            }

            return true;
        }

        public void RemoveTile()
        {
            tileStack.Pop();
        }

        public void SetCellPosition(Vector3 value)
        {
            cellPosition = value;
        }

        public Vector3 GetCellPosition()
        {
            return cellPosition;
        }

          public Tile PeekTopTile()
    {
        return tileStack.Count > 0 ? tileStack.Peek() : null;
    }

    public bool HasMatchingTopTile(Cell otherCell)
    {
        Tile thisTopTile = PeekTopTile();
        Tile otherTopTile = otherCell.PeekTopTile();
        return thisTopTile != null && otherTopTile != null && thisTopTile.IsSameType(otherTopTile.HexType);
    }
}
}