using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;


public class ScoreManager : MonoBehaviour
{
    #region Other scripts parameters
    RewardBestowment[] capturedscripts;
    public AudioSource music;
    MusicOnOFF volume;
    #endregion

    [Header("Score Card parameters")]
    public Image playerSleepinessBar;

    float sleepiness;
    float maxSleepiness = 1;

    GameObject canvas;
    [SerializeField] Button initialScene;
    [SerializeField] Button nextLevel;
   

    private void Awake()
    {
        capturedscripts = FindObjectsOfType<RewardBestowment>(false);
        canvas = GameObject.Find("WinCanvas");
        initialScene.onClick.AddListener(InitialScene);
        nextLevel.onClick.AddListener(NextLevel);
        canvas.SetActive(false);
        volume = FindObjectOfType<MusicOnOFF>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(canvas.name);

        if(sleepiness >= maxSleepiness)
        {
            Time.timeScale = 0;
           
            canvas.SetActive(true);
            music.enabled = false;
            if (volume.musicOn)
            {
                canvas.GetComponent<AudioSource>().volume = 0.5f;
            } else canvas.GetComponent<AudioSource>().volume = 0.0f;

        }


    }

    public void Sleepiness()
    {
         sleepiness += maxSleepiness / ((float)capturedscripts.Length );
         playerSleepinessBar.fillAmount = sleepiness;
        
       
    }
    public void InitialScene()
    {
        //goes back to initial page
        SceneManager.LoadScene("InitialScene");
        Time.timeScale = 0.5f;
        music.enabled = false;
    }

    public void NextLevel()
    {
        //for now replay level one
        SceneManager.LoadScene("LevelOne");
        Time.timeScale = 1;
        music.enabled = true;
    }
}
