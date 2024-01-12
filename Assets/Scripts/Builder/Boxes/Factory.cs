using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Box
{
    private void Awake()
    {
        this.t = BoxType.FACTORY;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GetTurn() > turn)
        {
            turn++;
            level++;
            if (this.t == BoxType.FACTORY)
            {
                GameManager.instance.ChangeCO2(Random.Range(1.98f, 11.55f));
                GameManager.instance.ChangeMoney(5);
            }
        }
    }
}
