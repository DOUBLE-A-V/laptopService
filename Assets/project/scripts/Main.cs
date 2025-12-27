using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR;
using System.Collections;

public class Main : MonoBehaviour
{
    public static Camera cam;
    [SerializeField] private List<CursorState> cursorStates;
    public static List<Interactable> interactables = new List<Interactable>();

    public static Main obj;

    public PCManager pcManager;

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
    }

    private void UpdateInteractables()
    {
        bool removed = true;
        while (removed)
        {
            removed = false;
            foreach (Interactable inter in interactables)
            {
                if (!inter.active) continue;
                inter.UpdateInteracable();
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
