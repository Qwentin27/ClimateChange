using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoixModeDeJeu : MonoBehaviour
{    
    public PlayerData playerData;
    public int choix;

    public void choixModeDeJeu()
    {
        Debug.Log("ModeDeJeu1");
        choix = 1;
        playerData.choixModeDeJeu = 1;
    }

}