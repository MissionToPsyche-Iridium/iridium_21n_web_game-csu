using System;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Interaction;
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

    void Start()
    {
        
    }

    public void setTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach(var note in array)
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
        if (spawnCount < timeStamps.Count)
        {
            if(Manager.getAudioSourceTime () >= timeStamps[spawnCount] - 1) //replaced noteTime with 1. Greater than 1 messes with the spawn of the note. If changed and everything else the same, this will cause the notes to come down a lot faster
            {
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Notes>());
                note.GetComponent<Notes>().assignedTime = (float)timeStamps[spawnCount];  
                spawnCount++;
            }
        }

        if(inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = Manager.Instance.marginOfError;
            double audioTime = Manager.getAudioSourceTime();

            if(Input.GetKeyDown(input))
            {
                if(Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                        Manager.Hit();
                        Destroy(notes[inputIndex].gameObject);
                        inputIndex++;
                }
            }
            if(timeStamp + marginOfError <= audioTime)
            {
                Manager.Miss();
                inputIndex++;
            }

        }
    }
}
