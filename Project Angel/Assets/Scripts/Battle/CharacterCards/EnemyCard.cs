using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyCard : BaseCard
{

    public Image healthBar;

    public override void UpdateCard()
    {
        healthBar.fillAmount = (float)character.Health / (float)character.info.baseMaxHealth;
    }

    public override  void InitCard(BattleCharacter character)
    {
        base.InitCard(character);
        nameTxt.text = character.info.characterName;
        UpdateCard();

    }

    private void Update()
    {
        Vector3 pos = CameraManager.Instance.battleCamera.WorldToScreenPoint((character as BattleCharacter_Enemy).characterCardPivot.position);
        transform.position = pos;
    }

}
