using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public class Workplace : Place
{
    [SerializeField] private int toolsPerTurn;
    [SerializeField] private List<LaptopTarget> targetsPrefabs;
    [SerializeField] private List<Laptop> laptopsPrefabs;
    
    [SerializeField] private List<Tool>  toolsPrefabs;

    [SerializeField] private Interactable endTurnButton;
    [SerializeField] private Interactable finishServiceButton;
    public List<Tool> tools;
    public Laptop currentLaptop;
    
    public LaptopTarget touchingTarget;

    public Tip tip;

    [SerializeField] private SpriteRenderer highlight;

    public GameObject hand;

    public Tool draggingTool;

    private void ShowButtons()
    {
        endTurnButton.transform.DOKill();
        finishServiceButton.transform.DOKill();

        endTurnButton.active = true;
        finishServiceButton.active = true;

        endTurnButton.transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
        finishServiceButton.transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
    }

    private void HideButtons()
    {
        endTurnButton.active = false;
        finishServiceButton.active = false;
        
        endTurnButton.transform.DOKill();
        finishServiceButton.transform.DOKill();
        
        endTurnButton.transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo);
        finishServiceButton.transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo);
    }

    public void FinishService()
    {
        HideButtons();
        Main.obj.ShowGoToButtons();
    }

    public IEnumerator EndTurn()
    {
        foreach (Tool tool in GetHandTools())
        {
            tool.RemoveFromScreen();
            tool.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);
        }
        yield return new WaitForSeconds(0.5f);

        currentLaptop.DoTurn();
        
        foreach (Tool tool in tools)
        {
            if (tool.toolName == "hand")
            {
                //tool.usesLeft = tool.maxUses;
                tool.UpdateUsesLeftText();
                tool.GiveInHand();
            }
        }
        
        GiveToolsHand();
    }
    
    public override void OnEnter()
    {
        if (Main.currentTask != null)
        {
            if (!currentLaptop)
            {
                GenerateLaptop();
            }
            Main.obj.HideGoToButtons();

            foreach (Tool tool in tools)
            {
                if (tool.toolName == "hand")
                {
                    tool.usesLeft = tool.maxUses;
                    tool.UpdateUsesLeftText();
                    tool.GiveInHand();
                }
            }
        
            GiveToolsHand();
            
            ShowButtons();
        }
    }

    public void GiveTool(string toolName)
    {
        Tool tool = Instantiate(toolsPrefabs.Find(t => t.toolName == toolName), hand.transform);
        tools.Add(tool);
        Main.interactables.Add(tool);
    }

    private List<Tool> GetHandTools()
    {
        List<Tool> result = new();
        foreach (Tool t in tools)
        {
            if (t.inHand) result.Add(t);
        }
        return result;
    }

    public void UpdateToolsHand()
    {
        int count = 0;
        foreach (Tool t in GetHandTools())
        {
            t.id = count;
            t.PlaceInHand();
            count++;
        }
    }
    
    private void GiveToolsHand()
    {
        for (int i = 0; i < Mathf.CeilToInt(tools.Count / 2f); i++)
        {
            tools[Random.Range(0, tools.Count)].GiveInHand();
        }
    }

    private void GenerateLaptop()
    {
        currentLaptop = Instantiate(laptopsPrefabs[Random.Range(0, laptopsPrefabs.Count)], transform);
        int amountOfTargets = Random.Range(Mathf.RoundToInt(3 + Main.currentTask.difficulty/2f), Mathf.RoundToInt(5 + Main.currentTask.difficulty/2f));
        for (int j = 0; j < amountOfTargets; j++)
        {
            for (int i = 0; i < 1024; i ++)
            {
                bool noSpace = false;
                LaptopTarget t = targetsPrefabs[Random.Range(0, targetsPrefabs.Count)];
                if (t.difficulty <= Main.currentTask.difficulty && t.difficulty > Main.currentTask.difficulty - 2)
                {
                    LaptopTarget target = Instantiate(t, transform);
                    for (int k = 0; k < 128; k++)
                    {
                        if (k == 127)
                        {
                            noSpace = true;
                            break;
                        }
                        target.transform.localPosition = new Vector3(Random.Range(-150, 150)/100f, Random.Range(-150, 150)/100f, 0);
                        bool touches = false;
                        foreach (LaptopTarget t2 in currentLaptop.targets)
                        {
                            if (Vector2.Distance(t2.transform.position, target.transform.position) < target.size)
                            {
                                touches = true;
                                break;
                            }
                        }

                        if (touches) continue;
                        break;
                    }

                    if (noSpace)
                    {
                        Destroy(target.gameObject);
                        break;
                    }
                    currentLaptop.targets.Add(target);
                    break;
                }
            }
        }
    }

    private void OnHoverTarget(LaptopTarget t)
    {
        highlight.DOKill();
        highlight.transform.DOKill();
        highlight.DOFade(1, 0.5f);
        highlight.transform.DOMove(t.transform.position, 0.5f).SetEase(Ease.OutExpo);
        tip.ShowTip(t.tip, t.transform.position);
    }

    private void OnHoverExitTarget()
    {
        highlight.DOKill();
        highlight.DOFade(0, 0.5f);
        if (!GetHandTools().Find(t => t.touching))tip.HideTip();
    }

    private void Update()
    {
        if (!currentLaptop) return;
        Vector2 mouse = Main.cam.ScreenToWorldPoint(Input.mousePosition);
        LaptopTarget nearestTarget = null;
        float nearestDistance = float.MaxValue;
        foreach (LaptopTarget target in currentLaptop.targets)
        {
            float distance = Vector2.Distance(mouse, target.transform.position);
            if (distance < target.size && distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = target;
            }
        }

        if (nearestTarget)
        {
            if (touchingTarget != nearestTarget)
            {
                touchingTarget = nearestTarget;
                OnHoverTarget(nearestTarget);
            }
        }
        else
        {
            OnHoverExitTarget();
            touchingTarget = null;
        }
    }
}
