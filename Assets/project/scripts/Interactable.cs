
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private SpriteRenderer outline;
    private GameObject mask;
    [SerializeField] private BoxCollider2D collider;
    public bool active = true;
    public bool touching = false;
    
    public bool removed = false;

    private void Awake()
    {
        Main.interactables.Add(this);
    }
    
    protected virtual void OnUpdateInteractable()
    {
        
    }

    protected virtual void Interact()
    {
        
    }

    protected virtual void OnHover()
    {
        
    }

    protected virtual void OnHoverExit()
    {
        
    }
    
    public void UpdateInteracable()
    {
        if (collider.bounds.Contains(Input.mousePosition) && active)
        {
            if (!touching) OnHover();
            touching = true;
            Main.obj.SetCursorState("interactive");
        }
        else
        {
            if (touching)
            {
                Main.obj.SetCursorState("normal");
                OnHoverExit();
            }
            touching = false;
        }

        if (touching && Input.GetMouseButtonDown(0) && active)
        {
            Interact();
        }
        OnUpdateInteractable();
    }
}
