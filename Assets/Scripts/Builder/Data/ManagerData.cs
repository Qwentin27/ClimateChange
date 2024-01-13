using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ManagerData
{
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

    public ManagerData(int tu, int mont, float co, float ch, float te, float s, int p, int mone, int chp, int cop, int fp, int pp)
    {
        this.turn = tu;
        this.month = mont;
        this.co2 = co;
        this.ch4 = ch;
        this.temp = te;
        this.sealvl = s;
        this.popularity = p;
        this.money = mone;
        this.ch4Price = chp;
        this.co2Price = cop;
        this.fieldPrice = fp;
        this.pasturePrice = pp;
    }
}
