﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(BattleHUD))]
public class BattleManager : MonoBehaviour
{

    #region Variables

    public static BattleManager Instance;

    [Header("Spawns")]
    public Transform partySpawns;
    public Transform enemySpawns;

    [Header("Attack Points")]
    public Transform enemyAttackPoint;
    public Transform partyAttackPoint;

    //Character list
    private List<BattleCharacter> characterList;
    private List<BattleCharacter> partyList;
    private List<BattleCharacter> enemyList;

    //Turns
    private QueueList<BattleCharacter> turnQueue;
    private BattleCharacter currentCharacter;

    private bool selectingCharacter;
    private BattleCharacter selectedCharacter;

    public bool InBattle { get; private set; }

    #endregion

    #region Setup Battle

    public void InitBattle(List<BattleCharacter> party, List<BattleCharacter> enemies)
    {

        //Delete current characters
        DeleteCharacters();

        //Spawn in new characters
        SpawnCharacters(party, enemies);

        CameraManager.Instance.ResetBattleCameraPosition();
        selectingCharacter = false;
        selectedCharacter = null;

        BattleHUD.Instance.InitHUD(characterList);

        CameraManager.Instance.UpdateCamera(CameraManager.CameraType.Battle);

        InBattle = true;

        turnQueue = new QueueList<BattleCharacter>();
        SetTurnQueue();

        NextTurnInQueue();

    }

    private void DeleteCharacters()
    {

        if(characterList != null && characterList.Count > 0)
        {
            foreach(BattleCharacter c in characterList)
            {
                c.Destroy();
            }
        }

        characterList = new List<BattleCharacter>();
        partyList = new List<BattleCharacter>();
        enemyList = new List<BattleCharacter>();

    }

    private void SpawnCharacters(List<BattleCharacter> party, List<BattleCharacter> enemies)
    {

        int spawnIndex = 0;

        foreach(BattleCharacter c in party)
        {
            BattleCharacter character = Instantiate(c.gameObject, partySpawns.GetChild(spawnIndex).position, Quaternion.identity, transform.parent).GetComponent<BattleCharacter>();
            character.InitCharacter();
            characterList.Add(character);
            partyList.Add(character);
            spawnIndex++;
        }

        spawnIndex = 0;

        foreach (BattleCharacter c in enemies)
        {
            BattleCharacter character = Instantiate(c.gameObject, enemySpawns.GetChild(spawnIndex).position, Quaternion.identity, transform.parent).GetComponent<BattleCharacter>();
            character.InitCharacter();
            characterList.Add(character);
            enemyList.Add(character);
            spawnIndex++;
        }

    }

    public static IEnumerable<BattleCharacter> OrderCharsByCurrentSpeed(List<BattleCharacter> chars)
    {
        return chars.OrderBy(c => c.info.baseSpeed);
    }

    public void SetTurnQueue()
    {
        foreach(BattleCharacter character in OrderCharsByCurrentSpeed(characterList))
        {
            turnQueue.Enqueue(character);
        }
    }

    public void RemoveCharacter(BattleCharacter character)
    {
        RemoveFromQueue(character);
        characterList.Remove(character);
        enemyList.Remove(character);
        BattleHUD.Instance.DeleteCharacterCard(character);
    }

    private void RemoveFromQueue(BattleCharacter character)
    {

        if (turnQueue.Contains(character))
        {
            turnQueue.Remove(character);
        }

    }

    #endregion

    #region Player Turn

    public void Weapon()
    {
        StartCoroutine(SelectCharacter<BattleCharacter_Enemy>());
    }

    public void Cast()
    {
        StartCoroutine(SelectCharacter<BattleCharacter_Enemy>());
    }

    public void Guard()
    {
        currentCharacter.IsGuarding = true;
        SetupNextTurn();
    }

    public void Item()
    {

        Dictionary<Item, int> items = InventoryManager.Instance.GetItemsByType<RestorableItem>();
        StartCoroutine(BattleHUD.Instance.SetupButtons(items));

    }

    public void Enrage()
    {

        if ((currentCharacter as BattleCharacter_Party).TP == 6)
        {
            print("Entered Enrage Mode");
            (currentCharacter as BattleCharacter_Party).TP = 0;
            SetupNextTurn();
        }
        else
            print("You don't have the TP yet...");

        SetupNextTurn();

    }

    public void Stance()
    {
        BattleHUD.Instance.UpdateMenu(BattleHUD.MenuType.Stance);
    }

    public void UpdateStance(BattleCharacter_Party.Stances stance)
    {
        (currentCharacter as BattleCharacter_Party).SetStance(stance);
        SetupNextTurn();
    }

    #endregion

    #region Enemy Turn

    private void EnemyTurn()
    {
        currentCharacter.GetComponent<BattleAI>().DoTurn();
    }

    #endregion

    #region Battle

    private void NextTurnInQueue()
    {

        currentCharacter = turnQueue.Dequeue();

        if (currentCharacter is BattleCharacter_Party)
        {
            if ((currentCharacter as BattleCharacter_Party).IsKnockedOut == true)
                SetupNextTurn();
        }

        MoveCharacterToPoint(currentCharacter, PointType.Attack);
        currentCharacter.ProcessEffects();

        if (currentCharacter is BattleCharacter_Party)
        {
            BattleHUD.Instance.UpdateMenu(BattleHUD.MenuType.Main);
        }
        else if(currentCharacter is BattleCharacter_Enemy)
        {
            EnemyTurn();
        }

    }

    public IEnumerator DealDamage()
    {

        BattleHUD.Instance.UpdateMenu(BattleHUD.MenuType.None);

        selectedCharacter.TakeDamage(currentCharacter);

        yield return new WaitForSeconds(1f);

        SetupNextTurn();

    }

    public void SetupNextTurn()
    {
        MoveCharacterToPoint(currentCharacter, PointType.Start);

        if (turnQueue.Count == 0)
            SetTurnQueue();

        NextTurnInQueue();
    }

    #endregion

    private enum PointType { Start, Attack}

    private void MoveCharacterToPoint(BattleCharacter character, PointType type)
    {

        if (type == PointType.Attack)
        {
            Vector3 point = Vector3.zero;

            if (character is BattleCharacter_Party)
                point = partyAttackPoint.position;
            else
                point = enemyAttackPoint.position;

            character.transform.position = point;
        }
        else
            character.transform.position = character.startingPoint;

    }

    public List<BattleCharacter> GetPartyList()
    {
        return partyList;
    }

    List<BattleCharacter> tempList;
    int index = 0;

    private IEnumerator SelectCharacter<T>()where T : BattleCharacter
    {

        BattleHUD.Instance.UpdateMenu(BattleHUD.MenuType.None);

        tempList = characterList.Where(x => x.GetType() == typeof(T)).ToList();
        index = 0;
        yield return new WaitForEndOfFrame();
        selectingCharacter = true;
    }

    private void CharacterSelector()
    {
        if (!selectingCharacter)
            return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            index++;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            index--;

        if (index < 0)
            index = tempList.Count - 1;
        else if (index == tempList.Count)
            index = 0;

        selectedCharacter = tempList[index];

        if(selectedCharacter)
            CameraManager.Instance.SetBattleCameraPosition(selectedCharacter.transform.position);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            selectingCharacter = false;
            CameraManager.Instance.ResetBattleCameraPosition();
            StartCoroutine(DealDamage());
        }

    }

    public void SetSelectedCharacter(BattleCharacter character)
    {
        selectedCharacter = character;
    }

    public List<BattleCharacter> GetPartyMembers()
    {
        return characterList.Where(x => x.GetType() == typeof(BattleCharacter_Party)).ToList();
    }

    #region Unity Methods

    private void Update()
    {

        if (!InBattle)
            return;

        CharacterSelector();

    }

    private void Awake()
    {
        Instance = this;
    }

    #endregion
}
