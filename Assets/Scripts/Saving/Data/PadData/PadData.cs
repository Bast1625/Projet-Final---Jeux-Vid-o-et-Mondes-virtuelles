using System;

[Serializable]
public class PadData : BaseData
{
    public bool IsOut;
    public string CurrentDocumentName;
    public int CurrentPageNumber;
}