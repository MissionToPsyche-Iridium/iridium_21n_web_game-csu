using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Manager : MonoBehaviour
{
    public static Manager Instance;
    public Lanes[] lanes;
    public AudioSource theSong;
    public double marginOfError;
    private string[] midiName = { "day50.mid", "day69.mid",  "day15.mid" };
    private string[] midiNameNASA = {"ascension.mid", "breathless.mid", "degasparis.mid"};
    //public float noteTime;
    public float spawnYCoordinate;
    public float tapYCoordinate;
    public static bool gameRunning;
    public bool readFromWeb;
    public static int level = 0;
    public static int midiLevel = 0;
    public AudioClip[] clip;
    public Image healthBar;

    public int finalScore;
    public int scorePerHit = 100;
    public Text scoreText;
    public Text multiplierText;
    public int finalMultiplier;
    public int multiplierTracker;
    public int[] multiplierThresh;

    public GameObject popupTextPrefab; 
    public float popupDuration = 1f;    
    public float fadeDuration = 0.5f;

    public float satelliteFuel = 100f;
    public static MidiFile[] loadedMidis;

    public static int pointStreak = 0;

    public float despawnYCoordinate
    {
        get
        {
            return tapYCoordinate - (spawnYCoordinate - tapYCoordinate);
        }
    }

    public static MidiFile midiFile;
    public RawImage rawImageUI;       
    public RenderTexture renderTexture;   
    public VideoPlayer videoPlayer;    
    public static bool GameOver = false;  

    public static bool NASACollection = true;
    
    void Start()
    {
        //  noteTime = 1;
        tapYCoordinate = -271;
        spawnYCoordinate = 400;
        marginOfError = 0.25;
        Instance = this;
        scoreText.text = "Score: 0";
        multiplierText.text = "Multiplier: x1";
        finalMultiplier = 1;
        multiplierThresh = new int[] { 2, 4, 6 };
        if (NASACollection)
        {
            clip = new AudioClip[]
            {
                Resources.Load<AudioClip>("Songs/ascension"),
                Resources.Load<AudioClip>("Songs/breathless"),
                Resources.Load<AudioClip>("Songs/degasparis")
            };
        }
        else
        {
            clip = new AudioClip[]
            {
                Resources.Load<AudioClip>("Songs/day50"),
                Resources.Load<AudioClip>("Songs/day69"),
                Resources.Load<AudioClip>("Songs/day15")
            };
        }
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
             StartCoroutine(loadStreamingAsset());
            readFromWeb = true;
        }
        else
        {
            if(NASACollection)
            {
                midiName = midiNameNASA;
                videoPlayer.url = Application.streamingAssetsPath + "/PsycheBackgroundNASAVer.mov";
            }
            else{
                videoPlayer.url = Application.streamingAssetsPath + "/fullLevelsPsyche.mp4";
            }
            StartCoroutine(ReadFromFile());
        }
    }
    void StartVideoDisplay()
    {
        rawImageUI.texture = renderTexture;
        videoPlayer.Play();
    }

    public void useFuel()
    {
        if (satelliteFuel <= 100 && satelliteFuel >= 1 && gameRunning)
        {
            if(level == 1)
            {
                satelliteFuel--;
            }
            else if(level == 2)
            {
                satelliteFuel -= 2;
            }
            else if(level == 3)
            {
                satelliteFuel -=2;
            }
            if (satelliteFuel < 0)
            {
                satelliteFuel = 0;
            }
            healthBar.fillAmount = satelliteFuel / 100f;
        }
    }

    public void gainFuel()
    {
        if (satelliteFuel <= 100 && satelliteFuel >= 1 && gameRunning)
        {
            satelliteFuel += 2;
            if (satelliteFuel > 100)
            {
                satelliteFuel = 100;
            }
            healthBar.fillAmount = satelliteFuel / 100f;
        }
    }

    private IEnumerator loadStreamingAsset()
    {
        string pathVid;
        if(NASACollection)
        {
           pathVid = Application.streamingAssetsPath + "/PsycheBackgroundNASAVer.mov";
           midiName = midiNameNASA;
        }
        else
        {
            pathVid = Application.streamingAssetsPath + "/fullLevelsPsyche.mp4";
        }
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = pathVid;
        videoPlayer.Prepare();
        string fullPath1 = Application.streamingAssetsPath + "/" + midiName[0];
        string fullPath2 = Application.streamingAssetsPath + "/" + midiName[1];
        string fullPath3 = Application.streamingAssetsPath + "/" + midiName[2];
        string[] paths = { fullPath1, fullPath2, fullPath3 };
        int i = 0;
        foreach (string path in paths)
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
    private IEnumerator ReadFromFile()
    {
         midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + midiName[midiLevel]);
         yield return null;
    }

    public IEnumerator getDataFromMidi()
    {
        if (readFromWeb)
        {
            midiFile = loadedMidis[level];
        }
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (var lane in lanes)
        {
            lane.setTimeStamps(array);
        }
        yield return null;
    }

    public static double getAudioSourceTime()
    {
        return (double)Instance.theSong.timeSamples / Instance.theSong.clip.frequency;
    }

    public void startGame(bool isGameRunning)
    {
        if (!isGameRunning)
        {
            if (level != 0)
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
           yield return StartCoroutine(ReadFromFile());
        }

    yield return StartCoroutine(getDataFromMidi());
    videoPlayer.Prepare();

    while (!videoPlayer.isPrepared)
    {
        yield return null;
    }      
    if(level == 0 )
    {
        NextScene.savedTime = 0;
    }   
        videoPlayer.time = NextScene.savedTime; 
        videoPlayer.Play(); 
        theSong.clip = clip[level];
       // theSong.pitch = 10f;
       // videoPlayer.playbackSpeed = 10f;
        theSong.Play();     
        gameRunning = true;
        level++;
    }

    public static void Hit()
    {
        pointStreak++;
        Instance.gainFuel();
        //   Debug.Log($"You hit!! Streak: {pointStreak}, midi: {midiLevel} ");

        //Debug.Log("Hit on time");
        Instance.finalScore += Instance.scorePerHit * Instance.finalMultiplier;
        Instance.scoreText.text = "Score: " + Instance.finalScore;
        Instance.multiplierText.text = "Multiplier: x" + Instance.finalMultiplier;
    }

   

    public static void Miss()
    {
        Instance.finalMultiplier = 1;
        Instance.multiplierTracker = 0;
        Instance.multiplierText.text = "Multiplier: x" + Instance.finalMultiplier;


        pointStreak = 0;
    }

    public void delayStart()
    {
        StartVideoDisplay();
        startGame(gameRunning);
    }

    public bool trackEnded()
    {
        if(Math.Round(getAudioSourceTime()) >= Math.Round(theSong.clip.length))
        {
            return true;
        }
        else{
            return false;
        }
    }

    void Update()
    {               
        if(NextScene.backToGame)
        {
            gameRunning = false;
            NextScene.backToGame = false;
            if(level == 0)
            {
                delayStart();
            }
            else if (level > 0) {
                startGame(gameRunning);
            }
            
        }       
        if (level > 0 && level < 3 && !GameOver && gameRunning )
            {
            if (trackEnded())
                {
                    videoPlayer.Pause();
                    NextScene.savedTime = videoPlayer.time;
                    if (gameRunning)
                    {
                        NextScene.Instance.nextScene();
                    }
                }
            }
        if(level >= 3 && Math.Round(videoPlayer.time) >= Math.Round(videoPlayer.length))
            {
                if (gameRunning)
                    {
                        NextScene.Instance.nextScene();
                    }
            }
            if (satelliteFuel == 0)
            {
                GameOver = true;
                gameRunning = false;
                NextScene.Instance.GameOverScene();
            }
            if (Input.GetKeyDown(KeyCode.Escape) && level > 0 && level <= 3 && gameRunning)
            {
                if (!Lanes.Instance.isPaused && !trackEnded())
                {
                    Lanes.Instance.PauseGame();
                    theSong.Pause();
                    videoPlayer.Pause();
                }
                else if (Lanes.Instance.isPaused)
                {
                    Lanes.Instance.ResumeGame();
                    theSong.Play();
                    videoPlayer.Play();
                }
            }
        }
}

