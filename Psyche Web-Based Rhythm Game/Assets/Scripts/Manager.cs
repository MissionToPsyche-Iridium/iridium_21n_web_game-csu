using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using System;

public class Manager : MonoBehaviour
{
    public static Manager Instance;
    public Lanes[] lanes;
    public AudioSource theSong;
    public double marginOfError;
    private string[] midiName = {"day50.mid", "day69.mid", "day15.mid"};
    //public float noteTime;
    public float spawnYCoordinate;
    public float tapYCoordinate;
    public bool startPlaying;
    public bool functionCalled;
    public bool readFromWeb;
    private static int level = 0;
    private static int midiLevel =0;
    public  AudioClip[] clip;

    public MidiFile[] loadedMidis;

    public static int pointStreak = 0;

      public float despawnYCoordinate
    {
        get{
            return tapYCoordinate - (spawnYCoordinate - tapYCoordinate);
        }
    }

    public static MidiFile midiFile;
    void Start() 
    {
      //  noteTime = 1;
        tapYCoordinate = -3;
        spawnYCoordinate = 7;
        marginOfError = 0.25;
        Instance = this;
        print($"Press Return/Enter to start the Game!");
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
            StartCoroutine(loadStreamingAsset());
            readFromWeb = true;
        }
        else
        {
            ReadFromFile();
        }
    }

   private IEnumerator loadStreamingAsset()
{
    string fullPath1 = Application.streamingAssetsPath + "/" + midiName[0];
    string fullPath2 = Application.streamingAssetsPath + "/" + midiName[1];
    string fullPath3 = Application.streamingAssetsPath + "/" + midiName[2];
    string[] paths = { fullPath1, fullPath2, fullPath3 };
    int i = 0;
    foreach(string path in paths)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(path))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error loading MIDI: " + www.error);
            }
        else
            {
                byte[] results = www.downloadHandler.data;
                using (MemoryStream stream = new MemoryStream(results))
                {
                    loadedMidis[i] = MidiFile.Read(stream); 
                }
            }
        }
        i++;
    }
}
    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + midiName[midiLevel]);                  
    }

    public void getDataFromMidi()
    { 
        if(readFromWeb && level == 0)
        {
            midiFile = loadedMidis[0];
        }
        else if(readFromWeb && level == 1)
        {
            midiFile = loadedMidis[1];
        }
         else if(readFromWeb && level == 2)
        {
            midiFile = loadedMidis[2];
        }
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array,0);

        foreach(var lane in lanes)
        {
            lane.setTimeStamps(array);
        }

        Invoke(nameof(StartSong), 0);
        
    }

    public static double getAudioSourceTime()
    {
        return (double)Instance.theSong.timeSamples / Instance.theSong.clip.frequency;
    }

    public void StartSong()
    {
        theSong.Play();     
    }

    public void startGame(bool functionCall)
    {
         
         if(!functionCall)
                {
                    if(level != 0) 
                    {
                        midiLevel++;
                    }           
                       StartCoroutine(StartGameCoroutine());            
                }            
    }

    private IEnumerator StartGameCoroutine()
    {
        if (readFromWeb)
        {
            yield return StartCoroutine(loadStreamingAsset()); 
        }
        else
        {
            ReadFromFile();
        }
        getDataFromMidi(); 
        theSong.clip = clip[level];
        startPlaying = true;
        functionCalled = true;
        level++;
    }

    private void checkSong()
    {
        if(level < 3)
        {
        if (Math.Round(getAudioSourceTime()) >= Math.Round(theSong.clip.length))
        {
            if(functionCalled)
            {
                nextLevel();
            }  
        }
        }
    }

    private void nextLevel()
    {
        functionCalled = false;
        print($"Press Return/Enter to proceed to level {level+1}!");
        if(Input.GetKeyDown(KeyCode.Return))
        {
            startGame(functionCalled);
        }
    }

    public static void Hit()
    {
        pointStreak++;
        Debug.Log($"You hit!! Streak: {pointStreak}, midi: {midiLevel} ");
    }

    public static void Miss()
    {
        pointStreak = 0;
        Debug.Log($"You missed! Streak reset. {getAudioSourceTime()} - {Instance.theSong.clip.length} level: {level} function: {Instance.functionCalled} ");
    }
    void Update() {

            if(Input.GetKeyDown(KeyCode.Return))
            {            
                startGame(functionCalled);             
            }
            if(level > 0 && level < 3)
            {
                Invoke(nameof(checkSong), 0.01f);
            }
    }
}

