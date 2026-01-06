using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR;
using System.Collections;

public class Main : MonoBehaviour
{
    public static Camera cam;
    [SerializeField] private List<CursorState> cursorStates;
    [SerializeField] private List<GoToButton> goToButtons;
    [SerializeField] private GameObject goToButtonsStart;
    
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
    
    public static Interactable touchingInteractable = null;

    [SerializeField] private string startGoToPlace = "pc";

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
            btn.transform.localPosition = goToButtonsStart.transform.localPosition + new Vector3(0, 150 * count, 0);
            count++;
        }
    }
    
    public IEnumerator GoTo(string place)
    {
        obj.blackscreen.Show();
        HideGoToButtons();
        yield return new WaitForSeconds(1f);
        currentPlace = places.Find(x => x.placeName == place);
        places.ForEach(x => x.gameObject.SetActive(x.placeName == place));
        currentPlace.OnEnter();
        obj.blackscreen.Hide();
        ShowGoToButtons();
    }
    
    [System.Serializable]
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
        StartCoroutine(GoTo(startGoToPlace));
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
