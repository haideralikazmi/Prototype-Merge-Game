
using Tray;
using UnityEngine;
using System.Collections.Generic;

namespace Grid{

    public class GridSystem : MonoBehaviour, IGrid
    {
        [SerializeField] private Tile baseTile;
        [SerializeField] private int length;
        [SerializeField] private Cell cell;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameObject gridParent;
        [SerializeField] private float offset = 0.75f;
        
        private Cell[,] grid;
        private int width = 5;
        private float rowDistance = 1.3f;
        private float columnDistance = 1.1f;

        public ITileTray TileTray {get;set;}

        public void GenerateGrid()
        {
            grid = new Cell[width, length];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    var xPos = 0f;
                    var yPos = 0f;

                    if (j % 2 == 0)
                    {
                        xPos = i * rowDistance;
                        yPos = j * columnDistance;
                    }
                    else
                    {
                        xPos = i * rowDistance + rowDistance / 2;
                        yPos = j * columnDistance;
                    }

                    var position = new Vector3(xPos, 0, yPos);

                    cell = new Cell();
                    cell.SetXCoordinate(i);
                    cell.SetYCoordinate(j);
                    cell.SetCellPosition(position);
                    grid[i, j] = cell;

                    var tile = Instantiate(baseTile, position, Quaternion.identity, gridParent.transform);
                    tile.InitializeTile();
                    cell.AddTile(tile);
                }
            }
        }

        private void Start()
        {
            GenerateGrid();
        }

        bool IGrid.OnTrayReleased(Vector3 trayPosition, Stack<Tile> block)
        {
            var tileStack = block;
            var closestCellIndex = GetClosestCell(trayPosition);
            var closestCell = grid[closestCellIndex.x, closestCellIndex.y];
            if (closestCell.CanAddNewTile())
            {
                var offsetY = 0.23f;
                var position = closestCell.GetCellPosition();

                while (tileStack.Count > 0)
                {
                    var tile = tileStack.Pop();
                    closestCell.AddTile(tile);
                    position.y += offsetY;
                    tile.MoveToPosition(position);
                    tile.ReparentObject(gridParent.transform);
                }
                return true;
            }
            return false;
        }

        private Vector2Int GetClosestCell(Vector3 trayPosition)
        {
            float minDistance = Mathf.Infinity;
            var closestIndex = new Vector2Int(-1, -1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < length; y++)
                {
                    var cell = grid[x, y];
                    var cellPosition = cell.GetCellPosition();
                    var distance = Vector3.Distance(cellPosition, trayPosition);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestIndex = new Vector2Int(x, y);
                    }
                }
            }

            return closestIndex;
        }
    }
}