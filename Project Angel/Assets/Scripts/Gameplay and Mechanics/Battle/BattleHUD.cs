using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{

    #region Variables

    public static BattleHUD Instance;

    public GameObject battleHUD;

    public EventSystem eventSystem;

    [Header("Character Cards")]
    public GameObject partyCardPrefab;
    public Transform partyCardPanel;
    public GameObject enemyCardPrefab;
    private List<BaseCard> characterCards;

    [Header("Selection Menus")]
    public Transform mainMenu;
    public Transform inventoryMenu;
    public Transform stanceMenu;
    public Dictionary<MenuType, Transform> menuDictionary;
    public enum MenuType { None, Main, Inventory, Stance };
    public MenuType currentMenu;
    private Transform currentSelectionMenu = null;

    [Header("Buttons")]
    public GameObject buttonPrefab;
    private int selectionIndex;
    private bool canSelect;
    private List<Button> buttons;
    private GameObject lastSelectedButton;

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
        StartCoroutine(InitButtonSelection(currentMenuTransform));

    }

    #endregion

    #region Buttons
    //Deletes and spawns in buttons
    public IEnumerator SetupButtons<T>(T type)
    {

        if (type is Dictionary<Item, int>)
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
                foreach (Button button in buttons)
                {
                    Destroy(button.gameObject);
                }
            }
        }

        buttons = new List<Button>();

    }

    //Spawns in buttons based on the type passed through
    private void SpawnButtons<T>(T type)
    {
        
        if(type is Dictionary<Item, int>)
        {
            List<Item> items = new List<Item>((type as Dictionary<Item, int>).Keys);

            foreach(Item i in items)
            {
                Transform obj = Instantiate(buttonPrefab, currentSelectionMenu).transform;
                obj.GetChild(0).GetComponent<TextMeshProUGUI>().text = ($"{i.itemName}  {(type as Dictionary<Item, int>)[i]}");
                obj.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(SetupButtons(BattleManager.Instance.GetPartyList())); });
                buttons.Add(obj.GetComponent<Button>());
            }

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

    private IEnumerator InitButtonSelection(Transform currentMenu)
    {
        if (currentMenu != null)
        {
            eventSystem.SetSelectedGameObject(null);
            yield return new WaitForEndOfFrame();
            eventSystem.SetSelectedGameObject(currentMenu.GetChild(0).gameObject);
        }
    }

    #endregion

    #region Unity Methods

    private void Update()
    {

        if (battleHUD.activeSelf != BattleManager.Instance.InBattle)
            battleHUD.SetActive(BattleManager.Instance.InBattle);

        if (eventSystem.currentSelectedGameObject != null)
            lastSelectedButton = eventSystem.currentSelectedGameObject;
        else
            eventSystem.SetSelectedGameObject(lastSelectedButton);

    }

    private void Start()
    {
        if (eventSystem == null)
            eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Awake()
    {
        Instance = this;
    }
    #endregion
}
