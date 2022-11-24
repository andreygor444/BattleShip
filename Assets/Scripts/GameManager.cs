using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private const bool FIRST = false, SECOND = true;
    public ShipCell shipCellPrefab;
    public Ship[] p1Ships, p2Ships;
    public ShipCell[,] p1Cells, p2Cells;
    private bool p1Ready, attacker;
    private int p1DestroyedShipsCnt, p2DestroyedShipsCnt;
    void Start()
    {
        p1DestroyedShipsCnt = 0;
        p2DestroyedShipsCnt = 0;
        p1Ready = false;
        attacker = FIRST;
        p1Cells = new ShipCell[10,10];
        p2Cells = new ShipCell[10,10];
        p1Ships = new Ship[10];
        p2Ships = new Ship[10];
        for (int i = 0; i < 10; ++i)
        {
            p1Ships[i] = new Ship();
            p2Ships[i] = new Ship();
        }
        for (int i = 0; i < 10; ++i)
        {
            for (int j = 0; j < 10; ++j)
            {
                p1Cells[i, j] = Instantiate(shipCellPrefab, new Vector3(), Quaternion.identity) as ShipCell;
                p1Cells[i, j].Init(i, j, FIRST, this);
                p1Cells[i, j].Open();
                p2Cells[i, j] = Instantiate(shipCellPrefab, new Vector3(), Quaternion.identity) as ShipCell;
                p2Cells[i, j].Init(i, j, SECOND, this);
            }
        }
    }

    void MakeShips(Ship[] ships, ShipCell[,] cells)
    {
        int i = 0;
        Stack<ShipCell> stack = new Stack<ShipCell>();
        foreach (var cell in cells)
        {
            if (cell.IsSelected() && cell.GetShip() == null)
            {
                if (i >= 10) break;
                Ship ship = ships[i++];
                stack.Push(cell);
                while (stack.Count > 0)
                {
                    ShipCell cell_ = stack.Pop();
                    cell_.SetShip(ship);
                    ship.AddCell(cell_);
                    int x = cell_.GetX();
                    int y = cell_.GetY();
                    if (x + 1 < 10 && cells[x + 1, y].IsSelected() && cells[x + 1, y].GetShip() == null) stack.Push(cells[x + 1, y]);
                    if (x - 1 >= 0 && cells[x - 1, y].IsSelected() && cells[x - 1, y].GetShip() == null) stack.Push(cells[x - 1, y]);
                    if (y + 1 < 10 && cells[x, y + 1].IsSelected() && cells[x, y + 1].GetShip() == null) stack.Push(cells[x, y + 1]);
                    if (y - 1 >= 0 && cells[x, y - 1].IsSelected() && cells[x, y - 1].GetShip() == null) stack.Push(cells[x, y - 1]);
                }
            }
        }
    }
    
    bool CheckShipsSetup()
    {
        var cells = p1Cells;
        var ships = p1Ships;
        if (p1Ready)
        {
            cells = p2Cells;
            ships = p2Ships;
        }
        foreach (var cell in cells)
        {
            if (cell.IsSelected())
            {
                int x = cell.GetX();
                int y = cell.GetY();
                if (x + 1 < 10 && y + 1 < 10 && cells[x + 1, y + 1].IsSelected()) return false;
                if (x + 1 < 10 && y - 1 >= 0 && cells[x + 1, y - 1].IsSelected()) return false;
                if (x - 1 >= 0 && y + 1 < 10 && cells[x - 1, y + 1].IsSelected()) return false;
                if (x - 1 >= 0 && y - 1 >= 0 && cells[x - 1, y - 1].IsSelected()) return false;
            }
        }
        MakeShips(ships, cells);
        int[] shipSizes = new int[10];
        for (int i = 0; i < 10; ++i)
        {
            shipSizes[i] = ships[i].GetCells().Count;
        }

        Array.Sort(shipSizes);
        if (shipSizes[0] == 1 && shipSizes[3] == 1 && shipSizes[4] == 2 && shipSizes[6] == 2 && shipSizes[7] == 3 &&
            shipSizes[8] == 3 && shipSizes[9] == 4) return true;
        return false;
    }

    void OpenSetupForP2()
    {
        foreach (var cell in p1Cells) cell.Close();
        foreach (var cell in p2Cells) cell.Open();
    }

    void StartGame()
    {
        foreach (var cell in p2Cells)
        {
            cell.Close();
            cell.Open();
            cell.EnableFightMode();
        }
        foreach (var cell in p1Cells)
        {
            cell.EnableFightMode();
        }
    }

    public void CheckSetup()
    {
        var cells = p1Cells;
        if (p1Ready) cells = p2Cells;
        int selectedCnt = 0;
        foreach (var cell in cells)
        {
            if (cell.IsSelected()) selectedCnt++;
        }

        if (selectedCnt == 20 && CheckShipsSetup())
        {
            if (p1Ready) StartGame();
            else
            {
                OpenSetupForP2();
                p1Ready = true;
            }
        }
    }

    private void ChangeAttacker()
    {
        attacker = !attacker;
        var attackerCells = p1Cells;
        var defenderCells = p2Cells;
        if (attacker == FIRST)
        {
            attackerCells = p2Cells;
            defenderCells = p1Cells;
        }
        foreach (var cell in attackerCells)
        {
            cell.Open();
        }
        foreach (var cell in defenderCells)
        {
            cell.Close();
        }
    }

    private void DestroyShip(Ship ship)
    {
        var cells = p1Cells;
        if (attacker == FIRST)
        {
            cells = p2Cells;
            p2DestroyedShipsCnt++;
        }
        else
        {
            p1DestroyedShipsCnt++;
        }
        foreach (var cell in ship.GetCells())
        {
            int x = cell.GetX();
            int y = cell.GetY();
            if (x + 1 < 10) cells[x + 1, y].TakeShot();
            if (x - 1 >= 0) cells[x - 1, y].TakeShot();
            if (y + 1 < 10) cells[x, y + 1].TakeShot();
            if (y - 1 >= 0) cells[x, y - 1].TakeShot();
            if (x + 1 < 10 && y + 1 < 10) cells[x + 1, y + 1].TakeShot();
            if (x + 1 < 10 && y - 1 >= 0) cells[x + 1, y - 1].TakeShot();
            if (x - 1 >= 0 && y + 1 < 10) cells[x - 1, y + 1].TakeShot();
            if (x - 1 >= 0 && y - 1 >= 0) cells[x - 1, y - 1].TakeShot();
        }

        if (p1DestroyedShipsCnt == 10 || p2DestroyedShipsCnt == 10)
        {
            Debug.Log($"Победил игрок");
            Application.Quit();
        }
    }
    
    public void HandleShot(ShipCell cell)
    {
        Ship ship = cell.GetShip();
        if (ship == null)
        {
            ChangeAttacker();
            return;
        }

        foreach (var cell_ in ship.GetCells())
        {
            if (!(cell_.IsDestroyed())) return;
        }

        DestroyShip(ship);
    }
}
