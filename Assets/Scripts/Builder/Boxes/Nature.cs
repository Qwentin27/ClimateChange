using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Nature : Box
{
    public GameObject model1;
    public GameObject model2;
    public GameObject model3;
    private bool done;
    private int i;

    private void Awake()
    {
        this.t = BoxType.NATURE;
        done = false;
        i = Random.Range(0, 2);
    }

    void Update()
    {
        if (GameManager.instance.GetTurn() > turn)
        {
            turn++;
            level++;
            if(i == 0 && level >= 1)
            {
                GameManager.instance.ChangeCO2(-Random.Range(0.5f, 1.33f));
            }
        }
        if (level == 1 && !done && i == 0)
        {
            model1.SetActive(false);
            model2.SetActive(true);
            GameManager.instance.ChangePopularity(1);
            done = true;
        }

        if (level == 1 && !done && i == 1)
        {
            model1.SetActive(false);
            model3.SetActive(true);
            done = true;
        }
    }
}
