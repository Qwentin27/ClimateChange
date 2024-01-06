using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class JsonReadWriteSystem : MonoBehaviour
{
    public string chemin, jsonString;
    public GameObject textDisplay;
    public GameObject ChoiceButton1, ChoiceButton2;
    public GameObject ChoiceButton1Text, ChoiceButton2Text;
    public Dictionary<string, Passage> values;
    public Root root;

    // faire une fonction pour les choix du joueur et réfléchir aux enbranchements

    public void WriteOnJson() // gestion évènements
    {

    }

    public void test(string jsonString)
    {
        /*string t = "{" +
            "oui" +
            "}";
        string s = "";
        for (int i = 1; i < t.Length ;i++)
        {
            s += t[i];
        }
        Debug.Log(s);*/
        chemin = "Assets/Scripts/JSON/TwineOrigin.json";
        jsonString = File.ReadAllText(chemin);
        bool flag1 = true;
        bool flag2 = true;
        jsonString.Replace('[', 'e');
        Debug.Log(jsonString);

       /*for (int i = 1; i < jsonString.Length; i++)
        {

            if (flag1 && jsonString[i] == ('[') && jsonString.Substring(i-11, 8) == "passages")
            {
                jsonString.Remove(i, 1);
                flag1 = false;
                i--;
            }

            if (flag2 && jsonString[i] == (']') && jsonString.Substring(i+10, 13) == "Projet Climat") // a vérifier
            {
                jsonString.Remove(i, 1);
                flag2 = false;
                i--;
            }

        }*/

        //jsonString.Replace("passages", "D1"); // va remplacer tous les passages
    }

    public void DisplayChoices()
    {
        // Affichage choix 1

        string Choice1 = root.passages[0].links[0].name;
        Debug.Log(Choice1);
        ChoiceButton1Text.GetComponent<TMPro.TMP_Text>().text = Choice1;

        // Affichage Choix 2

        string Choice2 = root.passages[0].links[1].name;
        Debug.Log(Choice2);
        ChoiceButton2Text.GetComponent<TMPro.TMP_Text>().text = Choice2;
    }

    public void DisplayDialogue()
    {
        textDisplay.GetComponent<Text>().text = root.passages[0].text;
        Debug.Log(root.passages[0].text);
    }

    public void LoadFromJson() // voir JsonDemo du projet d'externalisation avec M. Panzoli

        // Dans le Json, les balises (text1) correspondent au texte dit au joueur et (resp0) à la réponse proposée au joueur.

        // Il faudra alors vérifier qu'il n'y ait pas les réponses en tant que texte dit au joueur (étant donné que les réponses
        // sont également dans le JSON avec les balises (text1) de façon automatique).

        // A VOIR SI ON UTILISE PAS PLUTÔT LES BALISES "TEXT" ET "NAME" DEJA EXISTANTES 
    {

        /*
        //Debug.Log(jsonString);
        textDisplay.GetComponent<Text>().text = jsonString; // affichage du texte
        values = JsonConvert.DeserializeObject<Dictionary<string, Passage>>(jsonString);
        //Passage[] values = JsonHelper.getJsonArray<Passage>(jsonString);
        //Debug.Log(s);
        Debug.Log(values["passages"].text);*/

        chemin = "Assets/Scripts/JSON/TwineOrigin.json";
        jsonString = File.ReadAllText(chemin);
        root = JsonConvert.DeserializeObject<Root>(jsonString);
        //textDisplay.GetComponent<Text>().text = root.passages[0].text;
        Debug.Log(root.passages[0].text);
    }

    [System.Serializable]
    public class Link
    {
        public string name { get; set; }
        public string link { get; set; }
        public string pid { get; set; }
    }

    [System.Serializable]
    public class Passage
    {
        public string text { get; set; }
        public List<Link> links { get; set; }
        public string name { get; set; }
        public string pid { get; set; }
        public Position position { get; set; }
    }

    [System.Serializable]
    public class Position
    {
        public string x { get; set; }
        public string y { get; set; }
    }

    [System.Serializable]
    public class Root
    {
        public List<Passage> passages { get; set; }
        public string name { get; set; }
        public string startnode { get; set; }
        public string creator { get; set; }

        [JsonProperty("creator-version")]
        public string creatorversion { get; set; }
        public string ifid { get; set; }
    }
}
