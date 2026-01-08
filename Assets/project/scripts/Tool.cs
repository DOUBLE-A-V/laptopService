using DG.Tweening;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Analytics;

public class Tool : Interactable
{
    public string toolName;
    public int usesLeft;
    public int globalDamage;
    public bool inHand;

    [SerializeField] private Vector2 size;
    
    private SpriteRenderer spriteRenderer;
    [SerializeField] private TMP_Text usesText;

    public TipDescriptor tip;

    private bool dragging;

    public int maxUses;

    public int id;

    [SerializeField] protected bool overrideBreakLogic = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        maxUses = usesLeft;
        tip.maxUses = maxUses;
        tip.usesLeft = usesLeft;
    }
    
    public void UpdateUsesLeftText()
    {
        if (usesLeft >= 0)
        {
            usesText.color = Color.white;
            usesText.text = usesLeft + " / " + maxUses;
        }
        else usesText.color = new  Color(1, 1, 1, 0);
        tip.usesLeft = usesLeft;
    }

    private void OnDrop()
    {
        Main.obj.workplace.draggingTool = null;
        dragging = false;

        if (Main.obj.workplace.touchingTarget)
        {
            Main.obj.workplace.touchingTarget.ApplyTool(this);
        }
        else
        {
            PlaceInHand();
        }
    }

    protected override void OnUpdateInteractable()
    {
        if (touching)
        {
            if (Input.GetMouseButton(0) && !Main.obj.workplace.draggingTool)
            {
                Main.obj.workplace.draggingTool = this;
                dragging = true;
            }
        }

        if (dragging)
        {
            if (!Input.GetMouseButton(0))
            {
                OnDrop();
            }
            else
            {
                transform.DOMove(Main.cam.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10), 0.2f).SetEase(Ease.OutExpo);
                Main.obj.workplace.tip.HideTip();
            }
        }
    }

    protected override void OnHoverExit()
    {
        if (!Main.obj.workplace.draggingTool)Main.obj.workplace.tip.HideTip();
    }

    protected override void OnHover()
    {
        if (!Main.obj.workplace.draggingTool)Main.obj.workplace.tip.ShowTip(tip, transform.position);
    }

    protected virtual void OnUse(LaptopTarget target)
    {
        
    }

    protected virtual void OnBreakTool(LaptopTarget against)
    {
        
    }

    public void RemoveFromScreen()
    {
        active = false;
        transform.DOKill();
        transform.DOScale(0, 1f).SetEase(Ease.OutExpo);
        inHand = false;
    }

    public void PlaceInHand()
    {
        active = true;
        transform.DOKill();
        transform.DOLocalMove(new Vector3(0 + (id - (Main.obj.workplace.tools.Count - 1) / 2.0f) * (size.x + 0.20f), 0, 0), 0.5f).SetEase(Ease.OutExpo);
        transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
        UpdateUsesLeftText();
    }
    
    public void BreakTool(LaptopTarget against)
    {
        if (overrideBreakLogic)
        {
            OnBreakTool(against);
        }
        else
        {
            transform.DOKill();
            transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo);
            Main.obj.workplace.tools.Remove(this);
            Main.obj.workplace.UpdateToolsHand();
            OnBreakTool(against);
            Destroy(gameObject, 0.5f);
        }
    }

    public void GiveInHand()
    {
        inHand = true;
        active = true;
        spriteRenderer.DOKill();
        spriteRenderer.DOFade(1, 0.5f);
        Main.obj.workplace.UpdateToolsHand();
    }



    private IEnumerator AfterAnim(LaptopTarget target)
    {
        yield return new WaitForSeconds(0.7f);
        RemoveFromScreen();
    }    
    public void Use(LaptopTarget target)
    {
        active = false;
        transform.DOKill();
        transform.localScale = Vector3.one * 0.9f;
        transform.DOScale(1, 1f).SetEase(Ease.OutElastic, 0.5f);
        //if (globalDamage != 0) target.Damage(globalDamage, this);
        usesLeft--;
        UpdateUsesLeftText();
        OnUse(target);
        if (usesLeft <= 0)
        {
            BreakTool(target);
            return;
        }
        StartCoroutine(AfterAnim(target));
    }
}
