using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGameCanvas : MonoBehaviour 
{

    #region Retrieved On Awake
    PlayerMove playerMove;
    ScoreManager music;
    GameObject canvas;
    #endregion

    [Header(" UI Called")]
    [SerializeField] Button start;


    private void Awake()
    {
        canvas = GameObject.Find("StartCanvas");
        playerMove = FindObjectOfType<PlayerMove>();
        music = GetComponent<ScoreManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        playerMove.gameObject.GetComponent<PlayerMove>().enabled = false;
        Time.timeScale = 0.5f;
        start.onClick.AddListener(StartGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame()
    {
        //start game/level
        SceneManager.LoadScene("LevelOne");
        playerMove.gameObject.GetComponent<PlayerMove>().enabled = true;
        music.gameObject.GetComponent<AudioSource>().enabled = true;
        Time.timeScale = 1;
    }
}
