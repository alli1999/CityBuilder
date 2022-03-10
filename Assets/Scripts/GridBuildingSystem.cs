using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem Instance;

    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;

    [SerializeField]
    private List<PlacedObjectInfo> placedObjectList = null;

    [SerializeField]
    GameObject rotateButton;

    private PlacedObjectInfo placedObject;

    private bool doubleClick = false;

    private int width;
    private int height;
    private float cellSize;
    private GridCreator<GridObject> grid;
    private PlacedObjectInfo.Direction direction;//PlacedObjectInfo.Direction.Down;

    private float touchTime;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        width = 50;
        height = 50;
        cellSize = 5f;
        grid = new GridCreator<GridObject>(width, height, cellSize, Vector3.zero, (GridCreator<GridObject> g, int x, int z) => new GridObject(g, x, z));
        placedObject = null;// placedObjectList[0];
    }

    private void Update()
    {
        if (doubleClick && placedObject != null) //Input.GetMouseButtonDown(0)
        {
            grid.GetXZ(GetMouseWorldPosition(), out int x, out int z);
            List<Vector2Int> allBuildingPositions = placedObject.GetGridPosition(new Vector2Int(x, z), direction);

            bool canBuild = true;
            foreach(Vector2Int gridPosition in allBuildingPositions)
            {
                if((gridPosition.x >= width || gridPosition.y >= height) || !grid.GetValue(gridPosition.x, gridPosition.y).CanBuild())
                {
                    canBuild = false;
                    break;
                }
            }

            if (canBuild)
            {
                Vector2Int rotationPadded = placedObject.GetPaddedPosition(direction);
                Vector3 finalPlacementOfBuilding = grid.GetWorldPosition(x, z) + new Vector3(rotationPadded.x, 0, rotationPadded.y) * cellSize;
                PlacedObject placedObj = PlacedObject.Create(placedObject, finalPlacementOfBuilding, new Vector2Int(x, z), direction);
                //Transform pos = Instantiate(placedObject.prefab, finalPlacementOfBuilding, Quaternion.Euler(0, placedObject.GetRotationAngle(direction), 0));
                foreach(var gridPosition in allBuildingPositions)
                {
                    grid.GetValue(gridPosition.x, gridPosition.y).SetTranform(placedObj);
                }
                OnObjectPlaced?.Invoke(this, EventArgs.Empty);
                DeselectObjectType();
                rotateButton.SetActive(false);
            }
            else
            {
                Debug.Log("Can't build in this place");
            }
        }

        if (doubleClick)
            doubleClick = false;

#if UNITY_STANDALONE_WIN
        if (Input.GetMouseButtonDown(1))
        {
            GridObject gridObject = grid.GetValue(GetMouseWorldPosition());
            PlacedObject placed = gridObject.GetPlacedObject();
            if (placed != null)
            {
                placed.DestoryThis();
                List<Vector2Int> allBuildingPositions = placed.GetGridPositionList();
                foreach (var gridPosition in allBuildingPositions)
                {
                    grid.GetValue(gridPosition.x, gridPosition.y).ClearTransform();
                }
            }
        }
#endif

#if UNITY_ANDROID
        if(Input.touchCount >= 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchTime = Time.time;
            }

            /*if(touch.phase == TouchPhase.Stationary)
            {
                if(Time.time - touchTime > 2f && CityModeToExplore.Instance.isBuilding)
                {
                    Debug.Log("Holding");
                    GridObject gridObject = grid.GetValue(GetMouseWorldPosition());
                    PlacedObject placed = gridObject.GetPlacedObject();
                    if (placed != null)
                    {
                        placed.DestoryThis();
                        List<Vector2Int> allBuildingPositions = placed.GetGridPositionList();
                        foreach (var gridPosition in allBuildingPositions)
                        {
                            grid.GetValue(gridPosition.x, gridPosition.y).ClearTransform();
                        }
                    }
                }
            }*/

            if(touch.tapCount == 3)
            {
                GridObject gridObject = grid.GetValue(GetMouseWorldPosition());
                PlacedObject placed = gridObject.GetPlacedObject();
                if (placed != null)
                {
                    placed.DestoryThis();
                    List<Vector2Int> allBuildingPositions = placed.GetGridPositionList();
                    foreach (var gridPosition in allBuildingPositions)
                    {
                        grid.GetValue(gridPosition.x, gridPosition.y).ClearTransform();
                    }
                }
            }
        }
#endif

        if (Input.GetKeyDown(KeyCode.R))
        {
            direction = PlacedObjectInfo.NextDirection(direction);
            Debug.Log(direction);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            placedObject = placedObjectList[0]; RefreshSelectedObjectType();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            placedObject = placedObjectList[1]; RefreshSelectedObjectType();
        if (Input.GetKeyDown(KeyCode.Alpha3))
            placedObject = placedObjectList[2]; RefreshSelectedObjectType();
        if (Input.GetKeyDown(KeyCode.Alpha4))
            placedObject = placedObjectList[3]; RefreshSelectedObjectType();
        if (Input.GetKeyDown(KeyCode.Alpha5))
            placedObject = placedObjectList[4]; RefreshSelectedObjectType();
        if (Input.GetKeyDown(KeyCode.Alpha6))
            placedObject = placedObjectList[5]; RefreshSelectedObjectType();
        if (Input.GetKeyDown(KeyCode.Alpha7))
            placedObject = placedObjectList[6]; RefreshSelectedObjectType();

        if (Input.GetKeyDown(KeyCode.Alpha0))
            DeselectObjectType();
    }

    public class GridObject
    {
        private GridCreator<GridObject> grid;
        private int x;
        private int z;
        private PlacedObject placedObject;

        public GridObject(GridCreator<GridObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
            placedObject = null;
        }

        public void SetTranform(PlacedObject placedObject)
        {
            this.placedObject = placedObject;
            grid.TriggerGridObjectChanged(x, z);
        }

        public PlacedObject GetPlacedObject()
        {
            return placedObject;
        }

        public void ClearTransform()
        {
            placedObject = null;
            grid.TriggerGridObjectChanged(x, z);
        }

        public bool CanBuild()
        {
            return placedObject == null;
        }

        public override string ToString()
        {
            return x + ", " + z + "\n" + placedObject;
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

    public void Rotate()
    {
        direction = PlacedObjectInfo.NextDirection(direction);
        Debug.Log(direction);
    }

    public void RemoveVisual()
    {
        DeselectObjectType();
    }

    public void VisualizeObject(PlacedObjectInfo po)
    {
        placedObject = po;
        RefreshSelectedObjectType();
    }

    public void DoubleClickActivate()
    {
        doubleClick = true;
    }

    private void DeselectObjectType()
    {
        placedObject = null; RefreshSelectedObjectType();
    }

    private void RefreshSelectedObjectType()
    {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetMouseWorldSnappedPosition()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        grid.GetXZ(mousePosition, out int x, out int z);

        if (placedObject != null)
        {
            Vector2Int rotationOffset = placedObject.GetPaddedPosition(direction);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * cellSize;
            return placedObjectWorldPosition;
        }
        else
        {
            return mousePosition;
        }
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition) {
        grid.GetXZ(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public Quaternion GetPlacedObjectRotation()
    {
        if (placedObject != null)
        {
            return Quaternion.Euler(0, placedObject.GetRotationAngle(direction), 0);
        }
        else
        {
            return Quaternion.identity;
        }
    }

    public PlacedObjectInfo GetPlacedObjectType()
    {
        return placedObject;
    }
}