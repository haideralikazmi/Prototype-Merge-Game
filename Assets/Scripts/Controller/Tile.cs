﻿using UnityEngine;
using DG.Tweening;

namespace Grid
{

    /*
        The tile class represents a single hexagonal tile that can be placed on the grid.
        It is responsilbe for maintaining the type and location of the tile.
    */

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

        public void MoveToPosition(Vector3 newPosition)
        {
            transform.DOJump(newPosition, 0.45f, 1, 0.5f, false);
        }

        public void ReparentObject(Transform parent)
        {
            transform.SetParent(parent);
        }

        public bool IsSameType(TileType type)
        {
            if (type == HexType)
            {
                return true;
            }
            return false;

        }
    }
}



public enum TileType
{
    Empty = 0,
    Red,
    Green,
    Blue
}