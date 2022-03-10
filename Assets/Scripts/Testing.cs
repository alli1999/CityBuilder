using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private GridCreator<int> grid;

    // Start is called before the first frame update
    private void Start()
    {
        //grid = new GridCreator<int>(10, 10, 10f, new Vector3(0, 0, 0), AddValue);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //grid.SetValue(GetMouseWorldPosition(), 11);
        }

        if (Input.GetMouseButton(1))
        {
            //Debug.Log(grid.GetValue(GetMouseWorldPosition()));
        }
    }

    public Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            Vector3 vec = raycastHit.point;
            return vec;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public int AddValue()
    {
        return 1;
    }
}
