using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class ScoreManager : MonoBehaviour
{
    #region Other scripts parameters
    RewardBestowment[] capturedscripts;
    RewardBestowment capturedBool;
    #endregion

    [Header("Score Card parameters")]
    public Image playerSleepinessBar;

    float sleepiness;
    float maxSleepiness = 1;

    private void Awake()
    {
        capturedscripts = FindObjectsOfType<RewardBestowment>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(sleepiness);
        
    }

    public void Sleepiness()
    {
         sleepiness += maxSleepiness / (capturedscripts.Length );
         playerSleepinessBar.fillAmount = sleepiness;
         if(sleepiness == maxSleepiness)
        {

        }
    }
}
