using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField]
    PlacedObjectInfo objectToLoad;

    [SerializeField]
    GameObject rotateButton;

    public void LoadObject()
    {
        GridBuildingSystem.Instance.VisualizeObject(objectToLoad);
        rotateButton.SetActive(true);
    }
}
