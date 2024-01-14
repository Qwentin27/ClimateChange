using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{
    public static Grid current;

    public Vector2Int size;
    public Vector2 offset;

    private Box element;

    [Header("Interactive")]
    public Box[] elementPrefab;
    public Transform root;

    [Header("Final")]
    public Box[] backgroundPrefab;
    public Transform backRoot;

    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;

    private static readonly Dictionary<TileType, TileBase> tileBases = new();

    public readonly List<Box> boxes = new();
    private Box temp;
    private Vector3 previousPos;
    private BoundsInt previousArea;

    #region Unity Methods

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        string tilePath = @"Tiles/";
        if (!tileBases.ContainsKey(TileType.FINAL))
        {
            tileBases.Add(TileType.FINAL, null);
        }
        if (!tileBases.ContainsKey(TileType.NATURE))
        {
            tileBases.Add(TileType.NATURE, Resources.Load<TileBase>(tilePath + "white"));
        }
        if (!tileBases.ContainsKey(TileType.FACTORY))
        {
            tileBases.Add(TileType.FACTORY, Resources.Load<TileBase>(tilePath + "red"));
        }
        if (!tileBases.ContainsKey(TileType.AGRICULTURE))
        {
            tileBases.Add(TileType.AGRICULTURE, Resources.Load<TileBase>(tilePath + "green"));
        }

        NewGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!temp) { return; }

        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject(0)) { return; }
            if (!temp.Placed)
            {
                Vector2 touchpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = gridLayout.LocalToCell(new Vector2(touchpos.x, touchpos.y+1));

                if (previousPos != cellPos)
                {
                    temp.gameObject.transform.position = gridLayout.CellToLocal(cellPos);
                    previousPos = cellPos;
                    FollowBox();
                }

            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3Int positionInt = gridLayout.LocalToCell(temp.transform.position);
            BoundsInt areaTemp = temp.area;
            areaTemp.position = new Vector3Int(positionInt.x - 1, positionInt.y - 1, positionInt.z);
            if (temp.CanBePlaced(areaTemp))
            {
                temp.Place(areaTemp);
                temp = null;
                GameManager.instance.button = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearArea();
            Destroy(temp.gameObject);
            GameManager.instance.button = true;
        }
    }
    #endregion

    #region BoardInitialization

    public void SetBlock(int x, int y, int i)
    {
        element = Instantiate(backgroundPrefab[i], backRoot);
        element.transform.position = new Vector3((x - y) * offset.x, (x + y + 2) * offset.y, 0);
        element.transform.localScale = new Vector3(3, (float)2.75, 1);
        element.area = new BoundsInt(new Vector3Int(x, y, 0), Vector3Int.one);
    }

    public void NewGame()
    {
        for (int i = 0; i < size.x + 10; i++)
        {
            for (int j = 0; j < size.y + 10; j++)
            {
                int x = i - 9;
                int y = j - 9;

                if ((i < 4 || j < 4) || (i > size.x + 5 || j > size.y + 5))
                {
                    int index = Random.Range(0, 2);
                    SetBlock(x, y, index);
                }
                else if ((i == 4 && j == 4) || (i == (size.x + 5) && j == (size.y + 5)) || (i == 4 && j == (size.y + 5)) || (i == (size.x + 5) && j == 4))
                {
                    SetBlock(x, y, 2);
                }
                else if ((i == 4) || (i == (size.x + 5)))
                {
                    SetBlock(x, y, 4);
                }
                else if ((j == 4) || (j == (size.y + 5)))
                {
                    SetBlock(x, y, 3);
                }
                else
                {
                    int index = Random.Range(0, elementPrefab.Length);
                    element = Instantiate(elementPrefab[index], root);
                    element.transform.position = new Vector3((x - y) * offset.x, (x + y + 2) * offset.y, 0);
                    element.transform.localScale = new Vector3(3, (float)2.75, 1);
                    element.area = new BoundsInt(new Vector3Int(x, y, 0), Vector3Int.one);

                    if (index == 0)
                    {
                        SetTilesBlock(new BoundsInt(new Vector3Int(x, y, 0), Vector3Int.one), MainTilemap, TileType.FACTORY);
                    }
                    else if (index <= 2)
                    {
                        SetTilesBlock(new BoundsInt(new Vector3Int(x, y, 0), Vector3Int.one), MainTilemap, TileType.AGRICULTURE);
                    }
                    else
                    {
                        if (index == 4)
                        {
                            element.index = 1;
                        }
                        SetTilesBlock(new BoundsInt(new Vector3Int(x, y, 0), Vector3Int.one), MainTilemap, TileType.NATURE);
                    }
                    boxes.Add(element);
                }
            }
        }
    }

    public void LoadGrid()
    {
        string tilePath = @"Tiles/";
        if (!tileBases.ContainsKey(TileType.FINAL))
        {
            tileBases.Add(TileType.FINAL, null);
        }
        if (!tileBases.ContainsKey(TileType.NATURE))
        {
            tileBases.Add(TileType.NATURE, Resources.Load<TileBase>(tilePath + "white"));
        }
        if (!tileBases.ContainsKey(TileType.FACTORY))
        {
            tileBases.Add(TileType.FACTORY, Resources.Load<TileBase>(tilePath + "red"));
        }
        if (!tileBases.ContainsKey(TileType.AGRICULTURE))
        {
            tileBases.Add(TileType.AGRICULTURE, Resources.Load<TileBase>(tilePath + "green"));
        }

        Box[] tiles = FindObjectsOfType<Box>();
        int i = 0;
        foreach (Box tile in tiles)
        {
            Debug.Log(i);
            if (tile.t == Box.BoxType.FACTORY)
            {
                SetTilesBlock(new BoundsInt(new Vector3Int(tile.area.position.x, tile.area.position.y, 0), Vector3Int.one), MainTilemap, TileType.FACTORY);
                if (i >= 0 && i < 81)
                {
                    boxes[i] = tile;
                    i++;
                }
            }
            else if (tile.t == Box.BoxType.AGRICULTURE)
            {
                SetTilesBlock(new BoundsInt(new Vector3Int(tile.area.position.x, tile.area.position.y, 0), Vector3Int.one), MainTilemap, TileType.AGRICULTURE);
                if (i >= 0 && i < 81)
                {
                    boxes[i] = tile;
                    i++;
                }
            }
            else if (tile.t == Box.BoxType.NATURE)
            {
                SetTilesBlock(new BoundsInt(new Vector3Int(tile.area.position.x, tile.area.position.y, 0), Vector3Int.one), MainTilemap, TileType.NATURE);
                if(i >= 0 && i < 81)
                {
                    boxes[i] = tile;
                    i++;
                }
            }
            else
            {
                SetTilesBlock(new BoundsInt(new Vector3Int(tile.area.position.x, tile.area.position.y, 0), Vector3Int.one), MainTilemap, TileType.FINAL);
            }
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
            Vector3Int pos = new(v.x, v.y, 0);
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

    public void InitializeWithBuilding(Box building)
    {
        if (GameManager.instance.button/* && (GameManager.instance.money >= building.price)*/)
        {
            temp = Instantiate(building, Vector3.zero, Quaternion.identity, root).GetComponent<Box>();
            temp.transform.localScale = new Vector3(3, (float)2.75, 1);
            FollowBox();
            GameManager.instance.button = false;
        }
    }

    private void ClearArea()
    {
        SetTilesBlock(previousArea, TempTilemap, TileType.FINAL);
    }

    private void FollowBox()
    {
        ClearArea();

        temp.area.position = gridLayout.LocalToCell(temp.gameObject.transform.position);
        temp.area.position = new Vector3Int(temp.area.position.x - 1, temp.area.position.y - 1, temp.area.position.z);
        BoundsInt buildingArea = temp.area;

        TileBase[] baseArray = GetTilesBlock(buildingArea, MainTilemap);

        if(temp.t == Box.BoxType.FACTORY)
        {
            for (int i = 0; i < baseArray.Length; i++)
            {
                if (baseArray[i] == tileBases[TileType.FINAL])
                {
                    SetTilesBlock(buildingArea, TempTilemap, TileType.FACTORY);
                    previousArea = buildingArea;
                    return;
                }
            }
        }
        if (temp.t == Box.BoxType.NATURE)
        {
            for (int i = 0; i < baseArray.Length; i++)
            {
                if (baseArray[i] != tileBases[TileType.FACTORY])
                {
                    if (baseArray[i] != tileBases[TileType.AGRICULTURE])
                    {
                        SetTilesBlock(buildingArea, TempTilemap, TileType.FACTORY);
                        previousArea = buildingArea;
                        return;
                    }
                }
            }
        }
        if (temp.t == Box.BoxType.AGRICULTURE)
        {
            for (int i = 0; i < baseArray.Length; i++)
            {
                if (baseArray[i] != tileBases[TileType.NATURE])
                {
                    SetTilesBlock(buildingArea, TempTilemap, TileType.FACTORY);
                    previousArea = buildingArea;
                    return;
                } else
                {
                    foreach(Box b in boxes)
                    {
                        if(b.area.position == buildingArea.position && b.level < 1)
                        {
                            SetTilesBlock(buildingArea, TempTilemap, TileType.FACTORY);
                            previousArea = buildingArea;
                            return;
                        }
                    }
                }
            }
        }

        SetTilesBlock(buildingArea, TempTilemap, TileType.AGRICULTURE);
        previousArea = buildingArea;
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, TempTilemap);
        foreach (var t in baseArray)
        {
            if (t != tileBases[TileType.AGRICULTURE])
            {
                return false;
            }
        }

        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        GameManager.instance.ChangeMoney(-temp.price);
        SetTilesBlock(area, TempTilemap, TileType.FINAL);
        if(temp.t == Box.BoxType.FACTORY)
        {
            SetTilesBlock(area, MainTilemap, TileType.FACTORY);
            GameManager.instance.ChangePopularity(-1);
        }
        if (temp.t == Box.BoxType.AGRICULTURE)
        {
            SetTilesBlock(area, MainTilemap, TileType.AGRICULTURE);
            GameManager.instance.ChangePopularity(1);
        }
        if (temp.t == Box.BoxType.NATURE)
        {
            SetTilesBlock(area, MainTilemap, TileType.NATURE);
        }

        Box bye = temp;
        foreach (Box b in boxes)
        {
            if(b.area == area)
            {
                bye = b;
                b.Replace();
            }
        }
        boxes.Remove(bye);
        boxes.Add(temp);
    }

    #endregion

    public enum TileType
    {
        FINAL,
        NATURE,
        FACTORY,
        AGRICULTURE
    }
}
