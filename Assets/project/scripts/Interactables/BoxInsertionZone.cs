using UnityEngine;
using DG.Tweening;

public class BoxInsertionZone : Interactable
{
	[SerializeField] private SpriteRenderer highlight;
	private float sincount = 0;
    protected override void Interact()
    {
        transform.DOKill();
        if (Main.obj.postMachine.waitingForInsertionZone)
        {
            active = false;
            if (Main.obj.postMachine.mode == "sending")
            {
                StartCoroutine(Main.obj.postMachine.EatBox());
            }
            else
            {
                Main.obj.postMachine.TakeBox();
            }
            Main.obj.postMachine.waitingForInsertionZone = false;
        }
    }

	protected void Update() {
		if (active) {
			sincount += Time.deltaTime * 2;
			if (sincount >= 360) sincount = 0;
			highlight.DOKill();
			highlight.color = new Color(1, 1, 1, Mathf.Abs(Mathf.Sin(sincount)));
		}
		else
		{
			sincount = 0;
			highlight.DOFade(0, 0.5f);
		}
	}
}