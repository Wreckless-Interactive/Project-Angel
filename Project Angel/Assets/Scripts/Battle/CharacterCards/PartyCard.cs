using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PartyCard : BaseCard
{

    [Header("Health")]
    public TextMeshProUGUI healthTxt;
    public Image healthBar;

    [Header("Mana")]
    public TextMeshProUGUI manaTxt;
    public Image manaBar;

    [Header("Stance")]
    public Image stanceBar;

    public override void UpdateCard()
    {

        healthTxt.text = $"{character.Health.ToString("000")}/{character.info.baseMaxHealth.ToString("000")}";
        healthBar.fillAmount = (float)character.Health / (float)character.info.baseMaxHealth;

        manaTxt.text = $"{(character as BattleCharacter_Party).Mana.ToString("000")}/{(character.info as CharacterInfo_Party).baseMaxMana.ToString("000")}";
        manaBar.fillAmount = (float)(character as BattleCharacter_Party).Mana / (float)(character.info as CharacterInfo_Party).baseMaxMana;

    }

    public override void InitCard(BattleCharacter character)
    {
        base.InitCard(character);
        nameTxt.text = this.character.info.characterName;
        UpdateCard();
    }
}
