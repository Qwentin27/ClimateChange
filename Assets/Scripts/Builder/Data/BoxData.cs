using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoxData
{
    public TransformData transformData;
    public BoundsInt area;
    public int level;
    public int index;
    public bool upgrade;

    public BoxData(Transform trans, BoundsInt a, int lvl, int i, bool u)
    {
        transformData = new TransformData(trans);
        area = a;
        level = lvl;
        index = i;
        this.upgrade = u;
    }
}