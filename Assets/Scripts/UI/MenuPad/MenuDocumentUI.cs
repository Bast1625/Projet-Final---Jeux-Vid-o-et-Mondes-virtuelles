using UnityEngine;

public class MenuDocumentUI : PadDocument
{  
    [SerializeField] PadDocumentHeader header;

    public override void Initialize(GameManager gameManager)
    {
        foreach(PadDocumentPage page in pages)
            page.gameObject.SetActive(false);

        SetPage(1);
    }

    public override void Refresh()
    {
        header.Refresh(this);
    }
}