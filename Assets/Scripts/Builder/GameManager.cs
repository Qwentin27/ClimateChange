using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using static UnityEngine.ParticleSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameData gameData;

    public bool button = true;

    public int turn;

    public int month;
    public float co2;
    public float ch4;

    public float temp;
    public float sealvl;

    public int popularity;
    public int money;

    public int ch4Price;
    public int co2Price;
    public int fieldPrice;
    public int pasturePrice;

    public TextMeshProUGUI monthText;
    public TextMeshProUGUI co2Text;
    public TextMeshProUGUI ch4Text;

    public TextMeshProUGUI tempText;
    public TextMeshProUGUI sealvlText;

    public TextMeshProUGUI moneyText;

    public TextMeshProUGUI ch4PriceText;
    public TextMeshProUGUI co2PriceText;
    public TextMeshProUGUI fieldPriceText;
    public TextMeshProUGUI pasturePriceText;

    public List<Vector3> stats1;
    public List<Vector4> stats2;

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

        co2PriceText.text = co2Price.ToString();
        ch4PriceText.text = ch4Price.ToString();
        fieldPriceText.text = fieldPrice.ToString();
        pasturePriceText.text = pasturePrice.ToString();

        stats1 = new();
        stats2 = new();
}

    // Update is called once per frame
    void Update()
    {
        ch4Text.text = Math.Round(ch4, 2).ToString();
        co2Text.text = Math.Round(co2, 2).ToString();
        tempText.text = Math.Round(temp, 2).ToString();
        sealvlText.text = Math.Round(sealvl, 2).ToString();
        moneyText.text = money.ToString();
        monthText.text = ((month % 12) + 1) + "/" + (1880 + ((month + 1) / 12));
    }

    public void Save()
    {
        try
        {
            var csv = new StringBuilder();
            var newLine = string.Format("month, money, popularity, ch4, co2, temp, sealvl");
            csv.AppendLine(newLine);
            for (int i = 0; i < stats1.Count; i++)
            {
                newLine = string.Format("{0},{1},{2},{3},{4},{5},{6}",
                    stats1[i].x.ToString().Replace(",","."), 
                    stats1[i].y.ToString().Replace(",", "."), 
                    stats1[i].z.ToString().Replace(",", "."), 
                    stats2[i].x.ToString().Replace(",", "."), 
                    stats2[i].y.ToString().Replace(",", "."), 
                    stats2[i].z.ToString().Replace(",", "."), 
                    stats2[i].w.ToString().Replace(",", "."));
                csv.AppendLine(newLine);
            }
            File.WriteAllText("data.csv", csv.ToString());
        } catch (Exception e)
        {
            Debug.LogException(e);
        }
        gameData.levels[0].Save();
    }

    public void Load()
    {
        gameData.levels[0].Load();
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
            stats1.Add(new Vector3(month, money, popularity));
            stats2.Add(new Vector4(ch4, co2, temp, sealvl));

            turn++;
            month++;

            ChangeCO2(UnityEngine.Random.Range(37.33f, 186.66f));
            if (money < 0) ChangePopularity(-10);
            if (temp > 1.5) ChangePopularity(-10);
        }
    }
}
