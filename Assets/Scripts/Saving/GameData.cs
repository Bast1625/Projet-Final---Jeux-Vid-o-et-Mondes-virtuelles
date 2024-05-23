using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public PlayerData PlayerData;
    public TaskManagerData TaskManagerData;
    
    public PadData PadData;

    public List<ChairData> ChairsData = new();
    public List<DoorData> DoorsData = new();
    public List<LightbulbData> LightbulbsData = new();
    public List<LightswitchData> LightswitchesData = new();
}