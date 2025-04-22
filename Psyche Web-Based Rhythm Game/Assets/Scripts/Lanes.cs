using System;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Interaction;
using TMPro;
using UnityEngine;

public class Lanes : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName notePicked;
    public KeyCode input;
    public GameObject notePrefab;
    List<Notes> notes = new List<Notes>();
    public List<double> timeStamps = new List<double>();
    int spawnCount = 0;
    int inputIndex = 0;
    public static Lanes Instance;
    public bool isPaused = false;

    void Start()
    {
        Instance = this;
    }
    public void PauseGame()
    {
        isPaused = true;

        foreach (Notes note in notes)
        {
            note.PauseNote();
        }
    }

    public void ResumeGame()
    {
        isPaused = false;

        foreach (Notes note in notes)
        {
            note.ResumeNote();
        }
    }

    public void setTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == notePicked)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, Manager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f); //converting the tempo map(when hit was recorded) to seconds. For instance, the first tap/object to appear in Lane 1 will have a timestamp of 48 seconds, meaning it will reach the -3 Y Axis(where user supposed to tap) at that time precisely.
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
            return; // Stop everything when paused

        if (spawnCount < timeStamps.Count)
        {
            if (Manager.getAudioSourceTime() >= timeStamps[spawnCount] - 1)
            {
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Notes>());
                note.GetComponent<Notes>().assignedTime = (float)timeStamps[spawnCount];
                spawnCount++;
            }
        }

        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = Manager.Instance.marginOfError;
            double audioTime = Manager.getAudioSourceTime();

            if (Input.GetKeyDown(input))
            {
                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    Manager.Hit();
                    Destroy(notes[inputIndex].gameObject);
                    inputIndex++;
                }
            }
            if (timeStamp + marginOfError <= audioTime)
            {
                Manager.Miss();
                inputIndex++;
            }
        }
    }
}