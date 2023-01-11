using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MusicOnOFF : MonoBehaviour
{

    AudioSource[] audioSource;
    [SerializeField] Button volumeButton;
    TMP_Text text;
    public bool musicOn = true;

    private void Awake()
    {
        
        volumeButton.onClick.AddListener(MusicOff);
        text = volumeButton.GetComponentInChildren<TMP_Text>();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MusicON()
    {
        musicOn = true;
        audioSource = FindObjectsOfType<AudioSource>();
        for (int i = 0; i <= audioSource.Length-1; i++)
        {
            audioSource[i].volume = 0.5f;
        }
        
        text.SetText("ON");
        text.fontStyle = FontStyles.Bold;

        volumeButton.onClick.RemoveListener(MusicON);
        volumeButton.onClick.AddListener(MusicOff);
    }
    public void MusicOff()
    {
        musicOn = false;   
        audioSource = FindObjectsOfType<AudioSource>(true);
        for (int i = 0; i <= audioSource.Length - 1; i++)
        {
            audioSource[i].volume = 0.0f;
        }
        
        text.SetText("OFF");
        text.fontStyle = FontStyles.Bold;

        volumeButton.onClick.RemoveListener(MusicOff);
        volumeButton.onClick.AddListener(MusicON);
    }
}
