using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CityBuilderScriptableObjects", menuName = "ScriptableObjects/CityBuilderScriptableObject")]
public class PlacedObjectInfo : ScriptableObject
{
    public enum Direction { Down, Left, Up, Right };

    public string objName;
    public Transform prefab;
    public Transform visual;
    public int width;
    public int height;

    public List<Vector2Int> GetGridPosition(Vector2Int pointerPosition, Direction dir)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        if (dir == Direction.Up || dir == Direction.Down)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    gridPositionList.Add(pointerPosition + new Vector2Int(x, z));
                }
            }
        }
        else if(dir == Direction.Right || dir == Direction.Left)
        {
            for (int x = 0; x < height; x++)
            {
                for (int z = 0; z < width; z++)
                {
                    gridPositionList.Add(pointerPosition + new Vector2Int(x, z));
                }
            }
        }
        return gridPositionList;
    }

    public static Direction NextDirection(Direction dir)
    {
        if (dir == Direction.Down)
            dir = Direction.Left;
        else if (dir == Direction.Left)
            dir = Direction.Up;
        else if (dir == Direction.Up)
            dir = Direction.Right;
        else if (dir == Direction.Right)
            dir = Direction.Down;

        return dir;
    }

    public int GetRotationAngle(Direction dir)
    {
        int rotation = 0;
        if (dir == Direction.Down)
            rotation = 0;
        else if (dir == Direction.Left)
            rotation = 90;
        else if (dir == Direction.Up)
            rotation = 180;
        else if (dir == Direction.Right)
            rotation = 270;

        return rotation;
    }

    public Vector2Int GetPaddedPosition(Direction dir)
    {
        Vector2Int padding = new Vector2Int(0, 0);
        if (dir == Direction.Down)
            padding = new Vector2Int(0, 0);
        else if (dir == Direction.Left)
            padding = new Vector2Int(0, width);
        else if (dir == Direction.Up)
            padding = new Vector2Int(width, height);
        else if (dir == Direction.Right)
            padding = new Vector2Int(height, 0);

        return padding;
    }
}
