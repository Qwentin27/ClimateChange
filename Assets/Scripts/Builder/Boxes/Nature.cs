using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Nature : Box
{
    public GameObject model1;
    public GameObject model2;
    private bool done;

    private void Awake()
    {
        this.t = BoxType.NATURE;
        done = false;
        index = Random.Range(0, 2);
    }

    void Update()
    {
        if (GameManager.instance.GetTurn() > turn)
        {
            level++;
            if(index == 0 && level >= 1)
            {
                GameManager.instance.ChangeCO2(-Random.Range(0.5f, 1.33f));
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
