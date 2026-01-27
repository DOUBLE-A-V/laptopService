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
    }
    public IEnumerator ChangeTime(int hoursAdd, float duration)
    {
        arrow.transform.DOLocalRotate(new Vector3(0, 0, arrow.transform.localRotation.eulerAngles.z - 40), duration).SetEase(Ease.Linear);
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
    }
    void Update()
    {
        sincounter +=  Time.deltaTime * 2;
        if (sincounter > 360) sincounter = 0;
        doublePointText.color = new Color(1, 1, 1, Mathf.Abs(Mathf.Sin(sincounter)));
    }
}
