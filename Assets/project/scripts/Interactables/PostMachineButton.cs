using UnityEngine;
using DG.Tweening;

public class PostMachineButton : Interactable
{
    public string task;
    protected override void Interact()
    {
        transform.DOKill();
        transform.localScale = defaultScale * 0.9f;
        transform.DOScale(defaultScale, 0.2f);
        Main.obj.postMachine.ClickNumButton(task);
    }
}