using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH4factory : Box
{
    public GameObject model1;
    public GameObject model2;
    public GameObject model3;
    private bool done;

    private void Awake()
    {
        this.t = BoxType.FACTORY;
        this.level = 1;
        this.done = false;
    }

    void Update()
    {
        if (GameManager.instance.getTurn() > turn)
        {
            turn++;
            level++;
            if(level >= 3)
            {
                GameManager.instance.changeCH4(-Random.Range(1.98f, 11.55f));
            }
        }
        if (level == 2 && !done)
        {
            model1.SetActive(false);
            model2.SetActive(true);
            done = true;
        }
        if (level == 3 && done)
        {
            model2.SetActive(false);
            model3.SetActive(true);
            done = false;
        }
    }
}
