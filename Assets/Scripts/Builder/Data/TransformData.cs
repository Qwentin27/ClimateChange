using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransformData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public TransformData(Transform trans)
    {
        position = trans.position;
        rotation = trans.rotation;
        scale = trans.localScale;
    }

    public TransformData(Vector3 pos, Quaternion rot, Vector3 s)
    {
        position = pos;
        rotation = rot;
        scale = s;
    }
}
