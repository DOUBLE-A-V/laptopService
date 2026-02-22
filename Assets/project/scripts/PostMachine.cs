using System;
using DG.Tweening;
using NUnit.Framework.Internal.Commands;
using UnityEngine;
using TMPro;
using System.Collections;

public class PostMachine : Place
{
    [SerializeField] private TMP_Text postMachineText;
    [SerializeField] private BoxInsertionZone boxInsertionZone;
    [SerializeField] private GameObject box;

    [SerializeField] private AudioSource eatingBoxSound;
    [SerializeField] private AudioSource takeBoxSound;
    
    public ServiceReport serviceReport;

    public string mode = "sending";
    
    public bool waitingForInsertionZone = false;

    private string postID = "";

    public override void OnEnter()
    {
		boxInsertionZone.active = false;
        postID = "";
        waitingForInsertionZone = false;
        mode = "";
        postMachineText.text = "select action\n\n\n\n\n\n\n\n                            \\/";
    }
    
    public void ClickNumButton(string task)
    {
        if (task.StartsWith("num "))
        {
            if (mode != "receiving" || postID.Length >= 7) return;
            postMachineText.text += int.Parse(task.Split(' ')[1]);
            postID += int.Parse(task.Split(' ')[1]);
        } else if (task == "mode send")
        {
            if (mode != "sending")
            {
                if (Main.obj.workplace.finished)
                {
                    mode = "sending";
                    postMachineText.text = "insert package";
                    boxInsertionZone.active = true;
                    waitingForInsertionZone = true;
                    box.transform.localPosition = new Vector3(0.12f, -6.3f, 0);
                }
                else
                {
                    postMachineText.text = "nothing to send";
                }
            }
        }
        else if (task == "mode receive")
        {
            mode = "receiving";
            box.transform.localPosition = new Vector3(0.12f, -2.3f, 0);
            postMachineText.text = "type post ID\n";
        }
        else if (task == "apply")
        {
            Apply();
        } else if (task == "remove" && mode == "receiving" && !waitingForInsertionZone)
        {
            postMachineText.text = "type post ID\n";
            postID = "";
        }
    }

    public IEnumerator EatBox()
    {
        float cost = (float)Math.Round(Main.currentTask.currentCost, 1);
        Main.obj.workplace.finished = false;
        Main.currentTask = null;
        serviceReport.qualities.quality = Main.obj.workplace.currentLaptop.quality;
        serviceReport.qualities.cost = cost;
        serviceReport.qualities.repChange = serviceReport.qualities.quality - Main.obj.qualities.qualityPositiveService;
        
        Destroy(Main.obj.workplace.currentLaptop.gameObject);
        
        Main.obj.DeactivateGoToButtons();
        if (eatingBoxSound)eatingBoxSound.Play();
        box.transform.DOLocalMove(box.transform.localPosition + new Vector3(0, 2.3f, 0), 1);
        yield return new WaitForSeconds(1.5f);
        box.transform.DOLocalMove(box.transform.localPosition + new Vector3(0, 1.7f, 0), 2).SetEase(Ease.Linear);
        yield return new WaitForSeconds(2);
        postMachineText.text = "have a nice day!";
        if (Main.obj.currentStage.id == Main.obj.stages.sendBox.id) Main.obj.currentStage = Main.obj.stages.gotoPc;
        Main.obj.GiveMoney((float)Math.Round(cost * serviceReport.qualities.quality / 100f, 1));
        serviceReport.Show();
        yield return new WaitForSeconds(0.5f);
        if (serviceReport.qualities.quality < Main.obj.qualities.qualityPoorService)
        {
            StartCoroutine(Main.obj.badStampReceive.Show());
        } else if (serviceReport.qualities.quality > Main.obj.qualities.qualityRemovePoorService)
        {
            if (Main.obj.badServiceStamps > 0)
            {
                StartCoroutine(Main.obj.infoSheet.RemoveStamp());
            }
        }
        Main.obj.reputation += serviceReport.qualities.quality - Main.obj.qualities.qualityPositiveService;
        
        Main.obj.workplace.finished = false;
        while (serviceReport.showed)
        {
            yield return null;
        }
        Main.obj.ActivateGoToButtons();
    }

    public void TakeBox()
    {
        if (takeBoxSound)takeBoxSound.Play();
        box.transform.DOKill();
        box.transform.DOLocalMove(new Vector3(0.12f, -6.3f, 0), 1f).SetEase(Ease.OutExpo);
        postMachineText.text = "have a nice day!";
    }

    private IEnumerator InvalidPostID()
    {
        Main.obj.DeactivateGoToButtons();
        postMachineText.text = "invalid post id!";
        yield return new WaitForSeconds(1.5f);
        postMachineText.text = "type post ID\n";
        postID = "";
        Main.obj.ActivateGoToButtons();
    }

    private void Apply()
    {
        if (mode == "sending")
        {
            
        }
        else if (mode == "receiving")
        {
            if (Main.currentTask != null && int.Parse(postID) == Main.currentTask.postID)
            {
                waitingForInsertionZone = true;
                boxInsertionZone.active = true;
                box.transform.DOLocalMove(box.transform.localPosition - new Vector3(0, 1.7f, 0), 2f).SetEase(Ease.Linear);
                postMachineText.text = "take your package";
                Main.obj.receivedBox = true;
                if (Main.obj.currentStage.id == Main.obj.stages.receiveBox.id)
                {
                    Main.obj.currentStage = Main.obj.stages.gotoWorkplace;
                }
            }
            else
            {
                StartCoroutine(InvalidPostID());
            }
        }
    }
}
