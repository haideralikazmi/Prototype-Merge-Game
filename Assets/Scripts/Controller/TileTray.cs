
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Grid;
using System.Linq;
using DG.Tweening;
using System;

namespace Tray
{
    
    /*
        The tile tray class is repsonsible for generating a new random stack of tiles.
        The tray carries the stack of tiles to the desired position via user touch input and informs the grid system to place the tile stack once
        the user lets go of the touch controls
    */

    public class TileTray : MonoBehaviour, ITileTray
    {
        [SerializeField] private GameObject trayObject;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Tile tile;
        [SerializeField] private float tileOffset = 1f;
        [SerializeField] private int maxTileCount = 5;
        [SerializeField] private float dragSpeed = 1.0f;

        private bool isDragging = false;
        private Vector3 offset;
        private Stack<Tile> tileStack = new Stack<Tile>();
        private Vector3 originalTrayPosition;
        
        public IGrid Grid { get; set; }


        private void Start()
        {
            StoreOriginalTrayPosition();
            GenerateTileStack();
        }

        private void Update()
        {
            OnTouchInput();
        }

        private void GenerateTileStack()
        {
            var position = trayObject.transform.position;
            var randomTileCount = Random.Range(2, maxTileCount);
            randomTileCount = Mathf.Abs(randomTileCount);

            for (int i = 0; i < randomTileCount; i++)
            {
                var randomNumber = Random.Range(1, 4);
                var tileNumber = Mathf.Abs(randomNumber);
                var newTile = Instantiate(tile, position, Quaternion.identity, trayObject.transform);
                newTile.InitializeTile(tileNumber);
                position = newTile.GetTilePosition();
                tileStack.Push(newTile);
                position += new Vector3(0, tileOffset, 0);
            }
        }

        private void OnTouchInput()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (IsTouchingTray(touch.position))
                        {
                            offset = transform.position - mainCamera.ScreenToWorldPoint(touch.position);
                            isDragging = true;
                        }
                        break;

                    case TouchPhase.Moved:
                        if (isDragging)
                        {
                            var targetPosition = mainCamera.ScreenToWorldPoint(touch.position) + offset;
                            targetPosition.y = transform.position.y;
                            transform.position = Vector3.Lerp(transform.position, targetPosition, dragSpeed * Time.deltaTime);
                        }
                        break;

                    case TouchPhase.Ended:
                        OnTrayReleased();
                        isDragging = false;
                        break;
                }
            }
        }

        private bool IsTouchingTray(Vector2 touchPosition)
        {
            var hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(touchPosition), -Vector3.down);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log("Collider is not null");
                return true;
            }
            Debug.Log("Collider is null");
            return true;
        }

        private void OnTrayReleased()
        {
            var tileBlock = tileStack;
            tileBlock.Reverse();
            var tileDropped = Grid.OnTrayReleased(transform.position, tileBlock);
            ResetTrayPosition(() =>
            {
                if (!tileDropped) return;
                ClearTray();
                GenerateTileStack();
            });
        }

        private void ClearTray()
        {
            tileStack.Clear();
        }

        private void ResetTrayPosition(Action callback)
        {
            transform.DOMove(originalTrayPosition, 0.35f, false).OnStepComplete(() => callback.Invoke());
        }

        private void StoreOriginalTrayPosition()
        {
            originalTrayPosition = transform.position;
        }

        void ITileTray.GenerateTray()
        {
            GenerateTileStack();
        }
    }
}