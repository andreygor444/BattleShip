using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCell : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPos(int x, int y)
    {
        transform.position = new Vector3(x, y, 0);
    }

}
