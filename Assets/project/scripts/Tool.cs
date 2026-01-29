using DG.Tweening;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class Tool : Interactable
{
    public string toolName;
    public int usesLeft;
    public int globalDamage;
    public bool inHand;

    [SerializeField] private Vector2 size;
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TMP_Text usesText;

    public TipDescriptor tip;

    private bool dragging;

    public int maxUses;

    public int id;

    public int energyCost;

    [SerializeField] protected bool overrideBreakLogic = false;
    
    protected List<LaptopTarget> hitTargets = new List<LaptopTarget>();

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        maxUses = usesLeft;
        tip.maxUses = maxUses;
        tip.usesLeft = usesLeft;
    }

    public virtual void CheckCollision()
    {
        
    }

    protected virtual void OnUp()
    {
        
    }

    protected virtual void OnDown()
    {
        
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
        OnDown();
        Main.obj.workplace.draggingTool = null;
        dragging = false;
        
        foreach (LaptopTarget target in hitTargets)
        {
            target.ApplyTool(this);
        }
        if (hitTargets.Count == 0)
        {
            PlaceInHand();
        }
        else
        {
            active = false;
            transform.DOKill();
            transform.localScale = Vector3.one * 0.9f;
            transform.DOScale(1, 1f).SetEase(Ease.OutElastic, 0.5f);
            //if (globalDamage != 0) target.Damage(globalDamage, this);
            usesLeft--;
            UpdateUsesLeftText();
            Main.obj.workplace.energyBar.Change(-energyCost);
            if (usesLeft <= 0)
            {
                BreakTool(hitTargets);
                return;
            }
            StartCoroutine(AfterAnim());
            hitTargets.Clear();
        }
        for (int i = 0; i < 10; i++)
        {
            Main.obj.highlightsManager.RemoveLine(i);
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
                OnUp();
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
                CheckCollision();
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

    protected virtual void OnBreakTool(List<LaptopTarget> against)
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
    
    public void BreakTool(List<LaptopTarget> against)
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



    private IEnumerator AfterAnim()
    {
        yield return new WaitForSeconds(0.7f);
        RemoveFromScreen();
    }    
    public void Use(LaptopTarget target)
    {
        OnUse(target);
    }
}