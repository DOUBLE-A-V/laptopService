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

    private int maxUses;

    public int id;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        maxUses = usesLeft;
    }

    private void UpdateUsesLeftText()
    {
        usesText.text = usesLeft + " / " + maxUses;
    }

    private void OnDrop()
    {
        Main.obj.workplace.draggingTool = null;
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
            transform.DOMove(Main.cam.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10), 0.2f).SetEase(Ease.OutExpo);
            Main.obj.workplace.tip.HideTip();
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
        transform.DOScale(0, 1f).SetEase(Ease.OutElastic, 0.5f);
        inHand = false;
    }

    public void PlaceInHand()
    {
        active = true;
        transform.DOKill();
        transform.DOLocalMove(new Vector3(0 + (id - (Main.obj.workplace.tools.Count - 1) / 2.0f) * size.x, 0, 0), 0.5f).SetEase(Ease.OutExpo);
    }
    
    public void BreakTool(LaptopTarget against)
    {
        transform.DOKill();
        transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo);
        Main.obj.workplace.tools.Remove(this);
        Main.obj.workplace.UpdateToolsHand();
        OnBreakTool(against);
        Destroy(gameObject, 0.5f);
    }

    public void GiveInHand()
    {
        inHand = true;
        spriteRenderer.DOKill();
        spriteRenderer.DOFade(1, 0.5f);
        Main.obj.workplace.UpdateToolsHand();
    }

    public IEnumerator Use(LaptopTarget target)
    {
        transform.DOKill();
        transform.localScale = Vector3.one * 0.9f;
        transform.DOScale(1, 1f).SetEase(Ease.OutElastic, 0.5f);
        OnUse(target);
        
        yield return new WaitForSeconds(1.5f);
        
        usesLeft -= 1;
        if (usesLeft <= 0)
        {
            BreakTool(target);
        }
        else
        {
            RemoveFromScreen();
        }
    }
}
