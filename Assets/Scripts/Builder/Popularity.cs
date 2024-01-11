using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popularity : MonoBehaviour
{
    public GameObject model1;
    public GameObject model2;
    public GameObject model3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y > ((GameManager.instance.popularity - 2) * 0.0334))
        {
            if (transform.position.y < ((GameManager.instance.popularity + 2) * 0.0334))
            {
                transform.Translate(0, 0, 0);

            } else
            {
                transform.Translate(0, (-1) * Time.deltaTime, 0);
            }
        } else
        {
            transform.Translate(0, 1 * Time.deltaTime, 0);
            
        } 
        if (GameManager.instance.popularity < 33)
        {
            model1.SetActive(false);
            model2.SetActive(false);
            model3.SetActive(true);
        } else if (GameManager.instance.popularity < 66)
        {
            model1.SetActive(false);
            model2.SetActive(true);
            model3.SetActive(false);
        } else
        {
            model1.SetActive(true);
            model2.SetActive(false);
            model3.SetActive(false);
        }
    }
}
