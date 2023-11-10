using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem current;

    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;
    public bool button = true;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();
    public List<Building> buildings;

    private Building temp;
    private Vector3 previousPos;
    private BoundsInt previousArea;

    #region Unity Methods

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        string tilePath = @"Tiles/";
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "white"));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
        buildings = new List<Building>();
    }

    private void Update()
    {
        if (!temp) { return; }

        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject(0)) { return; }
            if (!temp.Placed)
            {
                Vector2 touchpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = gridLayout.LocalToCell(touchpos);

                if (previousPos != cellPos)
                {
                    temp.gameObject.transform.position = gridLayout.CellToLocal(cellPos);
                    previousPos = cellPos;
                    FollowBuilding();
                }

            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (temp.CanBePlaced())
            {
                temp.Place();
                buildings.Add(temp);
                temp = null;
                button = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearArea();
            Destroy(temp.gameObject);
            button = true;
        }
    }

    #endregion

    #region TileManagement

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    private static void SetTilesBlock(BoundsInt area, Tilemap tilemap, TileType type)
    {
        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tileArray = new TileBase[size];
        FillTiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);
    }

    private static void FillTiles(TileBase[] array, TileType type)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = tileBases[type];
        }
    }

    #endregion

    #region Building Placement

    public void InitializeWithBuilding(GameObject building)
    {
        if(button)
        {
            foreach (var b in buildings)
            {
                Debug.Log(b.area);
            }
            temp = Instantiate(building, Vector3.zero, Quaternion.identity).GetComponent<Building>();
            FollowBuilding();
            button = false;
        }
    }

    private void ClearArea()
    {
        SetTilesBlock(previousArea, TempTilemap, TileType.Empty);
    }

    private void FollowBuilding()
    {
        ClearArea();

        temp.area.position = gridLayout.LocalToCell(temp.gameObject.transform.position);
        BoundsInt buildingArea = temp.area;

        TileBase[] baseArray = GetTilesBlock(buildingArea, MainTilemap);

        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] != tileBases[TileType.White])
            {
                SetTilesBlock(buildingArea, TempTilemap, TileType.Red);
                previousArea = buildingArea;
                return;
            }
        }
        SetTilesBlock(buildingArea, TempTilemap, TileType.Green);
        previousArea = buildingArea;
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
        foreach (var b in baseArray) 
        {
            if (b != tileBases[TileType.White])
            {
                Debug.Log("Cannot place here");
                return false;
            }
        }

        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TempTilemap, TileType.Empty);
        SetTilesBlock(area, MainTilemap, TileType.Green);
    }

    #endregion

    public enum TileType
    {
        Empty,
        White,
        Red,
        Green
    }
}
