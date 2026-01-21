using System.Numerics;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Vector3 = UnityEngine.Vector3;

public class InfoSticker : Interactable
{
    [SerializeField] private TMP_Text text;

    private Vector3 defPos;

    private void Awake()
    {
        defPos = transform.position;
        Main.interactables.Add(this);
    }

    private void UpdateText()
    {
        text.text = Main.obj.currentStage.name + "\n";
        if ((Main.obj.currentStage.id == 3 || Main.obj.currentStage.id == 4) && !Main.obj.workplace.finished)
        {
            text.text += "post id: " + Main.currentTask.postID;
        }
    }
    protected override void OnHover()
    {
        transform.DOKill();
        
        transform.DOMove(defPos + new Vector3(0, -2, 0), 0.5f).SetEase(Ease.OutExpo);
        UpdateText();
    }

    protected override void OnHoverExit()
    {
        transform.DOKill();
        
        transform.DOMove(defPos, 0.5f).SetEase(Ease.OutExpo);
        UpdateText();
    }
}