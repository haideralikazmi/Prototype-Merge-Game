using UnityEngine;
using System.Collections.Generic;

namespace Grid
{
    public interface IGrid
    {
        bool OnTrayReleased(Vector3 trayPosition, Stack<Tile> bloc);
    }
}