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

        public void RemoveTile(Tile tile)
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
    }
}