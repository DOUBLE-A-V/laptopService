using UnityEngine;
using System.Collections;
using DG.Tweening;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private SpriteRenderer highlight;
    [SerializeField] private Message message;

    [SerializeField] private Wallet wallet;
    [SerializeField] private InfoSheet infoSheet;
    [SerializeField] private InfoSticker gotoTip;
    
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
        yield return new WaitForSeconds(9f);
        ShowText("Read new message.", new Vector3(0, -1, 0));
        Highlight(new Vector3(0.47f, 2.1f, 0), new Vector3(0.7f, 0.5f, 0));
        while (Main.obj.pcManager.blocked == false) yield return null;
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
        ShowText("There always will be tip about what to do at the moment.", Vector3.zero);
        while (message.active) yield return null;
        Main.obj.ShowGoToButtons();
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
