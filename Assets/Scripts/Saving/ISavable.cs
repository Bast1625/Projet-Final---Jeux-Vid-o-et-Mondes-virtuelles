public interface ISavable : IIdentifiable
{
    public void Save(GameData gameData);
    public void Load(GameData gameData);
}