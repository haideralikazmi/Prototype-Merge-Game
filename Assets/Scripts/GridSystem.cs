using UnityEngine;

public class GridSystem : MonoBehaviour
{

    [SerializeField] private Tile tile;
    [SerializeField] private int length;
    [SerializeField] private Cell cell;
     [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject gridParent;
    [SerializeField] private  float offset = 0.75f;

    private Cell[,] cells;
    private int width = 5;
    private float rowDistance = 1.3f;
    private float columnDistance = 1.1f;

    public void GenerateGrid()
    {
        var hexWidth = rowDistance;
        var hexHeight = columnDistance;
        var grid = new Cell[width, length];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                var xPos =0f;
                var yPos =0f; 
               
            if (j % 2 == 0)
            {
               xPos = i * rowDistance;
               yPos = j * columnDistance ;
            }
            else
            {
                xPos = i * rowDistance + rowDistance/2;
                yPos = j * columnDistance;
            }

            var position = new Vector3(xPos, 0, yPos);

                cell = new Cell();
                cell.SetXCoordinate(i);
                cell.SetYCoordinate(j);
                cell.SetCellPosition(position);
                grid[i, j] = cell;

                Instantiate(tile, position, Quaternion.identity, gridParent.transform);
                cell.AddTile(tile);
            }
        }
    }

    private void Start()
    {
         GenerateGrid();
    }
}