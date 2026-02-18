using System.Numerics;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Vector3 = UnityEngine.Vector3;

public class InfoSticker : Interactable
{
    [SerializeField] private TMP_Text text;

    private Vector3 defPos;

    public bool display = true;

	private float sincount = 45;
	public bool opened = false;

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
		opened = true;
        transform.DOKill();
        
        transform.DOMove(defPos + new Vector3(0, -2, 0), 0.5f).SetEase(Ease.OutExpo);
        UpdateText();
    }

    protected override void OnHoverExit()
    {
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