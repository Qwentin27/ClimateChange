using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInGame : MonoBehaviour
{
    public PlayerData playerData;
    public Image character;
    public Image character2;
    public CharacterDatabase characterAleatoire;
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        character.sprite = playerData.spritePerso;
        index = characterAleatoire.IndexCharacter;
        character2.sprite = characterAleatoire.GetCharacter(index).characterSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
