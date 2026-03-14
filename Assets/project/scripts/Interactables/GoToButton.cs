using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class GoToButton : Interactable
{
    [SerializeField] private Image arrowsImage;

    [SerializeField] private TMP_Text text;

    public string goToPlace;
    
    protected override void OnHover()
    {
        Main.obj.PlaySound("hover");
        arrowsImage.transform.DOKill();
        arrowsImage.transform.DOScale(new Vector3(1.5f, 1, 1), 0.5f).SetEase(Ease.OutElastic, 0.5f);
    }
    protected override void OnHoverExit()
    {
        arrowsImage.transform.DOKill();
        arrowsImage.transform.DOScale(1, 0.5f).SetEase(Ease.OutElastic, 0.5f);
    }

    protected override void Interact()
    {
        Main.obj.PlaySound("click");
        StartCoroutine(Main.obj.GoTo(goToPlace));
    }

    public void Hide()
    {
        active = false;
        arrowsImage.transform.DOKill();
        arrowsImage.transform.DOScale(new Vector3(0, 1, 1), 1f).SetEase(Ease.OutExpo);
        arrowsImage.DOFade(0, 0.5f).SetEase(Ease.OutExpo);
        text.DOFade(0, 1f);
    }

    public void Show()
    {
        active = true;
        arrowsImage.transform.DOKill();
        arrowsImage.transform.DOScale(new Vector3(1, 1, 1), 1f).SetEase(Ease.OutExpo);
        arrowsImage.DOFade(1, 0.5f).SetEase(Ease.OutExpo);
        text.DOFade(1, 1f);
    }
}
