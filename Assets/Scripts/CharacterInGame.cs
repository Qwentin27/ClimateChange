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
    // Start is called before the first frame update
    void Start()
    {
        character.sprite = playerData.spritePerso;
        character2.sprite = characterAleatoire.GetCharacter(characterAleatoire.IndexCharacter).characterSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
