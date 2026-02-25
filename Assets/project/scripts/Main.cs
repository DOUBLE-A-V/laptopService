using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using DG.Tweening;

public class Main : MonoBehaviour
{
    public static Camera cam;
    [SerializeField] private List<CursorState> cursorStates;
    [SerializeField] private List<GoToButton> goToButtons;
    [SerializeField] private GameObject goToButtonsStart;

    [SerializeField] private int maxPoorServices;
    
    public HighlightsManager highlightsManager;
    
    public int poorServices = 0;
    public int reputation;
    
    public List<Place> places;
    
    public static List<Interactable> interactables = new List<Interactable>();
    public List<ServiceTask> serviceTasks;
    public List<string> names;
    public static ServiceTask currentTask;
    
    public Blackscreen blackscreen;

    public static Main obj;

    public Place currentPlace;

    public PCManager pcManager;
    public PostMachine postMachine;
    public Workplace workplace;
    public Shop shop;

    public Tutorial tutorial;

    public Wallet wallet;

    public Tool poppyTool;
    
    public static Interactable touchingInteractable = null;

    [SerializeField] private string startGoToPlace = "pc";
    
    public float difficultyMultiplier = 1f;

    public Qualities qualities;

    public float money = 0;

    public bool receivedBox = false;

    public Stages stages;

    public float maxEnergy;

	public float noUpdateInteractablesTimer = 0;

    public TMP_Text messageTextPrefab;

    [Serializable]
    public class Stages
    {
        public PlayerStage gotoPc;
        public PlayerStage checkNewMessage;
        public PlayerStage disablePc;
        public PlayerStage gotoPost;
        public PlayerStage receiveBox;
        public PlayerStage gotoWorkplace;
        public PlayerStage useTools;
        public PlayerStage takeBreak;
        public PlayerStage sendBox;
        public PlayerStage gotoShop;
        public PlayerStage buyTool;
    }

    public PlayerStage currentStage;

    public int badServiceStamps = 0;
    public BadStampReceive badStampReceive;
    
    public InfoSheet infoSheet;

    [SerializeField] private TMP_Text loseText;
    [SerializeField] private TMP_Text pressButtonText;


    public void ShowMessage(string message, Vector3 pos)
    {
        TMP_Text t = Instantiate(messageTextPrefab);
        t.transform.position = pos;
        t.text = message;
        t.transform.DOMove(t.transform.position + new Vector3(0, 1, 0), 1f).SetEase(Ease.OutExpo);
        t.DOFade(0, 2f).SetEase(Ease.OutFlash);
        Destroy(t.gameObject, 2);
    }
    
    public void GiveMoney(float amount)
    {
        if (amount < 0)
        {
            wallet.Spend(-amount);
        }
        else
        {
            money += amount;
        }
    }

    public void HideGoToButtons()
    {
        foreach (GoToButton btn in goToButtons)
        {
            btn.Hide();
        }
    }

    public void ShowGoToButtons()
    {
        int count = 0;
        foreach (GoToButton btn in goToButtons)
        {
            if (currentPlace.placeName == btn.goToPlace) continue;
            btn.Show();
            btn.transform.localPosition = goToButtonsStart.transform.localPosition + new Vector3(0, 220 * count, 0);
            count++;
        }
    }
    
    public IEnumerator GoTo(string place, bool showGoToButtons = true)
    {
        if (place == "pc" && currentStage.id == stages.gotoPc.id)
        {
            currentStage = stages.checkNewMessage;
        } else if (place == "post" && currentStage.id == stages.gotoPost.id)
        {
            currentStage = workplace.finished ? stages.sendBox : stages.receiveBox;
        } else if (place == "workplace" && currentStage.id == stages.gotoWorkplace.id)
        {
            currentStage = stages.useTools;
        }

        obj.blackscreen.Show();
        HideGoToButtons();
        yield return new WaitForSeconds(1f);
        currentPlace.OnExit();
        currentPlace.gameObject.SetActive(false);
        currentPlace = places.Find(x => x.placeName == place);
        places.ForEach(x => x.gameObject.SetActive(x.placeName == place));
        obj.blackscreen.Hide();
        if (showGoToButtons) ShowGoToButtons();
        currentPlace.OnEnter();
    }
    
    [Serializable]
    public class CursorState
    {
        public string name;
        public Texture2D texture;
    }
    
    public static CursorState currentCursorState;

    private void Awake()
    {
        cam =  Camera.main;
        obj = this;
    }

    public void SetCursorState(string cursorState)
    {
        foreach (CursorState state in cursorStates)
        {
            if (state.name == cursorState)
            {
                Cursor.SetCursor(state.texture, new Vector2(0, 0), CursorMode.Auto);
                currentCursorState = state;
            }
        }
    }

    public IEnumerator OnLose()
    {
        workplace.FinishService();
        blackscreen.Show();
        noUpdateInteractablesTimer = 9999999;
        yield return new WaitForSeconds(1f);
        noUpdateInteractablesTimer = 9999999;
        loseText.DOFade(1, 1f);
        float sincount = 0;
        while (!Input.GetMouseButton(0) && !Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.Return))
        {
            sincount += Time.deltaTime * 2;
            if (sincount > 360) sincount = 0;
            pressButtonText.color = new Color(1, 1, 1, Mathf.Abs(Mathf.Sin(sincount)));
            yield return null;
        }

        pressButtonText.DOFade(0, 0.5f);
        loseText.DOKill();
        loseText.DOFade(0, 0.5f);
        noUpdateInteractablesTimer = 9999999;
        yield return new WaitForSeconds(1f);
        StartCoroutine(GoTo("pc", false));
        noUpdateInteractablesTimer = 9999999;
        foreach (Tool tool in workplace.tools)
        {
            Destroy(tool.gameObject);
        }
        workplace.tools.Clear();
        workplace.GiveTool("hand");
        workplace.GiveTool("hand");
        currentStage = stages.checkNewMessage;
        money = 0;
        badServiceStamps = 0;
        reputation = 0;
        currentTask = null;
        noUpdateInteractablesTimer = 0;
        workplace.finished = false;
        Destroy(workplace.currentLaptop.gameObject);
    }
    
    private void Start()
    {
        SetCursorState("normal");
        HideGoToButtons();
        ShowGoToButtons();
        
        currentTask = new ServiceTask(
            "t",
            "please fuck niggers",
            "John pidrton",
            100,
            200,
            150,
            0,
            85874538
            );
        
        workplace.GiveTool("hand");
        workplace.GiveTool("hand");
        StartCoroutine(GoTo(startGoToPlace, tutorial.completed));
        
        //GiveMoney(1000);
        if (!tutorial.completed) StartCoroutine(tutorial.StartTutorial());
    }

    public void DeactivateGoToButtons() 
    {
        foreach (GoToButton btn in goToButtons) btn.active = false;
    }

    public void ActivateGoToButtons()
    {
        foreach (GoToButton btn in goToButtons) btn.active = currentPlace.placeName != btn.goToPlace;
    }

    private void UpdateInteractables()
    {
        bool removed = true;
        while (removed)
        {
            removed = false;
            foreach (Interactable inter in interactables)
            {
                if (!inter.active && touchingInteractable == inter)
                {
                    touchingInteractable = null;
                    SetCursorState("normal");
                    inter.touching = false;
                    continue;
                }

                if (inter.UpdateInteracable())
                {
                    break;
                }
                removed = inter.removed;
                if (removed) break;
            }
        }
    }

    private void Update()
    {
		noUpdateInteractablesTimer -= Time.deltaTime;
        if (noUpdateInteractablesTimer <= 0) UpdateInteractables();
        if (Input.GetKeyDown(KeyCode.E))
        {
            shop.Upgrade();
        }
        
    }
}