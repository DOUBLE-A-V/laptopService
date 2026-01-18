using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;
using Random = UnityEngine.Random;

public class PCManager : Place
{
    [SerializeField] private AudioSource pcStartSound;
    [SerializeField] private AudioSource pcWorkSound;
    [SerializeField] private PCButton pcButton;
    [SerializeField] private DiskWorkSimulation diskWorkSim;
    [SerializeField] private AudioSource shutdownSound;

    [SerializeField] private SpriteRenderer workingLed;

    [SerializeField] private SpriteRenderer splashSprite;

    [SerializeField] private GameObject caret;

    [SerializeField] private SpriteRenderer breakingNewsImage;

    [SerializeField] private GameObject mail;

    [SerializeField] private GameObject newMessageEffect;

    [SerializeField] private TMP_Text newsText;

    [SerializeField] private List<string> newsTexts;

    [SerializeField] private GameObject message;
    
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private TMP_Text fromText;
    
    public AudioSource pipSound;
    
    public bool working = false;

    public bool blocked = false;

    private float caretBlinkTimer = 0;
    
    private bool caretActive = false;
    
    private bool onMainScreen = false;

    public void Disable()
    {
        gameObject.SetActive(false);
    }
    
    public IEnumerator StartPC()
    {
        Main.obj.HideGoToButtons();
        pcWorkSound.enabled = true;
        blocked = true;
        workingLed.color = Color.white;
        pcStartSound.Play();
        Main.cam.DOKill();
        Main.cam.DOOrthoSize(3, 2).SetEase(Ease.InOutExpo);
        yield return new WaitForSeconds(1.2f);
        caretActive = true;
        yield return new WaitForSeconds(1.2f);
        pcWorkSound.Play();
        diskWorkSim.SimWork(0.2f);
        yield return new WaitForSeconds(0.4f);
        diskWorkSim.SimWork(0.2f);
        yield return new WaitForSeconds(0.7f);
        diskWorkSim.SimWork(0.2f);
        pipSound.Play();
        caretActive = false;
        caret.SetActive(false);
        splashSprite.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        diskWorkSim.SimWork(0.25f);
        yield return new WaitForSeconds(0.5f);
        diskWorkSim.SimWork(0.25f);
        yield return new WaitForSeconds(0.5f);
        pipSound.Play();
        splashSprite.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        diskWorkSim.SimWork(0.25f);
        yield return new WaitForSeconds(0.5f);
        pipSound.Play();
        mail.SetActive(true);
        working = true;
        breakingNewsImage.transform.localPosition = new Vector3(3, 0, 0);
        onMainScreen = true;
        newsText.text = "";
        yield return new WaitForSeconds(0.5f);
        diskWorkSim.SimWork(0.25f);
        yield return new WaitForSeconds(0.5f);
        newsText.text = newsTexts[Random.Range(0, newsTexts.Count)];
        
        blocked = false;
        pcButton.active = true;
    }

    public void Shutdown()
    {
        Main.obj.ShowGoToButtons();
        Main.cam.DOKill();
        Main.cam.DOOrthoSize(4.8f, 2).SetEase(Ease.InOutExpo);
        message.SetActive(false);
        onMainScreen = false;
        pcWorkSound.Stop();
        pcWorkSound.enabled = false;
        working = false;
        workingLed.color = new Color(0.5f, 0.5f, 0.5f, 1);
        shutdownSound.Play();
        pcButton.active = true;
        mail.SetActive(false);
    }

    public void Toggle()
    {
        if (working && !blocked) Shutdown();
        else if (!blocked) StartCoroutine(StartPC());
    }

    public IEnumerator GoToMessage()
    {
        if (blocked) yield break;
        blocked = true;
        diskWorkSim.SimWork(0.25f);
        mail.SetActive(false);
        onMainScreen = false;
        pipSound.Play();
        yield return new WaitForSeconds(0.5f);
        diskWorkSim.SimWork(0.25f);
        pipSound.Play();
        message.gameObject.SetActive(true);
        messageText.text = "";
        fromText.text = "";
        yield return new WaitForSeconds(0.5f);
        if (Main.currentTask == null)
        {
            Main.currentTask = Main.obj.serviceTasks[Random.Range(0, Main.obj.serviceTasks.Count)];
            Main.currentTask.postID = Random.Range(1000000, 9999999);
            Main.currentTask.currentCost = 
                (float)Math.Round(Random.Range(Main.currentTask.costFrom, Main.currentTask.costTo), 2);
            Main.currentTask.from = Main.obj.names[Random.Range(0, Main.obj.names.Count)];
        }
        
        messageText.text = Main.currentTask.message
                           + "\nI can pay " + Main.currentTask.currentCost + "$"
                           + "\npost ID: " + Main.currentTask.postID;
        fromText.text = Main.currentTask.from;
        
        blocked = false;
    }

    private void Update()
    {
        if (onMainScreen)
        {
            breakingNewsImage.transform.localPosition += new Vector3(-1.5f * Time.deltaTime, 0, 0);
            if (breakingNewsImage.transform.localPosition.x < -3) breakingNewsImage.transform.localPosition = new Vector3(3, 0, 0);
            caretBlinkTimer -= Time.deltaTime;
            if (caretBlinkTimer <= 0)
            {
                caretBlinkTimer = 0.5f;
                if (breakingNewsImage.color.r != 0) breakingNewsImage.color = Color.black;
                else breakingNewsImage.color = Color.white;

                newMessageEffect.SetActive(!newMessageEffect.active);
            }
        }
        if (caretActive)
        {
            caretBlinkTimer -= Time.deltaTime;
            if (caretBlinkTimer <= 0)
            {
                caret.SetActive(!caret.active);
                caretBlinkTimer = 0.3f;
            }
        }
    }
}
