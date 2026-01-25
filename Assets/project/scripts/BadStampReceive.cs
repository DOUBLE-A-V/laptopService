using DG.Tweening;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class BadStampReceive : MonoBehaviour
{
    [SerializeField] private Image stamp;
    [SerializeField] private TMP_Text repText;

    public IEnumerator Show()
    {
        Main.obj.noUpdateInteractablesTimer = 9999999;
        transform.DOKill();
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo);
        stamp.color = new Color(1, 1, 1 , 0);
        repText.color = new Color(0.8f, 0, 0, 0);
        yield return new WaitForSeconds(0.5f);
        stamp.transform.DOKill();
        stamp.DOFade(1, 0.5f).SetEase(Ease.Linear);
        stamp.transform.localScale = Vector3.one * 1.5f;
        stamp.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InExpo);
        yield return new WaitForSeconds(0.5f);
        repText.text = "-" + Main.obj.qualities.failedServiceRepChange + " reputation";
        repText.transform.localScale = Vector3.one * 1.5f;
        repText.transform.DOKill();
        repText.DOFade(1, 0.5f).SetEase(Ease.Linear);
        repText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo);
        yield return new WaitForSeconds(1.2f);
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Main.obj.infoSheet.AddStamp());
    }

    public void Hide()
    {
        Main.obj.noUpdateInteractablesTimer = 0;
    }
}
