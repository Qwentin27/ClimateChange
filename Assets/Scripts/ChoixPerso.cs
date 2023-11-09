using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoixPerso : MonoBehaviour
{    
    public PlayerData playerData;
    public int choix;

    public void ChoixNonBinaire()
    {
        Debug.Log("NonBinaire");
        choix = 3;
        playerData.choixDuPerso = 3;
    }

    public void ChoixHomme()
    {
        Debug.Log("Homme");
        choix = 2;
        playerData.choixDuPerso = 2;
    }

    public void ChoixFemme()
    {
        Debug.Log("Femme");
        choix = 1;
        playerData.choixDuPerso = 1;
    }

}