using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CaseReport 
{
    public int Income;
    public int Penalty;
    public int ToolLoss;

    public CaseReport(int income, int penalty, int toolloss)
    {
        Income = income;
        Penalty = penalty;
        ToolLoss = toolloss;
    }
}

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance;
    public int startingMoney;
    public int currentMoney;
    public List<CaseReport> dayStats = new();
    
    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void StartNewDay()
    {
        startingMoney = currentMoney;
        dayStats.Clear();
    }

    public void FinishCase(CaseReport newCase)
    {
        dayStats.Add(newCase);
    }
    
    public void SpendOnTool(int amount)
    {
        UpdateCurrentMoney(-amount);
        DayManager.Instance.currentToolCost += amount;
    }
    public void IncreasePenalty(int amount)
    {
        UpdateCurrentMoney(-amount);
        DayManager.Instance.currentPenalty += amount;
    }
    
    public void UpdateCurrentMoney(int change)
    {
        currentMoney += change;
        HUD.Instance.UpdateFundsCount();
    }
}
