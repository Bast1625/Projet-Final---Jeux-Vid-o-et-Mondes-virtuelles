using UnityEngine;
using TMPro;

public class PadDocumentHeader : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pageLabel;

    public void Initialize()
    {
        SetPageNumber(1, 1);
    }

    public void Refresh(PadDocument padDocument)
    {
        SetPageNumber(padDocument.CurrentPageNumber, padDocument.TotalPageNumber);
    }

    public void SetPageNumber(int currentPageNumber, int pageCount)
    {
        pageLabel.text = $"Page ({currentPageNumber}/{pageCount})";
    }
}