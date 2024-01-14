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

    public List<Vector3> stats1;
    public List<Vector4> stats2;

    public ManagerData(int tu, int mont, float co, float ch, float te, float s, int p, int mone, int chp, int cop, int fp, int pp, List<Vector3> s1, List<Vector4> s2)
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
        this.stats1 = s1;
        this.stats2 = s2;
    }
}
