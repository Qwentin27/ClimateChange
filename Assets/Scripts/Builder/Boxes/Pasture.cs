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
        this.level = 1;
        done = false;
    }

    void Update()
    {
        if (GameManager.instance.getTurn() > turn)
        {
            turn++;
            level++;
            if(level >= 2)
            {
                GameManager.instance.changeCH4(Random.Range(0.54f, 0.62f));
            }
        }
        if (level == 2 && !done)
        {
            model1.SetActive(false);
            model2.SetActive(true);
            GameManager.instance.changePopularity(1);
            done = true;
        }
    }
}
