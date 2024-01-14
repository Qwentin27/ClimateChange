using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CO2factory : Box
{
    public GameObject model1;
    public GameObject model2;
    public GameObject model3;
    private int done;

    private void Awake()
    {
        this.t = BoxType.FACTORY;
        this.price = GameManager.instance.co2Price;
        this.done = 0;
    }

    void Update()
    {
        if (GameManager.instance.GetTurn() > turn)
        {
            level++;
            if (level >= 2)
            {
                GameManager.instance.ChangeCO2(-Random.Range(1.98f, 11.55f));
            }
        }
        turn = GameManager.instance.GetTurn();
        if (level >= 1 && done == 0)
        {
            model1.SetActive(false);
            model2.SetActive(true);
            done++;
        }
        if (level >= 2 && done == 1)
        {
            model2.SetActive(false);
            model3.SetActive(true);
            done++;
        }
    }
}
