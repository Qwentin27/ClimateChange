using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public GameData gameData;

    public void Load()
    {
        gameData.change = true;
        SceneManager.LoadScene(4);
    }
}
