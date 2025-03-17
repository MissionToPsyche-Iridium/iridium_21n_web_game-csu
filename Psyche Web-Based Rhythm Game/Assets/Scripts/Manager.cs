using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

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
    private int level = 0;
    private int midiLevel =0;
    public AudioClip[] clip;

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
    }

    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + midiName[midiLevel]);        
        midiLevel++;     
    }

    public void getDataFromMidi()
    {
        
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
                    ReadFromFile();
                    theSong.clip = clip[level];
                    startPlaying = true;
                    functionCalled = true;
                    getDataFromMidi();
                    level++;
                }            
    }

    private void checkSong()
    {
        if (!theSong.isPlaying && level < 3)
        {
            if(level == 1 && functionCalled)
            {
                nextLevel();
            }
            else if (level == 2 && functionCalled)
            {
               nextLevel();
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
        Debug.Log($"You hit!! Streak: {pointStreak}");
    }

    public static void Miss()
    {
        pointStreak = 0;
        Debug.Log($"You missed! Streak reset.");
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

