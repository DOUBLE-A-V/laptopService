using UnityEngine;
using DG.Tweening;

public class PCButton : Interactable
{
    protected override void Interact()
    {
        transform.DOKill();
        transform.localScale = Vector3.one * 0.9f;
        transform.DOScale(Vector3.one, 0.2f);
        active = false;
        Main.obj.pcManager.Toggle();
    }
}