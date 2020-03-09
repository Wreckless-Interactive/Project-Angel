using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleHUD : MonoBehaviour
{

    #region Variables

    public static BattleHUD Instance;

    public GameObject battleHUD;

    [Header("Character Cards")]
    public GameObject partyCardPrefab;
    public Transform partyCardPanel;
    public GameObject enemyCardPrefab;
    private List<BaseCard> characterCards;

    [Header("Selection Menus")]
    public Transform mainMenu;
    public Transform characterMenu;
    public Transform inventoryMenu;
    public Transform stanceMenu;
    public Dictionary<MenuType, Transform> menuDictionary;
    public enum MenuType { None, Main, Character, Inventory, Stance };
    public MenuType currentMenu;
    private Transform currentSelectionMenu = null;

    [Header("Buttons")]
    public GameObject buttonPrefab;
    private int selectionIndex;
    private bool canSelect;
    private List<Wreckless.UI.Button> buttons;

    #endregion

    #region Setup HUD

    //Called when the battle is started. It sets up the hud deleting any unneeded elements
    public void InitHUD(List<BattleCharacter> characters)
    {

        //Character Cards
        CharacterCards(characters);

        SetMenuDictionary();

        UpdateMenu(MenuType.None);

    }

    //Used to setup character cards, removing any uneeded ones and adding new ones
    private void CharacterCards(List<BattleCharacter> characters)
    {

        //Destroy existing character cards
        DeleteCards();

        SpawnCards(characters);

    }

    //Deletes all currently spawned in character cards
    private void DeleteCards()
    {

        if(characterCards != null && characterCards.Count > 0)
        {
            foreach(BaseCard card in characterCards)
            {
                card.Destroy();
            }
        }

        characterCards = new List<BaseCard>();

    }

    //Spawns in new character cards
    private void SpawnCards(List<BattleCharacter> characters)
    {

        foreach(BattleCharacter c in characters)
        {

            if(c is BattleCharacter_Party)
            {
                BaseCard card = Instantiate(partyCardPrefab, partyCardPanel).GetComponent<BaseCard>();
                card.InitCard(c);
                characterCards.Add(card);
            }
            else if(c is BattleCharacter_Enemy)
            {
                BaseCard card = Instantiate(enemyCardPrefab, battleHUD.transform).GetComponent<BaseCard>();
                card.InitCard(c);
                characterCards.Add(card);
            }

        }

    }

    //Setup the menu dictionary used for switching the menu
    public void SetMenuDictionary()
    {
        menuDictionary = new Dictionary<MenuType, Transform>();

        menuDictionary.Add(MenuType.Main, mainMenu);
        menuDictionary.Add(MenuType.Character, characterMenu);
        menuDictionary.Add(MenuType.Inventory, inventoryMenu);
        menuDictionary.Add(MenuType.Stance, stanceMenu);
    }

    #endregion

    #region Menus

    //Switches the menu and setups variables needed for button selection
    public void UpdateMenu(MenuType type)
    {

        foreach(MenuType menu in menuDictionary.Keys)
        {

            if(menu == type)
                menuDictionary[menu].gameObject.SetActive(true);
            else
                menuDictionary[menu].gameObject.SetActive(false);

        }

        Transform currentMenuTransform = null;

        if (menuDictionary.ContainsKey(type))
            currentMenuTransform = menuDictionary[type];



        currentMenu = type;
        currentSelectionMenu = currentMenuTransform;
        InitButtonSelection(currentMenuTransform);

    }

    #endregion

    #region Buttons
    //Deletes and spawns in buttons
    public IEnumerator SetupButtons<T>(T type)
    {

        if (type is List<BattleCharacter>)
            UpdateMenu(MenuType.Character);
        else if (type is Dictionary<Item, int>)
            UpdateMenu(MenuType.Inventory);

        DeleteButtons();

        yield return new WaitForEndOfFrame();

        SpawnButtons(type);

    }

    //Deletes all buttons that are children to a object
    private void DeleteButtons()
    {

        if (buttons != null && buttons.Count > 0)
        {
            if (currentMenu != MenuType.Main)
            {
                foreach (Wreckless.UI.Button button in buttons)
                {
                    Destroy(button.gameObject);
                }
            }
        }

        buttons = new List<Wreckless.UI.Button>();

    }

    //Spawns in buttons based on the type passed through
    private void SpawnButtons<T>(T type)
    {

        if (type is List<BattleCharacter>)
        {
            foreach(BattleCharacter c in (type as List<BattleCharacter>))
            {
                Transform obj = Instantiate(buttonPrefab, currentSelectionMenu).transform;
                obj.GetComponent<Wreckless.UI.Button>().SetButtonTxt(c.info.characterName);
                obj.GetComponent<Wreckless.UI.Button>().OnSelect.AddListener(delegate { StartCoroutine(BattleManager.Instance.DealDamage(c)); });
                buttons.Add(obj.GetComponent<Wreckless.UI.Button>());
            }
        }
        else if(type is Dictionary<Item, int>)
        {
            List<Item> items = new List<Item>((type as Dictionary<Item, int>).Keys);

            foreach(Item i in items)
            {
                Transform obj = Instantiate(buttonPrefab, currentSelectionMenu).transform;
                obj.GetComponent<Wreckless.UI.Button>().SetButtonTxt($"{i.itemName}  {(type as Dictionary<Item, int>)[i]}");
                obj.GetComponent<Wreckless.UI.Button>().OnSelect.AddListener(delegate { StartCoroutine(SetupButtons(BattleManager.Instance.GetPartyList())); });
                buttons.Add(obj.GetComponent<Wreckless.UI.Button>());
            }

        }

    }

    //Sets up the buttons for selection
    private void InitButtonSelection(Transform parent)
    {

        if (parent == null)
            return;

        buttons = parent.GetComponentsInChildren<Wreckless.UI.Button>().ToList();

        selectionIndex = 0;
        canSelect = true;

    }

    //Being called by the Update method, this method allows the player to
    //Select buttons
    private void SelectButton()
    {

        if (!canSelect)
            return;

        if (Input.GetKeyDown(KeyCode.W))
            selectionIndex--;
        else if (Input.GetKeyDown(KeyCode.S))
            selectionIndex++;

        if (selectionIndex < 0)
            selectionIndex = buttons.Count - 1;
        else if (selectionIndex == buttons.Count)
            selectionIndex = 0;

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].SetSelectorActive(false);
            if (i == selectionIndex)
                buttons[i].SetSelectorActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            canSelect = false;
            buttons[selectionIndex].Select();
        }

    }

    #endregion

    #region Character Cards

    //Deletes character card if enemy is killed
    public void DeleteCharacterCard(BattleCharacter character)
    {
        foreach (BaseCard card in characterCards)
        {
            if (card.GetCharacter() == character)
            {
                card.Destroy();
                return;
            }
        }
    }

    //Updates elements in character cards
    public void UpdateCharacterCard(BattleCharacter character)
    {

        foreach(BaseCard card in characterCards)
        {
            if(card.GetCharacter() == character)
            {
                card.UpdateCard();
                return;
            }
        }

    }

    //Adds status effect to character card
    public void AddCardEffects(BattleCharacter character, StatusEffect effect)
    {
        foreach (BaseCard card in characterCards)
        {
            if (card.GetCharacter() == character)
            {
                card.AddStatusEffect(effect);
                return;
            }
        }
    }

    //Removes status effect from character card
    public void RemoveCardEffects(BattleCharacter character, StatusEffect effect)
    {
        foreach (BaseCard card in characterCards)
        {
            if (card.GetCharacter() == character)
            {
                card.RemoveStatusEffect(effect);
                return;
            }
        }
    }

    #endregion

    #region Unity Methods

    private void Update()
    {

        if (battleHUD.activeSelf != BattleManager.Instance.InBattle)
            battleHUD.SetActive(BattleManager.Instance.InBattle);

        if (!BattleManager.Instance.InBattle)
            return;

        SelectButton();

    }

    private void Awake()
    {
        Instance = this;
    }
    #endregion
}
