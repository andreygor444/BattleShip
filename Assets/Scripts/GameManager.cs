using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour, IPointerClickHandler
{
    public ShipCell shipCellPrefab;
    public Ship[] ships;
    public ShipCell[] cells;
    void Start()
    {
        cells = new ShipCell[1000];
        int cnt = 0;
        for (int i = 0; i < 10; ++i)
        {
            for (int j = 0; j < 10; ++j)
            {
                cells[cnt] = Instantiate(shipCellPrefab, new Vector3(), Quaternion.identity) as ShipCell;
                cells[cnt++].SetPos(i * 100, j * 100);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int x = (int) eventData.pointerCurrentRaycast.screenPosition.x, y = (int) eventData.pointerCurrentRaycast.screenPosition.y;
        var bounds = GetComponent<BoxCollider2D>().bounds.size;
        Debug.Log($"{x}, {y}, {bounds.x}, {bounds.y}");
    }
}
