using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarryNight : MonoBehaviour
{

    #region Spawn parameters
    [Header("Spawn public parameter")]
    [SerializeField] int starNumnberAtTimeX;
    [SerializeField] int maxStars;
    [SerializeField] float timeX;
    [SerializeField] GameObject star;
    [SerializeField] AnimationCurve spawninEvolutionOnTimeStars;
    [SerializeField] float circleSpawnRadius;

    private Vector3 radiusOffset;
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
        X = (starNumnberAtTimeX * timer.timer) / timeX;

        DeltaX1X2 = spawninEvolutionOnTimeStars.Evaluate(X / timeX);

        if (timer.timer > lastSpawn + DeltaX1X2 && starNumnberAtTimeX < maxStars)
        {
            Spawn();
        }
    }

    private void Spawn()
    {

        radiusOffset = Random.onUnitSphere * Random.Range(10, 120);
        spawnPos = radiusOffset * circleSpawnRadius;
        Instantiate(star, spawnPos + transform.position, Quaternion.identity);
        starNumnberAtTimeX++;
        lastSpawn = Time.time;
    }
}
