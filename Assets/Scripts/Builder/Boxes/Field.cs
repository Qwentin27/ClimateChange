using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : Box
{
    public GameObject model1;
    public GameObject model2;
    private bool done;

    private void Awake()
    {
        this.t = BoxType.AGRICULTURE;
        done = false;
    }

    void Update()
    {
        if (GameManager.instance.GetTurn() > turn)
        {
            turn++;
            level++;
            if(level >= 1)
            {
                //GameManager.instance.ChangeCO2(Random.Range(0.45f, 3.66f));
                GameManager.instance.ChangeMoney(1);
            }
        }
        if(level == 1 && !done)
        {
            model1.SetActive(false);
            model2.SetActive(true);
            GameManager.instance.ChangePopularity(1);
            done = true;
        }
    }
}
