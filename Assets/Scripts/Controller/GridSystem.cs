
using Tray;
using UnityEngine;
using System.Collections.Generic;

namespace Grid
{
    public class GridSystem : MonoBehaviour, IGrid
    {
        [SerializeField] private Tile baseTile;
        [SerializeField] private Cell cell;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameObject gridParent;

        private Cell[,] grid;
        private int width = 5;
        private int length = 4;
        private float offset = 0.75f;
        private float rowDistance = 1.3f;
        private float columnDistance = 1.1f;

        public ITileTray TileTray { get; set; }

        /*
            Generates a grid of hexagonal tiles and spawns them into the scene in a honeycomb pattern
        */

        private void Start()
        {
            GenerateGrid();
        }

        public void GenerateGrid()
        {
            grid = new Cell[width, length];
            var xPos = 0f;
            var yPos = 0f;

            for (int rowIndex = 0; rowIndex < width; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < length; columnIndex++)
                {
                    if (columnIndex % 2 == 0)
                    {
                        xPos = rowIndex * rowDistance;
                        yPos = columnIndex * columnDistance;
                    }
                    else
                    {
                        xPos = rowIndex * rowDistance + rowDistance / 2;
                        yPos = columnIndex * columnDistance;
                    }

                    var position = new Vector3(xPos, 0, yPos);
                    grid[rowIndex, columnIndex] = CreateNewCell(rowIndex,columnIndex,position);
                    SpawnTile(position);
                }
            }
        }

        private Cell CreateNewCell(int rowIndex, int columnIndex, Vector3 position)
        {
            cell = new Cell();
            cell.SetCellPosition(position);
            return cell;
        }

        private void SpawnTile(Vector3 position)
        {
            var tile = Instantiate(baseTile, position, Quaternion.identity, gridParent.transform);
            tile.InitializeTile();
        }


        /*
            Interface handler for grid to drop and move incoming tilestack to closest cell on the grid incase player lets go of the tilestack in hand.
            Flips the tilestack before dropping it on the grid cell
        */

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

                CheckAndDestroyMatchingTiles(closestCellIndex);

                return true;
            }
            return false;
        }

        /*
            Destroys tiles on the neighbouring cells incase they match the tile type in the newly dropped tilestack.
        */

        private void CheckAndDestroyMatchingTiles(Vector2Int cellIndex)
        {
            Vector2Int[] adjacentIndices =
            {
                new Vector2Int(cellIndex.x - 1, cellIndex.y),     
                new Vector2Int(cellIndex.x + 1, cellIndex.y),     
                new Vector2Int(cellIndex.x, cellIndex.y - 1),    
                new Vector2Int(cellIndex.x, cellIndex.y + 1)  
            };

            foreach (var adjacentIndex in adjacentIndices)
            {
                
                if (IsIndexValid(adjacentIndex))
                {
                    var adjacentCell = grid[adjacentIndex.x, adjacentIndex.y];
                    if (adjacentCell.HasMatchingTopTile(grid[cellIndex.x, cellIndex.y]))
                    {
                        DestroyMatchingTopTiles(adjacentCell, grid[cellIndex.x, cellIndex.y]);
                    }
                }
            }
        }

        private bool IsIndexValid(Vector2Int index)
        {
            return index.x >= 0 && index.x < width && index.y >= 0 && index.y < length;
        }

        private void DestroyMatchingTopTiles(Cell cell1, Cell cell2)
        {
            Tile topTile1 = cell1.PeekTopTile();
            Tile topTile2 = cell2.PeekTopTile();

            if (topTile1 != null && topTile2 != null && topTile1.IsSameType(topTile2.HexType))
            {
                cell1.RemoveTile();
                cell2.RemoveTile();
                Destroy(topTile1.gameObject);
                Destroy(topTile2.gameObject);
            }
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
