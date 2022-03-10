using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseDoubleClick : MonoBehaviour
{
    void OnGUI()
    {
        Event e = Event.current;
        if (e.isMouse)
        {
            //Debug.Log("Mouse clicks: " + e.clickCount);
            if (e.clickCount == 2)
                GridBuildingSystem.Instance.DoubleClickActivate();
        }
    }
}
