using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class Main : MonoBehaviour
{
    public static Camera cam;
    [SerializeField] private List<CursorState> cursorStates;
    [SerializeField] private List<GoToButton> goToButtons;
    [SerializeField] private GameObject goToButtonsStart;

    [SerializeField] private TMP_Text reputationText;

    [SerializeField] private int maxPoorServices;
    
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
    
    public static Interactable touchingInteractable = null;

    [SerializeField] private string startGoToPlace = "pc";
    
    public float difficultyMultiplier = 1f;

    public Qualities qualities;

    public float money = 0;

    public bool receivedBox = false;

    public Stages stages;

    public float maxEnergy;

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


    public void GiveMoney(float amount)
    {
        money += amount;
    }
    
    public void UpdateReputationText()
    {
        reputationText.text = "reputation: " + reputation;
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
    
    public IEnumerator GoTo(string place)
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
        ShowGoToButtons();
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
        StartCoroutine(GoTo(startGoToPlace));
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
        UpdateInteractables();
    }
}
