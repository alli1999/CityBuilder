using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateButton : MonoBehaviour
{
    public void RotateVisual()
    {
        GridBuildingSystem.Instance.Rotate();
    }
}
