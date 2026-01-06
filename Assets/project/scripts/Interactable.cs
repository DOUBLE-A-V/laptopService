
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private SpriteRenderer outline;
    private GameObject mask;
    [SerializeField] private BoxCollider2D collider;
    public bool active = true;
    public bool touching = false;
    
    public bool removed = false;

    protected Vector3 defaultScale;

    private void Awake()
    {
        defaultScale = transform.localScale;
        if (!Main.interactables.Contains(this)) Main.interactables.Add(this);
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
    
    public bool UpdateInteracable()
    {
        if (collider.OverlapPoint(Main.cam.ScreenToWorldPoint(Input.mousePosition)) && active)
        {
            if (!touching) OnHover();
            touching = true;
            if (Main.touchingInteractable != this)
            {
                Main.obj.SetCursorState("interactive");
                Main.touchingInteractable = this;
            }
        }
        else
        {
            if (Main.touchingInteractable == this)
            {
                Main.obj.SetCursorState("normal");
                Main.touchingInteractable = null;
            }
            if (touching)
            {
                OnHoverExit();
            }
            touching = false;
        }

        if (touching && Input.GetMouseButtonDown(0) && active)
        {
            Main.touchingInteractable = this;
            Interact();
            return true;
        }
        OnUpdateInteractable();
        return false;
    }
}
