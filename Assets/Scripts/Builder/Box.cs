using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Box : MonoBehaviour
{

    public enum BoxType{NATURE, AGRICULTURE, FINAL, FACTORY};

    public BoxType t;
    public int level;
    public int price;

    public bool Placed { get; private set; }
    public BoundsInt area;

    public int turn;

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        turn = GameManager.instance.GetTurn();
        area.size.Set(1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GetTurn() > turn)
        {
            turn++;
            level++;
        }
    }

    #endregion

    #region Build Methods

    public bool CanBePlaced()
    {
        Vector3Int positionInt = Grid.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = new Vector3Int(positionInt.x - 1, positionInt.y - 1, positionInt.z);

        if (Grid.current.CanTakeArea(areaTemp))
        {
            return true;
        }
        return false;
    }

    public void Place()
    {
        Vector3Int positionInt = Grid.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = new Vector3Int(positionInt.x - 1, positionInt.y - 1, positionInt.z);
        Placed = true;
        Grid.current.TakeArea(areaTemp);
    }

    public void Replace()
    {
        Destroy(gameObject);
    }

    #endregion
}
