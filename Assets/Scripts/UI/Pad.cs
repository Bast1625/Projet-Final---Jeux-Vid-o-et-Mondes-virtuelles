using System;
using System.Linq;
using UnityEngine;

public class Pad : MonoBehaviour, ISavable
{
    public event Action OnPadShow;
    public event Action OnPadHide;

    public string Id { get => $"{GetType()}_{name}"; }
    public PadDocument[] documents;
    
    public int CurrentPageNumber { get; private set; } = 1;
    public int TotalPage { get => currentDocument.GetTotalPage(); }
    
    public bool IsOut { get; private set; }

    private PadDocument currentDocument;

    public void Initialize(GameManager gameManager)
    {
        foreach(PadDocument document in documents)
        {
            document.Initialize(gameManager);

            document.gameObject.SetActive(false);
        }

        SwitchDocument(documents[0]);

        GameManager.OnGameStart += () => { SwitchDocument("taskDocument"); Show(); };
        GameManager.OnEndGameStart += () => { 
            SwitchDocument("endGameStartDocument"); 
            Show(); 

            OnPadHide += ShowEndGameTasks;
        };
        GameManager.OnEndGameEnd += () => { SwitchDocument("endGameEndDocument"); Show(); };
        GameManager.OnGameOver += () => { SwitchDocument("gameOverDocument"); Show(); };
        GameManager.OnGameEnd += () => { SwitchDocument("gameEndDocument"); Show(); };
    }

    public void SwitchDocument(PadDocument documentToDisplay)
    {
        if(currentDocument != null)
            currentDocument.gameObject.SetActive(false);

        currentDocument = documentToDisplay;

        currentDocument.gameObject.SetActive(true);

        currentDocument.SetPage(1);
    }
    public void SwitchDocument(string documentToDisplayName)
    {
        PadDocument documentToDisplay = documents.ToList().Find(document => document.name == documentToDisplayName);

        SwitchDocument(documentToDisplay);
    }
    public void SwitchDocument(int documentToDisplayIndex)
    {
        PadDocument documentToDisplay = documents[documentToDisplayIndex];

        SwitchDocument(documentToDisplay);
    }

    public void Show()
    {
        IsOut = true;

        gameObject.SetActive(true);

        Refresh();

        OnPadShow?.Invoke();
    }

    public void Hide()
    {
        IsOut = false;

        gameObject.SetActive(false);

        OnPadHide?.Invoke();
    }

    public void NextPage() 
    {
        if( !IsOut )
            return;

        CurrentPageNumber++;

        if(CurrentPageNumber > TotalPage)
            CurrentPageNumber = 1;
        
        currentDocument.SetPage(CurrentPageNumber);
        Refresh();
    }
    public void PreviousPage()
    {
        if( !IsOut )
            return;

        CurrentPageNumber--;

        if(CurrentPageNumber < 1)
            CurrentPageNumber = TotalPage;
        
        currentDocument.SetPage(CurrentPageNumber);
        Refresh();
    }
    
    public void Refresh()
    {
        if( !IsOut )
            return;

        currentDocument.Refresh();
    } 

    private void ShowEndGameTasks() 
    {
        OnPadHide -= ShowEndGameTasks;

        SwitchDocument("taskDocument");

        Show();
    }

    public void Save(GameData gameData)
    {
        PadData data = new PadData
        {
            IsOut = IsOut,

            CurrentDocumentName = currentDocument.name,
            CurrentPageNumber = CurrentPageNumber
        };

        gameData.PadData = data;
    }

    public void Load(GameData gameData)
    {
        PadData data = gameData.PadData;        

        SwitchDocument(data.CurrentDocumentName);
        
        CurrentPageNumber = data.CurrentPageNumber;
        currentDocument.SetPage(CurrentPageNumber);

        IsOut = data.IsOut;
        if(IsOut)
            Show();
        else
            Hide();

        Refresh();
    }
}