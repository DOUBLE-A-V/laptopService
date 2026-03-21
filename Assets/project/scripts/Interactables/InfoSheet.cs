using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Vector3 = UnityEngine.Vector3;
using System.Collections.Generic;

public class InfoSheet : Interactable
{
    [SerializeField] private TMP_Text text;
    
    [SerializeField] private List<SpriteRenderer> badServiceStampsSprites;

    public AudioSource soundIn;
    public AudioSource soundOut;

    public bool display = true;

    private bool addingStamp = false;
    private Vector3 defPos;

	private float sincount = 80;
	public bool opened = false;

    private void Awake()
    {
        defPos = transform.position;
        Main.interactables.Add(this);
    }

    private void UpdateText()
    {
        text.text = "<size=1.7>laptop service</size>\n";
        text.text += "reputation: " + Main.obj.reputation;
        foreach (SpriteRenderer s in badServiceStampsSprites) s.gameObject.SetActive(false);
        
        for (int i = 0; i < Main.obj.badServiceStamps; i++)
        {
            badServiceStampsSprites[i].gameObject.SetActive(true);
        }
    }
    protected override void OnHover()
    {
        if (addingStamp) return;
        soundIn.Play();
        soundOut.Stop();
		opened = true;
        transform.DOKill();
        
        transform.DOMove(defPos + new Vector3(0, -2.4f, 0), 0.5f).SetEase(Ease.OutExpo);
        UpdateText();
    }

    public IEnumerator AddStamp()
    {
        opened = true;
        addingStamp = true;
        transform.DOKill();
        transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);
        UpdateText();
        yield return new WaitForSeconds(0.5f);
        Main.obj.badServiceStamps++;
        SpriteRenderer sprite = badServiceStampsSprites[Main.obj.badServiceStamps-1];
        sprite.gameObject.SetActive(true);
        sprite.transform.localScale = Vector3.one * 0.14f;
        sprite.color = new Color(1, 0, 0, 0);
        sprite.transform.DOScale(Vector3.one * 0.08f, 0.5f).SetEase(Ease.InExpo);
        sprite.DOFade(1, 0.5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1f);
        addingStamp = false;
        if (Main.obj.badServiceStamps == 2)
        {
            StartCoroutine(Main.obj.OnLose());
        }
        else if (Main.obj.currentPlace == Main.obj.workplace)
        {
            foreach (Tool tool in Main.obj.workplace.tools)
            {
                tool.RemoveFromScreen();
            }
            OnHoverExit();
            StartCoroutine(Main.obj.GoTo("pc"));
            Main.obj.currentStage = Main.obj.stages.checkNewMessage;
            Main.currentTask = null;
            Main.obj.workplace.finished = false;
            Destroy(Main.obj.workplace.currentLaptop.gameObject);
        }
        opened = false;
        UpdateInteracable();
        yield return new WaitForSeconds(1f);
        Main.obj.noUpdateInteractablesTimer = 0;
    }
    
    public IEnumerator RemoveStamp()
    {
        opened = true;
        addingStamp = true;
        transform.DOKill();
        transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);
        UpdateText();
        yield return new WaitForSeconds(0.5f);
        Main.obj.badServiceStamps--;
        SpriteRenderer sprite = badServiceStampsSprites[Main.obj.badServiceStamps];
        sprite.DOFade(0, 0.5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1f);
        addingStamp = false;
        
        opened = false;
        yield return new WaitForSeconds(1f);
        Main.obj.noUpdateInteractablesTimer = 0;
    }
    
    protected override void OnHoverExit()
    {
        if (addingStamp) return;
        soundOut.Play();
        soundIn.Stop();
		opened = false;
        transform.DOKill();
        
        transform.DOMove(defPos, 0.5f).SetEase(Ease.OutExpo);
        UpdateText();
    }

    protected override void OnUpdateInteractable()
    {
        if (!display)
        {
            transform.DOKill();
            transform.DOMove(transform.position + new Vector3(0, 2, 0), 0.5f).SetEase(Ease.OutExpo);
        }
        else
        {
            sincount += Time.deltaTime*2;
            if (sincount > 360) sincount = 0;
            if (!opened)
            {
                transform.DOKill();
                transform.DOLocalMove(defPos + new Vector3(0, Mathf.Sin(sincount)/10, 0), 0.2f);
            }
        }
    }
}
