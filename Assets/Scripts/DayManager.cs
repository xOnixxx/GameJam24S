using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FungiFate 
{
    None,
    Detain,
    Release
}
public class DayManager : MonoBehaviour
{
    [Header("General Settings")]
    public int currentDay = 0;
    public int successPayment = 10;
    public int failurePenalty = 15;
    public int governmentMultiplier = 2;
    public static DayManager Instance;
    public CaseCreator creator;
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
    public (Fungi, Organ) currentCase;

    [HideInInspector]
    public int currentPenalty = 0;
    [HideInInspector]
    public int currentToolCost = 0;

    private int m_selectedTool;
    private int m_fungiID;
    [Header("Tools")]
    public List<Tool> toolSelection = new();
    public FungiFate currentChoiceOfFate;
    public Tool currentSelectedTool { get { return toolSelection[m_selectedTool]; } }

    public bool chosenFate = false;
    public bool chosenFungi = false;
    public bool hasActiveCase = false;

    [HideInInspector]
    public bool UVOn = false;
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
        currentCase = creator.GenerateCase();
        currentPenalty = 0;
        currentToolCost = 0;
    }

    public void Update()
    {
        if(quotaCompleted && Time.time - timerStart > timerLength)
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

    public void HandOffOrgan(int income,int penalty,int toolcost)
    {
        PlayerState.Instance.dayStats.Add(new(income,penalty,toolcost));
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
        UVOn = false;
        currentQuota = Random.Range(minimumQuota, maximumQuota + 1);
        GenerateNewEvents();
        PlayerState.Instance.StartNewDay();

    }

    public void ChangeSelectedTool(int index)
    {
        m_selectedTool = index;
    }
    public void ChangeSelectedFungi(int id)
    {
        m_fungiID = id;
    }

    public void EvaluateChoice()
    {
        if(!chosenFate || !chosenFungi || !hasActiveCase)
        {
            return;
        }
        int income = 0;
        int penalty = 0;
        if(m_fungiID == currentCase.Item1.ID)
        {
            if(currentDesiredFungi[m_fungiID])
            {
                if(currentChoiceOfFate == FungiFate.Detain)
                {
                    income = successPayment * governmentMultiplier;
                    penalty = currentPenalty;
                }
                else
                {
                    income = 0;
                    penalty = currentPenalty + failurePenalty * governmentMultiplier;
                }
            }
            else
            {
                if(currentCase.Item1.acceptedOrgans.Contains(currentCase.Item2.organType))
                {
                    if(currentChoiceOfFate == FungiFate.Release)
                    {
                        income = successPayment;
                        penalty = currentPenalty;
                    }
                    else
                    {
                        income = 0;
                        penalty = currentPenalty + failurePenalty;
                    }
                }
                else
                {
                    if(currentChoiceOfFate == FungiFate.Detain)
                    {
                        income = successPayment;
                        penalty = currentPenalty;
                    }
                    else
                    {
                        income = 0;
                        penalty = currentPenalty + failurePenalty;
                    }
                }
            }
        }
        HandOffOrgan(income, penalty, currentToolCost);
        HUD.Instance.ResetFateChoice();
        chosenFungi = false;
        hasActiveCase = false;
    }
}
