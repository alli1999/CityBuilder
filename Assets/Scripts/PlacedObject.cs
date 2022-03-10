using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    private PlacedObjectInfo placedObjectInfo;
    private Vector2Int origin;
    private PlacedObjectInfo.Direction direction;

    public static PlacedObject Create(PlacedObjectInfo objectInfo, Vector3 worldPosition, Vector2Int origin, PlacedObjectInfo.Direction direction)
    {
        Transform pos = Instantiate(objectInfo.prefab, worldPosition, Quaternion.Euler(0, objectInfo.GetRotationAngle(direction), 0));
        pos.gameObject.tag = "City";
        PlacedObject placedObject = pos.transform.GetComponent<PlacedObject>();
        placedObject.placedObjectInfo = objectInfo;
        placedObject.origin = origin;
        placedObject.direction = direction;

        return placedObject;
    }

    public void DestoryThis()
    {
        Destroy(this.gameObject);
    }

    public List<Vector2Int> GetGridPositionList()
    {
        return placedObjectInfo.GetGridPosition(origin, direction);
    }
}
