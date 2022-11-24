using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShipCell : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    private const bool FIRST = false, SECOND = true;
    private int x, y;
	private GameManager gm;
    private bool player, active = false, selected = false, fightMode = false, destroyed = false;
#nullable enable
    private Ship ship = null;
#nullable disable

    public void Init(int x, int y, bool player, GameManager gm)
    {
        this.x = x;
        this.y = y;
        this.player = player;
        this.gm = gm;
		if (player == SECOND) SetPos(12.5f + x, y);
        else SetPos(x, y);
	}

    public void SetPos(float x, float y)
    {
        transform.position = new Vector3(x*0.9f - 9.29f, y*0.9f - 4.48f, -5);
    }

    private void SetColor(Color color)
    {
		this.GetComponent<Renderer>().material.SetColor("_Color", color);
    }

	public void EnableFightMode() {
		fightMode = true;
	}
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (destroyed) return;
		if (!active) return;
        if (fightMode) {
            TakeShot();
            gm.HandleShot(this);
            return;
		}
		if (selected)
        {
			SetColor(new Color(0, 0, 0, 0));
		}
		else
		{
			SetColor(new Color(100, 100, 100, 100));
		}
		selected = !selected;
		gm.CheckSetup();
    }

    public void TakeShot()
    {
        destroyed = true;
		if (ship == null) {
			SetColor(new Color(255, 255, 255, 255));
		}
		else
		{
			SetColor(new Color(220, 0, 0, 100));
		}
		active = false;
    }

	public bool GetPlayer() {
		return player;
	}

    public Ship GetShip() {
        return ship;
    }

    public void SetShip(Ship ship) {
        this.ship = ship;
    }

    public int GetX() {
        return x;
    }

    public int GetY() {
        return y;
    }

	public bool IsActive()
	{
		return active;
	}
	
	public bool IsSelected()
	{
		return selected;
	}

    public bool IsDestroyed()
	{
		return destroyed;
	}

	public void Open() {
		active = true;
	}

	public void Close() {
        if (destroyed) return;
		SetColor(new Color(0, 0, 0, 0));
		active = false;
		selected = false;
	}
}
