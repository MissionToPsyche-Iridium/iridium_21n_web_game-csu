using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

public class CountDownTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] double remainingTime;

    void Update()
    {
        remainingTime = (Math.Round(Manager.Instance.theSong.clip.length) - Math.Round(Manager.getAudioSourceTime())) ;
        int minutes = Mathf.FloorToInt((float)(remainingTime / 60));
        int seconds = Mathf.FloorToInt((float)(remainingTime % 60));
        timerText.text = string.Format("{0:00}:{1:00}",minutes,seconds);

    }

    public void Start()
    {
    }
}
