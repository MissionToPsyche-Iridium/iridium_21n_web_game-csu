using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class Manager : MonoBehaviour
{
    public static Manager Instance;
    public Lanes[] lanes;
    public AudioSource theSong;
    public double marginOfError;
    private string[] midiName = { "day50.mid", "day69.mid",  "day15.mid" };
    private string[] midiNameNASA = {"ascension.mid", "breathless.mid", "degasparis.mid"};
    public float spawnYCoordinate;
    public float tapYCoordinate;
    public static bool gameRunning;
    public bool readFromWeb;
    public static int level = 0;
    public static int midiLevel = 0;
    public AudioClip[] clip;
    public Image healthBar;
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
    public int finalScore;
    public int scorePerHit = 100;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiplierText;
    public int finalMultiplier;
    public int multiplierTracker;
    public int[] multiplierThresh;
    public TextMeshProUGUI scorePopText;
    public TextMeshProUGUI goalScore;
    private int goalScoreLevel;
 

    void Start()
    {
        scorePopText.gameObject.SetActive(false);
        scoreText.text = "Score: 0";
        multiplierText.text = "Multiplier: x1";
        finalMultiplier = 1;
        multiplierThresh = new int[] { 2, 4, 6 };
        tapYCoordinate = -271;
        spawnYCoordinate = 400;
        marginOfError = 0.25;
        Instance = this;
        if(NASACollection)
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
        if (NASACollection)
        {
            if (level == 0)
            {
                goalScoreLevel = 20000;
            }
            if (level == 1)
            {
                goalScoreLevel = 37500;
            }
            if (level == 2)
            {
                goalScoreLevel = 32000;
            }
        }
        if (!NASACollection)
        {
            if (level == 0)
            {
                goalScoreLevel = 80000;
            }
            if (level == 1)
            {
                goalScoreLevel = 55000;
            }
            if (level == 2)
            {
                goalScoreLevel = 55000;
            }
        }

        goalScore.text = "Goal: " + goalScoreLevel.ToString();

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
        theSong.Play();     
        gameRunning = true;
        level++;
    }

    public void hideTextPop()
    {
        scorePopText.gameObject.SetActive(false);
    }
    public static void Hit()
    {
        pointStreak++;
        if(Instance.finalMultiplier == 1)
        {
            Instance.scorePopText.text = "Good!";
        }
        else if(Instance.finalMultiplier == 2)
        {
            Instance.scorePopText.text = "Great!";
        }
        else if(Instance.finalMultiplier == 4)
        {
            Instance.scorePopText.text = "Perfect!";
        }
        
        Instance.scorePopText.gameObject.SetActive(true);
        Instance.Invoke(nameof(hideTextPop), 0.2f);

        Instance.gainFuel();
        Instance.finalScore += Instance.scorePerHit * Instance.finalMultiplier;
        Instance.scoreText.text = "Score: " + Instance.finalScore;
        Instance.multiplierText.text = "Multiplier: x" + Instance.finalMultiplier;
        
        if (Instance.finalMultiplier - 1 < Instance.multiplierThresh.Length)
        {
            Instance.multiplierTracker++;

            if (Instance.multiplierThresh[Instance.finalMultiplier - 1] <= Instance.multiplierTracker)
            {
                Instance.multiplierTracker = 0;
                Instance.finalMultiplier++;
            }
        }

    }

    public static void Miss()
    {
        pointStreak = 0;
        Instance.finalMultiplier = 1;
        Instance.multiplierTracker = 0;
        Instance.multiplierText.text = "Multiplier: x" + Instance.finalMultiplier;
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

    public void callGameOver()
    {
        GameOver = true;
        gameRunning = false;
        NextScene.Instance.GameOverScene();
    }

    public void callNextScene()
    {
        videoPlayer.Pause();
        NextScene.savedTime = videoPlayer.time;
        if (gameRunning)
        {
            NextScene.Instance.nextScene();
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
                    if(level == 1 && finalScore > goalScoreLevel)
                    {
                        callNextScene();
                    }
                    else if(level == 2 && finalScore > goalScoreLevel)
                    {
                        callNextScene();
                    }
                    else if (level == 3 && finalScore > goalScoreLevel)
                    {
                        callNextScene();
                    }
                    else
                    {
                        callGameOver();
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
                callGameOver();
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

