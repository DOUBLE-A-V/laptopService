using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using TMPro;
using Random = UnityEngine.Random;
using Mathf = UnityEngine.Mathf;

public class Workplace : Place
{
    [SerializeField] private int toolsPerTurn;
    [SerializeField] public List<LaptopTarget> targetsPrefabs;
    [SerializeField] private List<Laptop> laptopsPrefabs;
    
    [SerializeField] private List<Tool>  toolsPrefabs;

    [SerializeField] private Interactable endTurnButton;
    [SerializeField] private Interactable finishServiceButton;

    [SerializeField] private string garrantedTargetDebug;
    [SerializeField] private string garrantedToolDebug;

    [SerializeField] public Clock clock;
    public List<Tool> tools;
    public Laptop currentLaptop;
    
    public LaptopTarget touchingTarget;

    public Tip tip;

    [SerializeField] private SpriteRenderer highlight;

    public GameObject hand;

    public Tool draggingTool;

    [SerializeField] private TMP_Text qualityText;
    [SerializeField] private TMP_Text qualityChangeText;

    [SerializeField] private GameObject bgNormal;
    [SerializeField] private GameObject bgBlur;

    public ProgressBar energyBar;

    public bool finished = false;

    [SerializeField] private Laptop finalLaptopPrefab;

    public bool isFinalLaptop = false;

    public void UpdateQualityText(bool animate=true)
    {
        qualityText.text = "quality: " + (currentLaptop.quality < 50 ? "<color=#ff0000>" : "<color=#00ff00>") + currentLaptop.quality + "%" +
                           (currentLaptop.quality < 50 ? "</color><color=#ffffff><size=20>\n - if quality is less than 50%\nyou will get 'poor service' stamp</size>" : "") + "</color>";
        if (animate)
        {
            qualityText.transform.DOKill();
            qualityText.transform.localScale = Vector3.one*0.9f;
            qualityText.transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
        }
    }

    public void ChangeQuality(int amount)
    {
        if (amount == 0) return;
        qualityChangeText.transform.DOKill();
        qualityChangeText.DOKill();
        qualityChangeText.text = amount + "%";
        if (amount > 0)
        {
            qualityChangeText.text = "+" + amount + "%";
            qualityChangeText.color = Color.green;
        }
        else
        {
            qualityChangeText.color = Color.red;
        }

        qualityChangeText.transform.localPosition = qualityText.transform.localPosition;
        qualityChangeText.transform.DOLocalMove(qualityChangeText.transform.localPosition - new Vector3(0, 80, 0), 2f).SetEase(Ease.OutExpo);
        qualityChangeText.DOFade(0, 2f).SetEase(Ease.InExpo);
        currentLaptop.quality += amount;
        UpdateQualityText();
    }
    
    private void ShowButtons()
    {
        endTurnButton.transform.DOKill();
        finishServiceButton.transform.DOKill();

        endTurnButton.active = true;
        finishServiceButton.active = true;

        endTurnButton.transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
        finishServiceButton.transform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
    }

    public void HideButtons()
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
        OnHoverExitTarget();
        tip.HideTip();
        Main.obj.currentStage = Main.obj.stages.gotoPost;
        energyBar.transform.DOKill();
        energyBar.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);

        clock.transform.DOKill();
        clock.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);
        
        qualityText.transform.DOKill();
        qualityText.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);
        
        HideButtons();
        foreach (Tool tool in tools)
        {
            tool.RemoveFromScreen();
        }
        Main.obj.ShowGoToButtons();
        
        Main.obj.shop.UpdateAllItems();
        
        finished = true;
        OnHoverExitTarget();
        tip.HideTip();
    }

    public IEnumerator EndTurn()
    {
        OnHoverExitTarget();
        tip.HideTip();
		Main.obj.noUpdateInteractablesTimer = 1.5f;
		StartCoroutine(clock.ChangeTime(Random.Range(1, 350)/100f, 0.5f));
        Main.obj.blackscreen.Show();
        foreach (Tool tool in GetHandTools())
        {
            tool.RemoveFromScreen();
            tool.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);
        }
        yield return new WaitForSeconds(1f);
        if (!Main.obj.tutorial.completed) Main.obj.tutorial.rested = true;
        energyBar.Set(energyBar.maxValue);
        foreach (LaptopTarget target in currentLaptop.targets) target.OnEndTurn();
        Main.obj.blackscreen.Hide();

        currentLaptop.DoTurn();
        
        foreach (Tool tool in tools)
        {
            if (tool.toolName == "hand" && tool.usesLeft > 0)
            {
                tool.UpdateUsesLeftText();
                tool.GiveInHand();
            }
        }
        
        GiveToolsHand();
    }

    public override void OnExit()
    {
        qualityText.gameObject.SetActive(false);
        energyBar.gameObject.SetActive(false);
        clock.gameObject.SetActive(false);
        Main.obj.globalProgressBar.Show();
    }
    
    public override void OnEnter()
    {
        Main.obj.globalProgressBar.Hide();
		bgNormal.SetActive(true);
		bgBlur.SetActive(false);
        clock.gameObject.SetActive(false);
        energyBar.gameObject.SetActive(false);
        if (Main.currentTask != null && Main.obj.receivedBox)
        {
            clock.gameObject.SetActive(true);
            clock.transform.DOScale(Vector3.one, 0.5f);
            clock.ResetClock(20, 00);
            energyBar.transform.localScale = Vector3.one;
			bgNormal.SetActive(false);
			bgBlur.SetActive(true);
            qualityText.gameObject.SetActive(true);
            energyBar.maxValue = Main.obj.maxEnergy;
            energyBar.Set(Main.obj.maxEnergy);
            if (!finished)
            {
                if (garrantedToolDebug != "") GiveTool(garrantedToolDebug);
                energyBar.gameObject.SetActive(true);
                if (!currentLaptop)
                {
                    finished = false;
                    GenerateLaptop();
                }
                
                Main.obj.HideGoToButtons();
            
                UpdateQualityText();
                
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
            
                if (Main.obj.tutorial.completed) ShowButtons();
            }
        }
    }

    public void GiveTool(string toolName)
    {
        Tool tool = Instantiate(toolsPrefabs.Find(t => t.toolName == toolName), hand.transform);

        tool.transform.localScale = Vector3.zero;
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
        for (int i = 0; i < tools.Count; i++)
        {
            List<Tool> tmp = tools.FindAll(t => !t.inHand && t.usesLeft > 0);
            if (tmp.Count == 0) break;
            tmp[Random.Range(0, tmp.Count)].GiveInHand();
        }
    }

    public void ChangeTime(float hours, float duration)
    {
        StartCoroutine(clock.ChangeTime(hours, duration));
    }

    private void RegenerateLaptop()
    {
        Destroy(currentLaptop.gameObject);
        GenerateLaptop();
    }

    private void GenerateLaptop()
    {
        int tmpRep = Main.obj.reputation;
        if (Main.obj.reputation == 0) tmpRep = 1;
        int tier1Chance = Mathf.RoundToInt(100 / (tmpRep / 10f));
        int tier2Chance = Mathf.RoundToInt((tmpRep - 100) * (450 - tmpRep)/200f);
        int tier3Chance = Mathf.RoundToInt((tmpRep - 400) * (700 - tmpRep) / 300f);

        if (tier2Chance < 10 && Main.obj.reputation > 200)
        {
            tier2Chance = 10;
        }
        
        tier2Chance = Mathf.Clamp(tier2Chance, 0, 100);
        tier3Chance = Mathf.Clamp(tier3Chance, 0, 100);

        isFinalLaptop = Main.obj.reputation == 600;
        
        currentLaptop = Instantiate(isFinalLaptop ? finalLaptopPrefab : laptopsPrefabs[Random.Range(0, laptopsPrefabs.Count)], transform);
        if (isFinalLaptop) energyBar.maxValue = Main.obj.maxEnergy + 10;
        energyBar.Set(energyBar.maxValue);
        int amountOfTargets = Random.Range(Mathf.RoundToInt((5 + Main.obj.reputation / 50f) / ((tier2Chance == 0
            ? 1
            : tier2Chance / 30f) + (tier3Chance == 0 ? 1 : tier3Chance/20f))), Mathf.RoundToInt(
            (7 + Main.obj.reputation / 50f) / ((tier2Chance == 0 ? 1 : tier2Chance / 30f) +
                                               (tier3Chance == 0 ? 1 : tier3Chance / 20f))));
        bool was = false;
        if (Main.obj.tutorial.completed)
        {
            for (int j = 0; j < amountOfTargets + (isFinalLaptop ? 3 : 0); j++)
            {
                int tmpTier = 0;
                if (Random.Range(0, 100) <= tier3Chance)
                {
                    tmpTier = 2;
                } else if (Random.Range(0, 100) <= tier2Chance)
                {
                    tmpTier = 1;
                } else if (Random.Range(0, 100) <= tier1Chance)
                {
                    tmpTier = 0;
                }
                List<LaptopTarget> candidates = targetsPrefabs.FindAll(x => x.difficulty == tmpTier || x.targetName == garrantedTargetDebug);
                bool noSpace = false;
                LaptopTarget t = candidates[Random.Range(0, candidates.Count)];
                if (!was && garrantedTargetDebug != "")
                {
                    was = true;
                    t = candidates.Find(x => x.targetName == garrantedTargetDebug);
                }
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
                }
                currentLaptop.targets.Add(target);
            }
        }
        else
        {
            LaptopTarget target = Instantiate(targetsPrefabs.Find(x => x.targetName == "scratches"), transform);
            for (int k = 0; k < 128; k++)
            {
                if (k == 127)
                {
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
            currentLaptop.targets.Add(target);
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
        if (!GetHandTools().Find(t => t.touching) && !clock.touching)tip.HideTip();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RegenerateLaptop();
        }
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
