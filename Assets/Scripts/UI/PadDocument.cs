using UnityEngine;

public class PadDocument : MonoBehaviour
{
    public PadDocumentPage[] pages;

    public int CurrentPageNumber { get; protected set; } = 1;
    public int TotalPageNumber { get => GetTotalPage(); }

    public PadDocumentPage CurrentPage { get; private set; }

    public virtual void Initialize(GameManager gameManager) { }

    public virtual int GetTotalPage() => pages.Length;
    public virtual void SetPage(int pageNumber)
    {
        if(CurrentPage != null)
            CurrentPage.gameObject.SetActive(false);

        CurrentPageNumber = pageNumber;

        CurrentPage = pages[CurrentPageNumber - 1];

        CurrentPage.gameObject.SetActive(true);
    }

    public virtual void Refresh() { }
}