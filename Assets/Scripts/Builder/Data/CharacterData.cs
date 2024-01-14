using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterData
{
    public PlayerData playerData;
    public Image character1;
    public Image character2;
    public CharacterDatabase characterAleatoire;
    public int index;

    public CharacterData(PlayerData p, Image c1, Image c2, CharacterDatabase ca, int i)
    {
        this.playerData = p;
        this.character1 = c1;
        this.character2 = c2;
        this.characterAleatoire = ca;
        this.index = i;
    }
}
