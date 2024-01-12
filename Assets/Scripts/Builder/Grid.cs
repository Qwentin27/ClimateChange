using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{
    public static Grid current;

    public Vector2 size;
    public Vector2 offset;

    public Box[] elementPrefab;
    private Box element;
    public Transform root;

    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;

    private static readonly Dictionary<TileType, TileBase> tileBases = new();

    private readonly List<Box> boxes = new();
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

        for (int i = 0; i < size.x; i++)
        {
            for(int j = 0; j < size.y; j++)
            {
                int x = i-4;
                int y = j-4;

                int index = Random.Range(0, elementPrefab.Length);
                element = Instantiate(elementPrefab[index], root);
                element.transform.position = new Vector3((x-y)*offset.x, (x+y)*offset.y, 0);
                element.transform.localScale = new Vector3(3, (float) 2.75, 1);


                element.area = new BoundsInt(new Vector3Int(x - 1, y - 1, 0), Vector3Int.one);

                boxes.Add(element);
                if (index == 0)
                {
                    SetTilesBlock(new BoundsInt(new Vector3Int(x - 1, y - 1, 0), Vector3Int.one), MainTilemap, TileType.FACTORY);
                } else if(index <= 2)
                {
                    SetTilesBlock(new BoundsInt(new Vector3Int(x - 1, y - 1, 0), Vector3Int.one), MainTilemap, TileType.AGRICULTURE);
                } else if(index <= 3)
                {
                    SetTilesBlock(new BoundsInt(new Vector3Int(x - 1, y - 1, 0), Vector3Int.one), MainTilemap, TileType.NATURE);
                } else
                {
                    SetTilesBlock(new BoundsInt(new Vector3Int(x - 1, y - 1, 0), Vector3Int.one), MainTilemap, TileType.FINAL);
                }
            }
        }
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
            if (temp.CanBePlaced())
            {
                temp.Place();
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
                if (baseArray[i] != tileBases[TileType.NATURE])
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
        foreach (Box b in boxes)
        {
            if (b.area == area && b.level < 1 && b.t == Box.BoxType.NATURE)
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
