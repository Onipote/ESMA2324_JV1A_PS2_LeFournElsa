using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.UI;

public class PlayerCoatSystem : MonoBehaviour
{
    public static PlayerCoatSystem instance;

    [Header("Coat system")]
    [SerializeField] private Light2D coat;
    [SerializeField] private float baseIntensity = 1f;
    [SerializeField] private float currentIntensity;
    [SerializeField] private float baseRange = 3f;
    [SerializeField] private float currentRange;
    public float lostLight;

    [Header("Dark timer")]
    [SerializeField] private Transform startRespawn;
    [SerializeField] private float darkTimerReset = 20f;
    public bool inDarkness = false;
    public float darkTimer;
    public float timeRemaining;
    private bool isDarkTimerRunning = false;

    [Header("Dark timer UI")]
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Image lightTimer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        coat = GetComponent<Light2D>();

        if (coat == null)
        {
            Debug.LogError("Light2D component not found!");
            return;
        }

        lostLight = 0;
        currentIntensity = baseIntensity;
        currentRange = baseRange;
        darkTimer = darkTimerReset;

        coat.intensity = currentIntensity;
        coat.pointLightOuterRadius = currentRange;
        coat.enabled = true;
    }

    private void Update()
    {
        if (currentIntensity <= 0 && currentRange <= 0)
        {
            inDarkness = true;
            PlayerMovements.instance.currentSpeed = PlayerMovements.instance.walkSpeed;
            Debug.Log("Ew...I need light !");
            Debug.Log("indarkness :"+inDarkness+"isDarkTimerRunning"+isDarkTimerRunning);

            if (inDarkness && !isDarkTimerRunning)
            {
                StartTimer(darkTimer);
            }

            if (isDarkTimerRunning)
            {
                if (timeRemaining > 0 && inDarkness)
                {
                    timeRemaining -= Time.deltaTime;
                    int seconds = Mathf.FloorToInt(timeRemaining % 60);
                    timerText.text = string.Format("{0}", seconds);

                }
                else if (timeRemaining <= 0 && inDarkness)
                {
                    timeRemaining = 0;
                    timerText.text = timeRemaining.ToString();

                    TimerEndedRespawn();
                }
                else if (timeRemaining > 0 && !inDarkness)
                {
                    TimerEndedHappyEnd();
                }
            }
        }
    }
    
    public void AddWotw()
    {
        currentIntensity++; //wotwCounter +1 => currentIntensity +1
        coat.intensity = currentIntensity; //change intensity

        currentRange = currentRange + (PlayerMovements.instance.wotwCounter * 2);
        coat.pointLightOuterRadius = currentRange; //change outer radius
    }

    public void RemoveWotw()
    {
        currentIntensity = Mathf.Clamp(currentIntensity - lostLight, 0, 1000);
        coat.intensity = currentIntensity;

        currentRange = Mathf.Clamp(currentRange - lostLight, 0, 1000);
        coat.pointLightOuterRadius = currentRange;

        PlayerMovements.instance.wotwCounter = Mathf.Clamp(PlayerMovements.instance.wotwCounter - lostLight, 0, 1000);
    }

    public void StartTimer(float duration)
    {
        timeRemaining = duration;
        isDarkTimerRunning = true;
    }

    private void TimerEndedRespawn()
    {
        

        //Respawn at the beginning (game over) because not out of darkness
        PlayerMovements.instance.rb.transform.position = startRespawn.transform.position;

        //Reset light properties
        currentIntensity = baseIntensity;
        Debug.Log("intensite :" + currentIntensity);
        currentRange = baseRange;
        coat.intensity = currentIntensity;
        coat.pointLightOuterRadius = currentRange;
        isDarkTimerRunning = false;
        inDarkness = false;
        Debug.Log("GAME OVER");
        darkTimer = darkTimerReset;
        timeRemaining = darkTimer;
        timerText.text = timeRemaining.ToString();
    }
    private void TimerEndedHappyEnd()
    {
        //Timer stopped because out of darkness
        isDarkTimerRunning = false;
        inDarkness = false;
        Debug.Log("Hehe.. Too easy !");
        darkTimer = darkTimerReset;
        timeRemaining = darkTimer;
    }
}