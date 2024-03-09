using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;
    public GameObject freezerLine;
    [Header("Quota Information")]
    public int currentQuota;
    public bool quotaCompleted;
    public int minimumQuota = 4;
    public int maximumQuota = 8;
    [Header("Timer Information")]
    private float timerStart;
    public float timerLength;
    [Header("Events")]
    public int priceIncrease = 0;
    public int rateOfPriceIncrease = 2;
    public int maxDesired = 3;
    public float desiredProbability = 0.02f;
    public int fungiVariationCount = 10;
    public Dictionary<int, bool> currentDesiredFungi = new();


    [HideInInspector]
    public int currentRevenue;
    public int currentPenalty;
    public int currentToolCost;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        for (int i = 0; i < fungiVariationCount; i++)
        {
            currentDesiredFungi.Add(i, false);
        }
    }


    public void GetNewOrgan()
    {
        //Make freezerLine move
        //Generate new organ
        currentPenalty = 0;
        currentToolCost = 0;
    }

    public void Update()
    {
        if(quotaCompleted && Time.time - timerLength > timerLength)
        {
            //Add call for result screen
            quotaCompleted = false;
        }
    }

    public void GenerateNewEvents()
    {
        int desiredFungiCount = 0;
        for (int i = 0; i < fungiVariationCount; i++)
        {
            if(desiredFungiCount < maxDesired && Random.value < desiredProbability)
            {
                desiredFungiCount++;
                currentDesiredFungi[i] = true;
            }
            else
            {
                currentDesiredFungi[i] = false;
            }
        }
    }

    public void HandOffOrgan()
    {
        PlayerState.Instance.dayStats.Add(new(currentRevenue,currentPenalty,currentToolCost));
        if(PlayerState.Instance.dayStats.Count == currentQuota)
        {
            quotaCompleted = true;
            timerStart = Time.time;
        }
    }

    public void StartNewDay()
    {
        priceIncrease += rateOfPriceIncrease;
        quotaCompleted = false;
        currentQuota = Random.Range(minimumQuota, maximumQuota + 1);
        GenerateNewEvents();
        PlayerState.Instance.StartNewDay();

    }
}
