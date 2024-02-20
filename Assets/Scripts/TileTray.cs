
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;
using Grid;

namespace Tray{

    public class TileTray : MonoBehaviour, ITileTray
    {

        [SerializeField] private GameObject trayObject;
        [SerializeField] private Tile tile;
        [SerializeField] private float offset = 1f;
        [SerializeField] private int maxTileCount = 5;
        private Stack<Tile> tileStack = new Stack<Tile>();

        private void GenerateTileStack()
        {
            var position = trayObject.transform.position;
            var randomTileCount = Random.Range(2, maxTileCount);
            randomTileCount = Mathf.Abs(randomTileCount);

            for (int i = 0; i < randomTileCount; i++)
            {
                var randomNumber = Random.Range(1, 3);
                var tileNumber = Mathf.Abs(randomNumber);
                var newTile = Instantiate(tile, position, Quaternion.identity, trayObject.transform);
                newTile.InitializeTile(tileNumber);
                position = newTile.GetTilePosition();
                tileStack.Push(newTile);
                position += new Vector3(0, offset, 0);
            }

        }

        void ITileTray.GenerateTray()
        {
            GenerateTileStack();
        }

        private void Start(){
        GenerateTileStack();
        }

    }
}