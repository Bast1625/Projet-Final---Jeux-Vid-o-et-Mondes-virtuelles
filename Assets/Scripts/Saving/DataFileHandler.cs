using System.IO;
using UnityEngine;

public class DataFileHandler
{
    private readonly string path = Application.persistentDataPath + "/UnityGameData.txt";

    public void Save(GameData gameData)
    {
        string dataToJSON = JsonUtility.ToJson(gameData);
       
        File.WriteAllText(path, dataToJSON);

        //Debug.Log(dataToJSON);
    }

    public void Load(out GameData gameData)
    {
        string jsonToData = File.ReadAllText(path);

        gameData = JsonUtility.FromJson<GameData>(jsonToData);
    }
}