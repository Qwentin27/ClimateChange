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

    public int money;
    //public TextMeshProUGUI moneyText;

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
        co2Text.text = co2.ToString();
        ch4Text.text = ch4.ToString();
        monthText.text = ((month % 12) + 1) + "/" + (1880 + ((month + 1) / 12));
    }

    // Update is called once per frame
    void Update()
    {
        ch4Text.text = Math.Round(ch4, 2).ToString();
        co2Text.text = Math.Round(co2, 2).ToString();
        tempText.text = Math.Round(temp, 2).ToString();
        sealvlText.text = Math.Round(sealvl, 2).ToString();
        //moneyText.text = money.ToString();
        monthText.text = ((month % 12) + 1) + "/" + (1880 + ((month + 1) / 12));
    }

    public int GetTurn()
    {

        return turn;
    }

    public void ChangeCO2(float amount)
    {
        co2 += amount;
        if(co2 < 0) co2 = 0;
    }

    public void ChangeCH4(float amount)
    {
        ch4 += amount;
        if(ch4 < 0) ch4 = 0;
    }

    public void ChangePopularity(int amount)
    {
        popularity += amount;
        if(popularity > 100) popularity = 100;
        if (popularity < 0) popularity = 0;
    }

    public void ChangeMoney(int amount)
    {
        money += amount;
    }

    public void NextTurn()
    {
        if(button && (turn < 60))
        {
            turn++;
            month++;
            if (money < 0) ChangePopularity(-10);
            if (temp > 1.5) ChangePopularity(-10);
        }
    }
}
