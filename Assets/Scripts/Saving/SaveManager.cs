using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static DataFileHandler dataFileHandler = new DataFileHandler();

    public static void Save()
    {
        GameData gameData = new GameData();

        ISavable[] savables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISavable>().ToArray();

        foreach(ISavable savable in savables)
            savable.Save(gameData);

        dataFileHandler.Save(gameData);
    }

    public static void Load()
    { 
        dataFileHandler.Load(out GameData gameData);

        ISavable[] savables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISavable>().ToArray();

        foreach(ISavable savable in savables)
            savable.Load(gameData);

    }
}