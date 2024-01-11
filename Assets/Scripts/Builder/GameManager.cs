using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool button = true;

    private int turn;

    public int month;
    public TextMeshProUGUI monthText;

    public float co2;
    public TextMeshProUGUI co2Text;

    public float ch4;
    public TextMeshProUGUI ch4Text;

    public float temp;
    public TextMeshProUGUI tempText;

    public float sealvl;
    public TextMeshProUGUI sealvlText;

    public int popularity;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        turn = 0;
        co2 = 290;
        co2Text.text = co2.ToString();
        ch4 = 800;
        ch4Text.text = ch4.ToString();
        month = 0;
        monthText.text = ((month % 12) + 1) + "/" + (month + 1) / 12;
    }

    // Update is called once per frame
    void Update()
    {
        tempText.text = Math.Round(temp, 2).ToString();
        sealvlText.text = Math.Round(sealvl, 2).ToString();
    }

    public int getTurn()
    {

        return turn;
    }

    public void changeCO2(float amount)
    {
        co2 += amount;
        if(co2 < 0) co2 = 0;
        co2Text.text = Math.Round(co2, 2).ToString();
    }

    public void changeCH4(float amount)
    {
        ch4 += amount;
        if(ch4 < 0) ch4 = 0;
        ch4Text.text = Math.Round(ch4, 2).ToString();
    }

    public void changePopularity(int amount)
    {
        popularity += amount;
        if(popularity > 100) popularity = 100;
        if (popularity < 0) popularity = 0;
    }

    public void nextTurn()
    {
        if(button)
        {
            turn++;
            month++;
            monthText.text = ((month % 12) + 1) + "/" + (month + 1) / 12;
        }
    }
}
