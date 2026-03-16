using UnityEngine;
using DG.Tweening;

public class PCButton : Interactable
{
    protected override void Interact()
    {
        transform.DOKill();
        transform.localScale = defaultScale * 0.9f;
        transform.DOScale(defaultScale, 0.2f);
        active = false;
        Main.obj.pcManager.Toggle();
        Main.obj.PlaySound("button");
    }
}