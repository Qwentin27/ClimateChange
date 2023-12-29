using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public CharacterDatabase characterDBFemme;
    public CharacterDatabase characterDBHomme;
    public CharacterDatabase characterDBNonB;

    public SpriteRenderer artworkSprite;

    public PlayerData playerData;

    public Image imagePerso;

    public CharacterDatabase dbPlayer;
    private int selectedOption = 0;

    void Start()
    {
        if (playerData.choixDuPerso == 1)
        {
            dbPlayer = characterDBFemme;
        }
        
        if(playerData.choixDuPerso == 2)
        {
            dbPlayer = characterDBHomme;
        }

        if(playerData.choixDuPerso == 3)
        {
            dbPlayer = characterDBNonB;
        }

      UpdateCharacter(selectedOption);
    }

    public void NextOption()
    {
        selectedOption++;

        if(selectedOption >= dbPlayer.CharacterCount)
        {
            selectedOption = 0;
        }

        UpdateCharacter(selectedOption);
    }
    
    public void BackOption()
    {
        selectedOption--;

        if(selectedOption < 0)
        {
            selectedOption = dbPlayer.CharacterCount - 1;
        }

        UpdateCharacter(selectedOption);
    }

    public void UpdateCharacter(int selectedOption)
    {
        Character character = dbPlayer.GetCharacter(selectedOption);
        artworkSprite.sprite = character.characterSprite;
        imagePerso.sprite = character.characterSprite;
    }
}
