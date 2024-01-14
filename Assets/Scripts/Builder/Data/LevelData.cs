using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public GameData gameData;

    public ManagerData managerData;

    public BoxData[] factoryData;
    public BoxData[] fieldData;
    public BoxData[] pastureData;
    public BoxData[] co2Data;
    public BoxData[] ch4Data;
    public BoxData[] natureData;
    public BoxData[] finalData;

    public void Save()
    {
        GameManager x = FindObjectOfType<GameManager>();
        managerData = new ManagerData(x.turn, x.month, x.co2, x.ch4, x.temp, x.sealvl, x.popularity, x.money, x.ch4Price, x.co2Price, x.fieldPrice, x.pasturePrice, x.stats1, x.stats2);

        // Chercher objets de type Factory
        Factory[] factories = FindObjectsOfType<Factory>();

        // Sauvegarder leurs positions, rotation et scales
        factoryData = new BoxData[factories.Length];
        for (int i=0; i<factoryData.Length; i++)
        {
            factoryData[i] = new BoxData(factories[i].transform, factories[i].area, factories[i].level, factories[i].index) ;
        }

        // Chercher objets de type Field
        Field[] fields = FindObjectsOfType<Field>();

        // Sauvegarder leurs positions, rotation et scales
        fieldData = new BoxData[fields.Length];
        for (int i = 0; i < fieldData.Length; i++)
        {
            fieldData[i] = new BoxData(fields[i].transform, fields[i].area, fields[i].level, fields[i].index);
        }

        // Chercher objets de type Pasture
        Pasture[] pastures = FindObjectsOfType<Pasture>();

        // Sauvegarder leurs positions, rotation et scales
        pastureData = new BoxData[pastures.Length];
        for (int i = 0; i < pastureData.Length; i++)
        {
            pastureData[i] = new BoxData(pastures[i].transform, pastures[i].area, pastures[i].level, pastures[i].index);
        }

        // Chercher objets de type CO2factory
        CO2factory[] cO2Factories = FindObjectsOfType<CO2factory>();

        // Sauvegarder leurs positions, rotation et scales
        co2Data = new BoxData[cO2Factories.Length];
        for (int i = 0; i < co2Data.Length; i++)
        {
            co2Data[i] = new BoxData(cO2Factories[i].transform, cO2Factories[i].area, cO2Factories[i].level, cO2Factories[i].index);
        }

        // Chercher objets de type CH4factory
        CH4factory[] cH4Factories = FindObjectsOfType<CH4factory>();

        // Sauvegarder leurs positions, rotation et scales
        ch4Data = new BoxData[cH4Factories.Length];
        for (int i = 0;i < ch4Data.Length; i++) { 
            ch4Data[i] = new BoxData(cH4Factories[i].transform, cH4Factories[i].area, cH4Factories[i].level, cH4Factories[i].index);
        }

        // Chercher objets de type Nature
        Nature[] natures = FindObjectsOfType<Nature>();

        // Sauvegarder leurs positions, rotation et scales
        natureData = new BoxData[natures.Length];
        for (int i = 0; i < natures.Length; i++)
        {
            natureData[i] = new BoxData(natures[i].transform, natures[i].area, natures[i].level, natures[i].index);
        }

        // Chercher objets de type Final
        Final[] finals = FindObjectsOfType<Final>();

        // Sauvegarder leurs positions, rotation et scales
        finalData = new BoxData[finals.Length];
        for(int i = 0; i < finals.Length; i++)
        {
            finalData[i] = new BoxData(finals[i].transform, finals[i].area, finals[i].level, finals[i].index);
        }

        EditorUtility.SetDirty(this); // force la save

        Debug.Log("[Save] DONE !");
    }

    private void DestroyCustom<T>() where T : MonoBehaviour
    {
        T[] element = FindObjectsOfType<T>();
        for (int i = 0; i < element.Length; i++)
        {
            if (Application.isPlaying)
            {
                Destroy(element[i].gameObject);
            }
            else
            {
                DestroyImmediate(element[i].gameObject);
            }
        }
    }

    private Transform CreateParentTransform(string n)
    {
        GameObject parent = GameObject.Find(n);
        if (parent == null)
        {
            parent = new GameObject(n);
        }

        return parent.transform;
    }

    private Box Spawn(Box prefab, Transform parent, BoxData transData)
    {
        Box g;
        g = Instantiate(prefab, parent);
        SetTransformData(g, transData.transformData);
        SetBoxData(g, transData.area, transData.level);
        return g;
    }

    private void SetTransformData(Box g, TransformData transData)
    {
        g.transform.SetPositionAndRotation(transData.position, transData.rotation);
        g.transform.localScale = transData.scale;
    }

    private void SetBoxData(Box g, BoundsInt zone, int lvl)
    {
        g.area = zone;
        g.level = lvl;
    }

    /// <summary>
    /// Supprimer les �lements pr�sents dans la sc�ne !
    /// Cr�ation des parents et instancie les �lements !
    /// </summary>
    public void Load()
    {
        GameManager m = FindObjectOfType<GameManager>();
        m.turn = managerData.turn;
        m.month = managerData.month;
        m.co2 = managerData.co2;
        m.ch4 = managerData.ch4;
        m.temp = managerData.temp;
        m.sealvl = managerData.sealvl;
        m.popularity = managerData.popularity;
        m.money = managerData.money;
        m.ch4Price = managerData.ch4Price;
        m.co2Price = managerData.co2Price;
        m.pasturePrice = managerData.pasturePrice;
        m.fieldPrice = managerData.fieldPrice;
        m.stats1 = managerData.stats1;
        m.stats2 = managerData.stats2;

        Transform parentBox = CreateParentTransform("== BOXES ==");

        // Factories
        DestroyCustom<Factory>();
        for (int i=0; i < factoryData.Length; i++)
        {
            Spawn(gameData.prefabBoxes[0], parentBox, factoryData[i]);
        }

        // Fields
        DestroyCustom<Field>();
        for (int i = 0; i < fieldData.Length; i++)
        {
            Spawn(gameData.prefabBoxes[1], parentBox, fieldData[i]);
        }

        // Pastures
        DestroyCustom<Pasture>();
        for (int i = 0; i < pastureData.Length; i++)
        {
            Spawn(gameData.prefabBoxes[2], parentBox, pastureData[i]);
        }

        // CO2 Factories
        DestroyCustom<CO2factory>();
        for (int i = 0; i < co2Data.Length; i++)
        {
            Spawn(gameData.prefabBoxes[3], parentBox, co2Data[i]);
        }

        // CH4 Factories
        DestroyCustom<CH4factory>();
        for (int i = 0; i < ch4Data.Length; i++)
        {
            Spawn(gameData.prefabBoxes[4], parentBox, ch4Data[i]);
        }

        // Nature Tiles
        DestroyCustom<Nature>();
        for (int i = 0; i < natureData.Length; i++)
        {
            Spawn(gameData.prefabBoxes[5 + natureData[i].index], parentBox, natureData[i]);
        }

        Transform backBox = CreateParentTransform("== BACK ==");

        // Final Tiles
        DestroyCustom<Final>();
        for (int i = 0; i < finalData.Length; i++)
        {
            Spawn(gameData.prefabBoxes[7 + finalData[i].index], backBox, finalData[i]);
            
        }

        Grid g = FindObjectOfType<Grid>();
        g.LoadGrid();

        Debug.Log("[Load] DONE !");
    }
}