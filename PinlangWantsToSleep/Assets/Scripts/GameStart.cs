using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    #region Retrieved On Awake
    GameObject canvas;
    #endregion

    [Header(" UI Called")]
    [SerializeField] Button play;
    [SerializeField] Button controls;

    private void Awake()
    {
        canvas = GameObject.Find("CanvasStart");
        Time.timeScale = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         play.onClick.AddListener(startGame);
      
    }
    public void startGame()
    {
        canvas.SetActive(false); 
        Time.timeScale = 1;
    }
}
