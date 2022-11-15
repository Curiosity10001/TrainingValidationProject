using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    #region Retrieved On Awake
    PlayerMove[] player;
    GameObject canvas;
    #endregion

    [Header (" UI Called")]
    [SerializeField] Button retry;

    void Awake()
    {
        player = FindObjectsOfType<PlayerMove>();
        canvas = GameObject.Find("CanvasGameOver");
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

    //Stops time in game and calls game over interfacce
    public void DestroyPlayer( GameObject PlayerToDestroy)
    {
        Destroy(PlayerToDestroy);
        Time.timeScale = 0;
        canvas.SetActive(true);
        retry.onClick.AddListener(RestartGame);
    }
    public void RestartGame()
    {
        //Restart game/level
        SceneManager.LoadScene("InitialScene");
        Time.timeScale = 1;
    }
   
}
