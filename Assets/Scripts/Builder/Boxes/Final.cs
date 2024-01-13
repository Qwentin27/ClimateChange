using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Final : Box
{
    private void Awake()
    {
        this.t = BoxType.FINAL;
    }

    void Update()
    {
        if (GameManager.instance.GetTurn() > turn)
        {
            level++;
        }
        turn = GameManager.instance.GetTurn();
    }
}