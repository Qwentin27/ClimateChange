using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Button = UnityEngine.UI.Button;

public class JsonReadWriteSystem : MonoBehaviour
{
    public string chemin, jsonString;
    public GameObject textDisplay;
    public GameObject ChoiceButton1, ChoiceButton2;
    public Button btn1, btn2;
    public int actualNode;
    public int indexPassage = 0;
    public int indexSort;
    public GameObject ChoiceButton1Text, ChoiceButton2Text;
    public Dictionary<int, string> dictPassages = new Dictionary<int, string>();
                    // (noeud du passage, passage à afficher)
    public Dictionary<int, List<string>> dictResponses = new Dictionary<int, List<string>>();
                    // (noeud du passage, réponses possibles)
    public Dictionary<List<int>, int> dictNodes = new Dictionary<List<int>, int>();
    // (noeud actuel, choix fait -0, 1 ou 2-) --> nouveau noeud 
    // 0 : pas de choix / suite de l'explication de la/du secrétaire ; 1 : choix 1 ; 2 : choix 2
    public Queue<List<int>> queueMessages = new Queue<List<int>>();
    public Root originRoot;

    // faire une fonction pour les choix du joueur et réfléchir aux enbranchements

    void Start()
    {
        btn1 = ChoiceButton1.GetComponent<Button>();
        btn2 = ChoiceButton2.GetComponent<Button>();
        btn1.onClick.AddListener(ChoiceClick1);
        btn2.onClick.AddListener(ChoiceClick2);
        /*List<(int, int)> keys = new List<(int, int)>(dictNodes.Keys);
        Debug.Log(keys.Count);
        queueMessages.Enqueue(keys[0]);
        Debug.Log(queueMessages.Peek());*/
        
        /*foreach (List<int> element in queueMessages)
        {
            Debug.Log(queueMessages.Dequeue());
            List<int> l = new List<int> {1, 2};
            queueMessages.Enqueue(dictNodes.Keys.FirstOrDefault(x => x == l));
        }*/

        /*foreach(KeyValuePair<int, string> item in dictPassages)
        {
            DisplayDialogue(item.)
        }*/
    }

    public void LaunchDiscussion()
    {
        foreach (List<int> element in queueMessages.ToList())
        {
            //Debug.Log(queueMessages.Dequeue());

            if (element[1] == 0)
            {
                DisplayDialogue(dictPassages[element[0]]);
            }
            else if (element[1] == 1 || element[1] == 2)
            {
                DisplayDialogue(dictPassages[element[0]]);
                DisplayChoices(dictResponses[element[0]][0], dictResponses[element[0]][1]);
                actualNode = element[0];
            }

            queueMessages.Dequeue();

            /*List<int> l = new List<int> { 1, 2 };
            queueMessages.Enqueue(dictNodes.Keys.FirstOrDefault(x => x == l));*/

        }
    }

    public void UpdateQueue(int n)
    {
        bool nodeFlag = false;
        bool choiceFlag = true;
        bool end = false;
        Passage foundPassage = originRoot.passages.First(passage => (passage.pid == "" + (n)));

        foreach (KeyValuePair<List<int>, int> item in dictNodes)
        {
            if (foundPassage.links == null)
            {
                end = true;
                DisplayDialogue(dictPassages[n]);
            }
            else
            {
                if (item.Key[0] == n)
                {
                    nodeFlag = true;
                }

                if (nodeFlag && choiceFlag)
                {
                    queueMessages.Enqueue(item.Key);
                }

                if (item.Key[1] != 0)
                {
                    choiceFlag = false;
                }
            }
            
        }
        Debug.Log(queueMessages.Count);
        if (!end)
        {
            LaunchDiscussion();
        }
        
    }

    public void WriteInJson() // gestion évènements
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
        Debug.Log(s);
        chemin = "Assets/Scripts/JSON/TwineOrigin.json";
        jsonString = File.ReadAllText(chemin);
        bool flag1 = true;
        bool flag2 = true;
        jsonString.Replace('[', 'e');
        //Debug.Log(jsonString);

       for (int i = 1; i < jsonString.Length; i++)
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

    public void FillNodesDictionary()
    {
        //int sameIndex = 0;

        /*for (int i = 0; i < dictResponses.Count; i++)
        {
            KeyValuePair<int, List<string>> entry = dictResponses.ElementAt(i);
            Debug.Log(entry.Key + " : " + entry.Value);
        }*/

        //Debug.Log("Nombre " + dictResponses.Count);

        foreach (KeyValuePair<int, string> item in dictPassages)
        {
            //Debug.Log(item.Key);
            //Debug.Log("Item.key " + item.Key);
            //Debug.Log("originRoot null ? " + originRoot);
            //Debug.Log("originRoot.passages count  = " + originRoot.passages.Count);
            //Debug.Log("originRoot.passages[item.Key]  = " + originRoot.passages[item.Key]);
            //Debug.Log("originRoot.passages[item.Key].links  = " + originRoot.passages[item.Key].links);
            //Debug.Log("Is Links null ? " + (originRoot.passages[item.Key].links == null));
            //Debug.Log("Is Links Empty ? " + (originRoot.passages[item.Key].links.Count == 0));
            //Debug.Log("originRoot.passages[item.Key].links[0]  = " + originRoot.passages[item.Key].links[0]);

            Passage foundPassage = originRoot.passages.First(passage => (passage.pid == ""+(item.Key)));
            //Debug.Log(foundPassage);
            if(foundPassage != null && foundPassage.links != null)
            {
                //Debug.Log(foundPassage);
                if (foundPassage.links.Count == 1)
                {
                    List<int> replaceList = new List<int> { item.Key, 0 };
                    dictNodes.Add(replaceList, int.Parse(foundPassage.links[0].pid));
                }
                else
                {
                    foreach (Link l in foundPassage.links)
                    {
                        List<int> replaceList = new List<int> {item.Key, 1};
                        bool containsFlag = false;
                        foreach (List<int> key in dictNodes.Keys)
                        {
                            if ((key[0] == item.Key) && (key[1] == 1))
                            {
                                containsFlag = true;
                            }
                        }

                        if (containsFlag)
                        {
                            List<int> replaceList2 = new List<int> {item.Key, 2};
                            dictNodes.Add(replaceList2, int.Parse(foundPassage.links[1].pid));
                        }
                        else
                        {
                            dictNodes.Add(replaceList, int.Parse(foundPassage.links[0].pid));
                        }
                            
                    }
                }
                
                
            }
            else
            {
                List<int> replaceList = new List<int> {item.Key, 0};
                dictNodes.Add(replaceList, 0);
            }
            //if (originRoot.passages.Count >= item.Key && originRoot.passages[item.Key].links != null)
            //{
            //    if (sameIndex == item.Key)
            //    {
            //        dictNodes.Add((item.Key, 2), int.Parse(originRoot.passages[item.Key].links[1].pid));
            //    }
            //    dictNodes.Add((item.Key, 1), int.Parse(originRoot.passages[item.Key].links[0].pid));
            //}
            //sameIndex = item.Key;
        }
    }

    public void FillDictionary()
    {
        
        for (int i = 0; i < originRoot.passages.Count; i++)
        {
            dictPassages.Add(int.Parse(originRoot.passages[i].pid), originRoot.passages[i].text);
            List<string> passLinks = new List<string>();
            //Debug.Log(originRoot.passages[i].links == null);
            // faire un test pour quand on arrive au bout du passage
            // pas de links dans le JSON lorsqu'il n' y a pas de choix 
            // regarder la fin du JSON 
            if (originRoot.passages[i].links != null)
            {
                for (int j = 0; j < originRoot.passages[i].links.Count; j++)
                {
                    passLinks.Add(originRoot.passages[i].links[j].name);
                }
                dictResponses.Add(int.Parse(originRoot.passages[i].pid), passLinks);
            }
            else
            {
                dictResponses.Add(int.Parse(originRoot.passages[i].pid), passLinks);
            }
            
        }
    }

    public string SortMessages(string sub)
    {
        indexSort = sub.IndexOf('|');
        Debug.Log(sub);
        //Debug.Log(indexSort);
        sub = sub.Substring(0, indexSort);

        return sub;
    }

    public void ChoiceClick1() // le joueur a effectué le choix 1
    {
        /*Debug.Log("Noeud actuel" + actualNode);
        List<int> nodeList = new List<int>();
        nodeList.Add(actualNode);
        nodeList.Add(1);
        int newNode = dictNodes[nodeList];*/
        Debug.Log(actualNode);
        Debug.Log(SearchPidNewPassageThroughChoice(actualNode));
        UpdateQueue(SearchPidNewPassageThroughChoice(actualNode));
    }

    public int SearchPidNewPassageThroughChoice(int n)
    {
        Passage foundPassage = originRoot.passages.First(passage => (passage.pid == "" + (n)));
        return (int.Parse(foundPassage.links[0].pid));
    }

    public void ChoiceClick2() // le joueur a effectué le choix 2
    {
        int indNewPassage = SearchIndexNewPassageThroughChoice(originRoot, indexPassage, 1); //int.Parse(originRoot.passages[indexPassage].links[1].pid);
        //Debug.Log(indNewPassage);
        DisplayDialogue(originRoot.passages[indNewPassage].text);

    }

    public int SearchIndexNewPassageThroughChoice(Root root, int indP, int choice)
    {
        int indNewPassage = 0;

        for (int i = 0; i < root.passages.Count; i++)
        {
            if (root.passages[i].pid == root.passages[indP].links[choice].pid)
            {
                indNewPassage = i;
            }
        }

        return indNewPassage;
    }

    public void DisplayChoices(string textChoice1, string textChoice2)
    {
        // Affichage choix 1

        ChoiceButton1Text.GetComponent<TMPro.TMP_Text>().text = textChoice1;

        // Affichage Choix 2

        ChoiceButton2Text.GetComponent<TMPro.TMP_Text>().text = textChoice2;
    }

    public void DisplayDialogue(string passageText)
    {
        //Debug.Log(passageText);
        string textToDisp = SortMessages(passageText);
        textDisplay.GetComponent<Text>().text = textToDisp;
    }

    public static void PrintDictPassages<K, V>(Dictionary<K, V> dict)
    {
        foreach (K key in dict.Keys)
        {
            Debug.Log(key + " : " + dict[key]);
        }
    }

    /*public static void PrintDictResponses<K, List<string>>(Dictionary<K, List<string>> dict)
    {
        foreach (K key in dict.Keys)
        {
            for (int i = 0; i < (dict[key]).Count; i++)
            {
                Debug.Log(key + " : " + (dict[key])[i]);
            }
            
        }
    }*/

    

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

        chemin = "Assets/Scripts/Dialogue/JSON/TwineOrigin.json";
        jsonString = File.ReadAllText(chemin);
        originRoot = JsonConvert.DeserializeObject<Root>(jsonString);
        Debug.Log("ORIGIN ROOT #### -> \n " + originRoot);
        //DisplayDialogue(originRoot.passages[0].text);
        FillDictionary();
        //PrintDictPassages(dictPassages);
        //Debug.Log("Nombre passages : " + dictPassages.Count);

        /*foreach (int key in dictResponses.Keys)
        {
            for (int i = 0; i < (dictResponses[key]).Count; i++)
            {
                Debug.Log(key + " : " + (dictResponses[key])[i]);
            }

        }*/

        FillNodesDictionary();
        //Debug.Log(dictNodes.Count);

        foreach (List<int> key in dictNodes.Keys)
        {

            Debug.Log(key[0] + ";" + key[1] + " : " + (dictNodes[key]));

        }

        /*Debug.Log(queueMessages.Peek());
        Debug.Log(queueMessages.Count);*/

        UpdateQueue(1);
        //Debug.Log(queueMessages.Count);

        /*if (queueMessages.Count > 0)
        {
            updateQueue();
        }*/

        //textDisplay.GetComponent<Text>().text = root.passages[0].text;
        //Debug.Log(root.passages[0].text);
    }

    [System.Serializable]
    public class Link
    {
        public string name { get; set; }
        public string link { get; set; }
        public string pid { get; set; }
        public override string ToString()
        {
            return "[" + pid + "] - " + name;
        }
    }

    [System.Serializable]
    public class Passage
    {
        public string text { get; set; }
        public List<Link> links { get; set; }
        public string name { get; set; }
        public string pid { get; set; }
        public Position position { get; set; }

        public override string ToString()
        {
            string message = "[" + pid + "] - " + name + ((links != null && links.Count > 0) ? " :" : "");
            if(links!=null)
                for (int i = 0; i < links.Count; i++)
                    message += "\n\t\t"+ links[i].ToString();
            return message;
        }
    }

    [System.Serializable]
    public class Position
    {
        public string x { get; set; }
        public string y { get; set; }
    }

    [System.Serializable]
    public class Root // objet contenant l'entièreté d'un acte et les choix possibles
    {
        public List<Passage> passages { get; set; }
        public string name { get; set; } // nom de l'acte (ici, "Projet Climat")
        public string startnode { get; set; }
        public string creator { get; set; }

        [JsonProperty("creator-version")]
        public string creatorversion { get; set; }
        public string ifid { get; set; }

        public override string ToString()
        {
            string message = name + ((passages != null && passages.Count > 0) ? " :" : "");
            if (passages != null)
                for (int i = 0; i < passages.Count; i++)
                    message += "\n\t" + passages[i].ToString();
            return message;
        }
    }
}
