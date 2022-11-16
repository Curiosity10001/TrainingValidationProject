using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ThunderAction : MonoBehaviour
{
    #region Collision parameters
    bool playerHit;
    #endregion

    #region Spawn parameters
    [Header("Spawn public parameter")]
    [SerializeField] int thunderLvlAtTimeX;
    [SerializeField] int mAXThunder;
    [SerializeField] float timeX;
    [SerializeField] GameObject thunder;
    [SerializeField] AnimationCurve spawninEvolutionOnTimeThunder;
    [SerializeField] float circleSpawnRadius;

    private Vector3 spawnPos;
    private float DeltaX1X2;
    private float lastSpawn;
    private float X;
    #endregion

    #region Other scripts parameters
    PlayerMove timer;
    #endregion


    private void Awake()
    {
        timer = FindObjectOfType<PlayerMove>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        X = (thunderLvlAtTimeX * timer.timer) / timeX;

        DeltaX1X2 = spawninEvolutionOnTimeThunder.Evaluate(X / timeX);

        if (timer.timer > lastSpawn + DeltaX1X2 && thunderLvlAtTimeX < mAXThunder)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        spawnPos = Random.insideUnitSphere * circleSpawnRadius;
        Instantiate(thunder, spawnPos + transform.position, Quaternion.identity, transform);
        thunderLvlAtTimeX++;
        lastSpawn = Time.time;
    }
}
