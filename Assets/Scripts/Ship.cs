using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship
{
    private List<ShipCell> cells;

    public Ship()
    {
        cells = new List<ShipCell>();
    }

    public List<ShipCell> GetCells() {
        return cells;
    }

    public void AddCell(ShipCell cell)
    {
        cells.Add(cell);
    }

    void Clear() {
        cells.Clear();
    }
}
