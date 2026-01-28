using UnityEngine;
using System.Collections;
using DG.Tweening;
using TMPro;

public class Clock : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text doublePointText;
    [SerializeField] private GameObject arrow;
    private float sincounter = 0;

    public int hours = 16;
    public int minutes = 0;

    private bool anim = false;

    public void ResetClock(int resetHours, int resetMinutes)
    {
        hours = resetHours;
        minutes = resetMinutes;
        UpdateText(hours, minutes);
        arrow.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void UpdateText(int hours, int minutes)
    {
        timeText.text = hours.ToString("00") + " " + minutes.ToString("00");
        doublePointText.text = hours.ToString("00") + ":" + minutes.ToString("00");
    }
    public IEnumerator ChangeTime(int hoursAdd, float duration)
    {
        arrow.transform.DOLocalRotate(new Vector3(0, 0, arrow.transform.localRotation.eulerAngles.z - 40), duration).SetEase(Ease.Linear);
        if (hours == 23) Main.obj.noUpdateInteractablesTimer = 9999999;
        for (int i = 0; i < hoursAdd * 30; i++)
        {
            yield return null;
            minutes += 2;
            if (minutes == 60)
            {
                hours++;
                minutes = 0;
                if (hours == 24) hours = 0;
            }
            UpdateText(hours, minutes);
        }

        if (hours == 0)
        {
            Main.obj.workplace.FinishService();
            transform.DOKill();
            transform.DOScale(1, 0.5f);
            anim = true;
            timeText.text = hours.ToString("00") + ":" + minutes.ToString("00");
            Main.obj.noUpdateInteractablesTimer = 9999999;
            yield return new WaitForSeconds(1);
            Main.obj.noUpdateInteractablesTimer = 9999999;
            Vector3 defPos = transform.localPosition;
            transform.DOLocalMove(Vector3.zero, 1).SetEase(Ease.OutExpo);
            transform.DOScale(Vector3.one * 3, 1f).SetEase(Ease.OutExpo);
            float timer = 0;
            float blinkTimer = 0;
            while (timer < 3.5f)
            {
                timer += Time.deltaTime;
                blinkTimer -= Time.deltaTime;
                if (blinkTimer <= 0)
                {
                    timeText.gameObject.SetActive(!timeText.gameObject.activeSelf);
                    blinkTimer = 0.5f;
                }

                yield return null;
            }

            transform.DOLocalMove(defPos, 0.5f).SetEase(Ease.OutExpo);
            transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);
            timeText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Main.obj.badStampReceive.Show());
            anim = false;
        }
    }
    void Update()
    {
        if (!anim)
        {
            sincounter +=  Time.deltaTime * 2;
            if (sincounter > 360) sincounter = 0;
            doublePointText.color = new Color(1, 1, 1, Mathf.Abs(Mathf.Sin(sincounter)));
        }
        else
        {
            doublePointText.color = new Color(1, 1, 1, 0);
        }
    }
}
