using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class HUD : MonoBehaviour
{
    [Header("Tool Container")]
    public GameObject toolContainer;
    public float timeToOpenTools = 0.5f;
    public Vector2 minimumMinAnchorTools = new(0.2f,0.1f);
    public Vector2 maximumMinAnchorTools = new(0.8f,0.1f);
    public bool toolsOpened;
    private RectTransform m_toolOptions;
    private CanvasGroup m_toolGroup;
    [Header("Encyclopedia")]
    public RectTransform encyTransform;
    public CanvasGroup encyGroup;
    public CanvasGroup prevPageButton;
    public CanvasGroup nextPageButton;
    public List<CanvasGroup> encyPages = new();
    private int m_currentPage = 0;
    public Vector2 minimumMinAnchorEncy = new(0.65f, -0.65f);
    public Vector2 minimumMaxAnchorEncy = new(0.95f,0.05f);
    public Vector2 maximumMinAnchorEncy = new(0.65f, 0f);
    public Vector2 maximumMaxAnchorEncy = new(0.95f,0.7f);
    public float encySpeed = 0.5f;
    public bool encyOpened = false;
    [Header("Fungi Fate Choice")]
    public CanvasGroup detainButton;
    public CanvasGroup releaseButton;
    public CanvasGroup confirmChoiceButton;
    [Header("Tool Result Folder")]
    public Vector2 minimumMinAnchorResults = new(0.05f,-0.5f);
    public Vector2 minimumMaxAnchorResults = new(0.6f,0.08f);
    public Vector2 maximumMinAnchorResults = new(0.05f,0f);
    public Vector2 maximumMaxAnchorResults = new(0.6f,0.58f);
    private int m_currentTab = 0;
    public RectTransform resultsFolder;
    public List<CanvasGroup> tabHeaders = new();
    public List<CanvasGroup> tabs = new();
    private bool resultsOpened = false;
    public float resultsSpeed = 0.5f;
    [Header("Guberment Panel")]
    public RectTransform gubermentPanel;
    public Vector2 minimumMinAnchorGov = new(-0.4f, 0.65f);
    public Vector2 minimumMaxAnchorGov = new(0.04f, 0.95f);
    public Vector2 maximumMinAnchorGov = new(0f, 0.65f);
    public Vector2 maximumMaxAnchorGov = new(0.44f, 0.95f);
    public float gubermentSpeed = 0.5f;
    private bool gubermentOpened = false;
    [Header("Timer")]
    public CanvasGroup timerContainer;
    public Text timerText;
    public bool timerOn = false;
    public float timerFadeSpeed = 0.3f;
    [Header("Funds")]
    public Text fundsCount;
    private int m_currentFunds = 0;
    public float fundsSpeed = 0.3f;
    [Header("Quota Counter")]
    public Text quotaCounter;
    [Header("Day Counter")]
    public Text dayCounter;
    [Header("EndDayScreen")]
    private int m_interimFunds;
    private bool m_haveCompletedSummary = false;
    public CanvasGroup endDayScreen;
    public CanvasGroup advanceButton;
    public Text caseSummary;
    public Text caseRevenue;
    public Text casePenalty;
    public Text caseToolCost;
    public Text fundSummary;
    public Text caseCountSummary;
    public Text message;

    public static HUD Instance;
    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        m_toolOptions = toolContainer.GetComponent<RectTransform>();
        m_toolGroup = toolContainer.GetComponent<CanvasGroup>();
        if (toolsOpened)
        {
            SetCanvasGroup(m_toolGroup, true);
            m_toolOptions.anchorMin = minimumMinAnchorTools;
        }
        else
        {
            SetCanvasGroup(m_toolGroup,false);
            m_toolOptions.anchorMin = maximumMinAnchorTools;
        }
        if(encyOpened)
        {
            SetCanvasGroup(encyGroup, true);
            encyTransform.anchorMin = maximumMinAnchorEncy;
            encyTransform.anchorMax = maximumMaxAnchorEncy;
        }
        else
        {
            SetCanvasGroup(encyGroup, false);
            encyTransform.anchorMin = minimumMinAnchorEncy;
            encyTransform.anchorMax = minimumMaxAnchorEncy;
        }
        if(m_currentPage == 0)
        {
            SetCanvasGroup(prevPageButton, false);
        }
        if(encyPages.Count == 0 || m_currentPage == encyPages.Count - 1)
        {
            SetCanvasGroup(nextPageButton, false);
        }
        for (int i = 0; i < encyPages.Count; i++)
        {
            SetCanvasGroup(encyPages[i], i == m_currentPage, i == m_currentPage ? 1 : 0);
        }
        CheckConfirmEligibility();
    }

    private void Start()
    {
        m_currentFunds = PlayerState.Instance.currentMoney;
    }
    public void InteractWithToolDrawer()
    {
        if(toolsOpened)
        {
            CloseTools();
        }
        else
        {
            OpenTools();
        }
        toolsOpened = !toolsOpened;
    }

    public void OpenTools()
    {
        m_toolOptions.DOAnchorMin(minimumMinAnchorTools, timeToOpenTools);
        StartCoroutine(EnableTools());
    }
    public void CloseTools()
    {
        SetCanvasGroup(m_toolGroup, false);
        m_toolOptions.DOAnchorMin(maximumMinAnchorTools, timeToOpenTools);
    }
   
    private IEnumerator EnableTools()
    {
        yield return new WaitForSeconds(timeToOpenTools);
        SetCanvasGroup(m_toolGroup, true);
    }

    public void ClickOnTool(int toolNumber)
    {
        DayManager.Instance.ChangeSelectedTool(toolNumber);
    }

    public void OpenEncyclopedia()
    {
        SetCanvasGroup(encyGroup,true);
        encyOpened = true;
        encyTransform.DOAnchorMin(maximumMinAnchorEncy, encySpeed);
        encyTransform.DOAnchorMax(maximumMaxAnchorEncy, encySpeed);
    }
    public void CloseEncyclopedia()
    {
        SetCanvasGroup(encyGroup,false);
        encyOpened = false;
        encyTransform.DOAnchorMin(minimumMinAnchorEncy, encySpeed);
        encyTransform.DOAnchorMax(minimumMaxAnchorEncy, encySpeed);
    }
    public void ClickOnEncyclopedia()
    {
        if(encyOpened)
        {
            CloseEncyclopedia();
        }
        else
        {
            OpenEncyclopedia();
        }
    }
    public void PrevPage()
    {
        m_currentPage--;
        if(m_currentPage == 0)
        {
            SetCanvasGroup(prevPageButton, false);
        }
        SetCanvasGroup(nextPageButton, true);
        SetCanvasGroup(encyPages[m_currentPage + 1],false,0);
        SetCanvasGroup(encyPages[m_currentPage], true);
    }
    public void NextPage()
    {
        m_currentPage++;
        if (m_currentPage == encyPages.Count - 1)
        {
            SetCanvasGroup(nextPageButton,false);
        }
        SetCanvasGroup(prevPageButton, true);
        SetCanvasGroup(encyPages[m_currentPage - 1], false, 0);
        SetCanvasGroup(encyPages[m_currentPage],true);
    }

    public void SetCanvasGroup(CanvasGroup group, bool state, int alpha = 1)
    {
        group.alpha = alpha;
        group.interactable = state;
        group.blocksRaycasts = state;
    }
    public void ClickOnShroomInEncyclopedia(int ID)
    {
        DayManager.Instance.ChangeSelectedFungi(ID);
        DayManager.Instance.chosenFungi = true;
        CheckConfirmEligibility();
    }

    public void ChooseDetain()
    {
        DayManager.Instance.chosenFate = true;
        CheckConfirmEligibility();
        SetCanvasGroup(detainButton, false);
        SetCanvasGroup(releaseButton, true);
    }
    public void ChooseRelease()
    {
        DayManager.Instance.chosenFate = true;
        CheckConfirmEligibility();
        SetCanvasGroup(detainButton, true);
        SetCanvasGroup(releaseButton, false);
    }

    public void ResetFateChoice()
    {
        DayManager.Instance.chosenFate = false;
        SetCanvasGroup(detainButton, true);
        SetCanvasGroup(releaseButton, true);
        CheckConfirmEligibility();
    }

    public void ConfirmChoice()
    {
        DayManager.Instance.EvaluateChoice();
    }
    public void CheckConfirmEligibility()
    {
        if(DayManager.Instance.chosenFate && DayManager.Instance.chosenFungi && DayManager.Instance.hasActiveCase)
        {
            SetCanvasGroup(confirmChoiceButton,true);
        }
        else
        {
            SetCanvasGroup(confirmChoiceButton, false);
        }
    }

    public void ClickOnResultsFolder()
    { 
        if(resultsOpened)
        {
            CloseResults();
        }
        else
        {
            OpenResults();
        }
    }
    public void OpenResults()
    {
        resultsFolder.DOAnchorMin(maximumMinAnchorResults, resultsSpeed);
        resultsFolder.DOAnchorMax(maximumMaxAnchorResults,resultsSpeed);
        resultsOpened = true;

    }
    public void CloseResults()
    {
        resultsFolder.DOAnchorMin(minimumMinAnchorResults, resultsSpeed);
        resultsFolder.DOAnchorMax(minimumMaxAnchorResults, resultsSpeed);
        resultsOpened = false;
    }
    public void ClickOnTab(int id)
    {
        SetCanvasGroup(tabHeaders[id], false);
        SetCanvasGroup(tabHeaders[m_currentTab], true);
        SetCanvasGroup(tabs[id], false, 1);
        SetCanvasGroup(tabs[m_currentTab], false, 0);
        m_currentTab = id;
    }
    public void RevealTab(int id)
    {
        SetCanvasGroup(tabHeaders[id], m_currentTab != id, 1);
    }

    public void ClickOnGuberment()
    {
        if(gubermentOpened)
        {
            CloseGuberment();
        }
        else
        {
            OpenGuberment();
        }
    }

    public void OpenGuberment()
    {
        gubermentPanel.DOAnchorMin(maximumMinAnchorGov, gubermentSpeed);
        gubermentPanel.DOAnchorMax(maximumMaxAnchorGov, gubermentSpeed);
        gubermentOpened = true;
    }
    
    public void CloseGuberment()
    {
        gubermentPanel.DOAnchorMin(minimumMinAnchorGov, gubermentSpeed);
        gubermentPanel.DOAnchorMax(minimumMaxAnchorGov, gubermentSpeed);
        gubermentOpened = false;
    }

    public void UpdateFundsCount()
    {
        DOVirtual.Int(m_currentFunds, PlayerState.Instance.currentMoney, fundsSpeed, (v) => fundsCount.text = v.ToString() + "€");
        m_currentFunds = PlayerState.Instance.currentMoney;
        //DOTween.To(() => m_currentFunds, x => m_currentFunds = x ,PlayerState.Instance.currentMoney, fundsSpeed);
    }

    public void EnableTimer()
    {
        timerOn = true;
        timerContainer.DOFade(1, timerFadeSpeed);
    }

    public void DisableTimer()
    {
        timerOn = false;
        timerContainer.DOFade(0, timerFadeSpeed);
    }

    private void Update()
    {
        if (timerOn)
        {
            TimeSpan time = TimeSpan.FromSeconds(Mathf.Max(0,DayManager.Instance.timerStart  - Time.time + DayManager.Instance.timerLength));
            timerText.text = time.ToString("hh':'mm':'ss");
        }
    }

    public void UpdateQuota() 
    {
        quotaCounter.text = Mathf.Min(PlayerState.Instance.dayStats.Count,DayManager.Instance.currentQuota).ToString() + "/" + DayManager.Instance.currentQuota;
    }

    public void UpdateDay()
    {
        dayCounter.text = "Day #" + DayManager.Instance.currentDay.ToString();
    }

    public void TestNumbers()
    {
        DayManager.Instance.quotaCompleted = true;
        EnableTimer();
        DayManager.Instance.timerStart = Time.time;
        PlayerState.Instance.UpdateCurrentMoney(-17);
    }

    public void SupremeButtonForFilip()
    {
        DayManager.Instance.creator.GenerateCase();
    }

    public void StartResults()
    {
        DisableTimer();
        
    }
}
