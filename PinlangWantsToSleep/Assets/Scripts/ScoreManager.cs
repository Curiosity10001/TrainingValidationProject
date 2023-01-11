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
        capturedscripts = FindObjectsOfType<RewardBestowment>();
        canvas = GameObject.Find("WinCanvas");
        initialScene.onClick.AddListener(InitialScene);
        nextLevel.onClick.AddListener(NextLevel);
        canvas.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


  
    }

    public void Sleepiness()
    {
         sleepiness += maxSleepiness / (capturedscripts.Length );
         playerSleepinessBar.fillAmount = sleepiness;
        if (sleepiness == maxSleepiness)
        {
            Time.timeScale = 0;
            Debug.Log("in here");
            canvas.SetActive(true);
        }

    }
    public void InitialScene()
    {
        //goes back to initial page
        SceneManager.LoadScene("InitialScene");   
    }

    public void NextLevel()
    {
        //for now replay level one
        SceneManager.LoadScene("LevelOne");
    }
}
