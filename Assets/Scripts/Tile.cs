using UnityEngine;

namespace Grid{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private Renderer tileRenderer;
        [SerializeField] private Transform tileTransform;
        public TileType HexType { get; private set; }

        public void InitializeTile(int type = 0)
        {
            SetAppearance(type);
        }


        public void SetAppearance(int type)
        {
            switch (type)
            {
                case (int)TileType.Red:
                    tileRenderer.material.color = Color.red;
                    break;
                case (int)TileType.Blue:
                    tileRenderer.material.color = Color.green;
                    break;
                case (int)TileType.Green:
                    tileRenderer.material.color = Color.blue;
                    break;
                default:
                    tileRenderer.material.color = Color.grey;
                    break;
            }
        }

        public Vector3 GetTilePosition()
        {
            return tileTransform.position;
        }
    }
}

public enum TileType
{
    Empty=0,
    Red,
    Green,
    Blue
}