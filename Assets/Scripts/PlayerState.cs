using System.Collections.Generic;
using UnityEngine;

public struct CaseReport 
{
    int Income;
    int Penalty;
    int ToolLoss;

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
    public int selectedTool;
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
        currentMoney -= amount;
        DayManager.Instance.currentToolCost += amount;
    }
}