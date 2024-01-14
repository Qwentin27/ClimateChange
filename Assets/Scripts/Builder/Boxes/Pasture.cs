using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pasture : Box
{
    public GameObject model1;
    public GameObject model2;
    private bool done;

    private void Awake()
    {
        this.t = BoxType.AGRICULTURE;
        this.price = GameManager.instance.pasturePrice;
        done = false;
    }

    void Update()
    {
        if (GameManager.instance.GetTurn() > turn)
        {
            level++;
            if(level >= 1)
            {
                GameManager.instance.ChangeCH4(Random.Range(0.54f, 0.62f));
                GameManager.instance.ChangeMoney(1);
            }
        }
        turn = GameManager.instance.GetTurn();
        if (level >= 1 && !done)
        {
            if (level == 1 && !upgrade)
            {
                GameManager.instance.ChangePopularity(1);
                upgrade = true;
            }
            model1.SetActive(false);
            model2.SetActive(true);
            done = true;
        }
    }
}
