using System;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;

public class Lanes : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public KeyCode input;
    public GameObject notePrefab;
    List<Notes> notes = new List<Notes>();
    public List<double> timeStamps = new List<double>();
    int spawnIndex = 0;
    int inputIndex = 0;

    void Start()
    {
        
    }

    public void setTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach(var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, Manager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f); //converting the tempo map(when hit was recorded) to seconds. For instance, the first tap/object to appear in Lane 1 will have a timestamp of 48 seconds, meaning it will reach the -3 Y Axis(where user supposed to tap) at that time precisely.
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnIndex < timeStamps.Count)
        {
            if(Manager.getAudioSourceTime () >= timeStamps[spawnIndex] - Manager.Instance.noteTime)
            {
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Notes>());
                note.GetComponent<Notes>().assignedTime = (float)timeStamps[spawnIndex];  
                spawnIndex++;
            }
        }

        if(inputIndex < timeStamps.Count)//Everytime the user taps/presses a key, it is 1 input index. If the amount of
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = Manager.Instance.marginOfError;
            double audioTime = Manager.getAudioSourceTime() - (0/1000.0);

            if(Input.GetKeyDown(input))
            {
                if(Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                        Hit();
                        Destroy(notes[inputIndex].gameObject);
                        inputIndex++;
                }
                else{
              //      print($"Hit inaccurate on {inputIndex} note ");
                }
            }
            if(timeStamp + marginOfError <= audioTime)
            {
                Miss();
              //  print($"Missed {inputIndex} Note");
                inputIndex++;
            }

        }
    }

    private void Hit()
    {
        print($"you hit!! ");
    }

    private void Miss()
    {

    }
}
