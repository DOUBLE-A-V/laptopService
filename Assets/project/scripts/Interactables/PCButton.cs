using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PCButton : Interactable
{
    protected override void Interact()
    {
        transform.DOKill();
        transform.localScale = Vector3.one * 0.9f;
        transform.DOScale(Vector3.one, 0.2f);
        Main.obj.pcManager.Toggle();
        active = false;
    }
}