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
    public Vector2 startResultAnchor = new Vector2(0.1f, 0.1f);
    public Vector2 tabSize = new Vector2(0.12f, 0.1f);
    public List<string> tabTitles = new ()
    { 
        "Incision",
        "Grown body",
        "UV colour",
        "Spores",
        "Blood"
    };  


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
}
