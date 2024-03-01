
using UnityEngine;
using Tray;
using Grid;

public class DependencyInjector : MonoBehaviour
{
    [SerializeField] private TileTray tileTray;
    [SerializeField] private GridSystem gridSystem;

    private void Awake()
    {
        tileTray.Grid = gridSystem;
        gridSystem.TileTray = tileTray;
    }
}