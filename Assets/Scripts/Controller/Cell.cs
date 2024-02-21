using UnityEngine;
using System.Collections.Generic;

namespace Grid
{
    public class Cell : MonoBehaviour
    {
        private int gridX;
        private int gridY;
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

        public void SetXCoordinate(int value)
        {
            gridX = value;
        }

        public void SetYCoordinate(int value)
        {
            gridY = value;
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

    public void DestroyMatchingTilesInAdjacentCells(List<Cell> adjacentCells)
    {
        foreach (var adjacentCell in adjacentCells)
        {
            if (HasMatchingTopTile(adjacentCell))
            {
                DestroyMatchingTopTiles(adjacentCell);
                adjacentCell.DestroyMatchingTilesInAdjacentCells(adjacentCells);
            }
        }
    }

    private void DestroyMatchingTopTiles(Cell otherCell)
    {
        Tile thisTopTile = PeekTopTile();
        Tile otherTopTile = otherCell.PeekTopTile();
        if (thisTopTile.IsSameType(otherTopTile.HexType))
        {
            RemoveTile();
            otherCell.RemoveTile();
            Destroy(thisTopTile.gameObject);
            Destroy(otherTopTile.gameObject);
        }
    }
}
}