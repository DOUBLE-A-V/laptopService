using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private SpriteRenderer highlight;
    [SerializeField] public Message message;

    [SerializeField] private Wallet wallet;
    [SerializeField] private InfoSheet infoSheet;
    [SerializeField] private InfoSticker gotoTip;

    [SerializeField] private WorkplaceButton restButton;
    [SerializeField] private WorkplaceButton finishService;
    
    public bool rested = false;

    public bool closedServiceReport = false;
    
    private float sincounter = 0;
    private bool animateHighlight = false;

    public bool completed = false;

    public void Highlight(Vector3 pos, Vector2 rect)
    {
        highlight.transform.position = pos;
        highlight.transform.localScale = rect;
        animateHighlight = true;
    }

    public void HideHighlight()
    {
        animateHighlight = false;
        highlight.DOFade(0, 0.5f);
    }

    public void ShowText(string content, Vector2 pos)
    {
        message.Show(content);
        message.transform.DOMove(pos, 0.5f).SetEase(Ease.OutExpo);
    }

    public IEnumerator StartTutorial()
    {
        gotoTip.display = false;
        wallet.display = false;
        infoSheet.display = false;
        
        Main.obj.HideGoToButtons();
        yield return new WaitForSeconds(1f);
        ShowText("Good morning, happy owner of the laptop service.", Vector3.zero);
        Main.obj.noUpdateInteractablesTimer = 9999999;
        while (message.active) yield return null;
        Main.obj.noUpdateInteractablesTimer = 0;
        Highlight(new Vector2(2.2f, -1.75f), Vector2.one);
        message.canSkip = false;
        ShowText("Turn on the pc to check your email.", Vector3.zero);
        while (Main.obj.pcManager.blocked == false) yield return null;
        Main.obj.pcManager.pcButton.active = false;
        message.Hide();
        HideHighlight();
        for (int i = 0; i < 90; i++)
        {
            Main.obj.pcManager.pcButton.active = false;
            yield return new WaitForSeconds(0.1f);
        }
        Main.obj.pcManager.pcButton.active = false;
        ShowText("Read new message.", new Vector3(0, -1, 0));
        Highlight(new Vector3(0.47f, 2.1f, 0), new Vector3(0.7f, 0.5f, 0));
        while (Main.obj.pcManager.blocked == false)
        {
            Main.obj.pcManager.pcButton.active = false;
            yield return null;
        }
        message.Hide();
        HideHighlight();
        yield return new WaitForSeconds(1f);
        message.canSkip = true;
        ShowText("This is your new client.", new Vector3(0, -1, 0));
        while (message.active)  yield return null;
        message.canSkip = false;
        ShowText("Shutdown the PC.", new Vector3(0, -1, 0));
        Main.obj.pcManager.pcButton.active = true;
        Highlight(new Vector2(2.2f, -1.75f), Vector2.one);
        while (Main.obj.pcManager.working) yield return null;
        Main.obj.HideGoToButtons();
        gotoTip.display = true;
        HideHighlight();
        message.Hide();
        yield return new WaitForSeconds(1f);
        ShowText("Hover mouse on this sticker.", Vector3.zero);
        Highlight(new Vector3(-6, 4.4f, 0), new Vector2(4, 0.6f));
        while (!gotoTip.opened)  yield return null;
        message.Hide();
        HideHighlight();
        message.canSkip = true;
        ShowText("There will always be a hint here about what to do.", Vector3.zero);
        while (message.active) yield return null;
        Main.obj.ShowGoToButtons();
        while (Main.obj.currentPlace != Main.obj.workplace || !Main.obj.receivedBox) yield return null;
        message.canSkip = false;
        ShowText("drag tools onto targets to use them.", new Vector3(0, 1, 0));
        while (Main.obj.workplace.tools.FindAll(x => x.inHand).Count != 0) yield return null;
        message.Hide();
        ShowText("good work, now recover your energy by resting.",  new Vector3(0, 1, 0));
        restButton.transform.DOScale(Vector3.one, 0.5f);
        restButton.active = true;
        while (!rested)  yield return null;
        restButton.active = false;
        message.Hide();
        message.canSkip = true;
        Main.obj.noUpdateInteractablesTimer = 9999999;
        ShowText("every time you use a tool, it's durability decreases.",  new Vector3(0, 1, 0));
        while (message.active) yield return null;
        ShowText("when durability reaches zero tool will be permanently removed from your inventory.",  new Vector3(0, 1, 0));
        while (message.active) yield return null;
        ShowText("except your hands of course. Their durability is restored with every new laptop you service.", new Vector3(0, 1, 0));
        while (message.active) yield return null;
        ShowText("you also have a clock, hover mouse over it to see more info.", new Vector3(0, 1, 0));
        Main.obj.noUpdateInteractablesTimer = 0;
        while (message.active) yield return null;
        message.canSkip = false;
        ShowText("now use the tools again and complete the service.", new Vector3(0, 1, 0));
        while (Main.obj.workplace.tools.FindAll(x => x.inHand).Count != 0) yield return null;
        message.Hide();
        finishService.active = true;
        finishService.transform.DOScale(Vector3.one, 0.5f);
        while (!closedServiceReport) yield return null;
        message.canSkip = true;
        Main.obj.HideGoToButtons();
        ShowText("congratulations with your first serviced laptop!", Vector3.zero);
        wallet.display = true;
        infoSheet.display = true;
        while (message.active) yield return null;
        ShowText("hover your mouse over your wallet to see how much money you have.",  Vector3.zero);
        Highlight(new Vector3(-6.5f, -3.9f, 0), new Vector2(3, 1.5f));
        while (message.active) yield return null;
        ShowText("hover your mouse over this sheet to see info about your service.", Vector3.zero);
        Highlight(new Vector3(6.2f, 4.2f, 0), new Vector2(4, 1f));
        while (message.active) yield return null;
        HideHighlight();
        ShowText("Tutorial complete! Enjoy, don't forget to visit a shop.",  Vector3.zero);
        Main.obj.ShowGoToButtons();
        completed = true;
    }

    private void Update()
    {
        if (!animateHighlight) return;
        sincounter += Time.deltaTime*2;
        if (sincounter > 360)
        {
            sincounter = 0;
        }
        highlight.color = new Color(1, 1, 1, Mathf.Abs(Mathf.Sin(sincounter)));
    }
}
