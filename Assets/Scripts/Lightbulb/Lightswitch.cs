using System;
using UnityEngine;

public class Lightswitch : Interactable, ITaskable, ISavable
{
    public event Action OnTurnOn;
    public event Action OnTurnOff;

    public string Id { get => $"{GetType()}_{name}"; }

    [SerializeField] bool defaultState;
    [SerializeField] Lightbulb[] lightbulbs;

    public bool IsOn { get; private set; }
    public bool IsOff { get => !IsOn; }

    protected override void Initialize()
    {
        base.Initialize();

        if (defaultState)
            TurnOn();
        else
            TurnOff();
    }

    public void TurnOn()
    {
        IsOn = true;

        foreach(Lightbulb lightbulb in lightbulbs)
            lightbulb.TurnOn();

        OnTurnOn?.Invoke();
    }

    public void TurnOff()
    {
        IsOn = false;

        foreach (Lightbulb lightbulb in lightbulbs)
            lightbulb.TurnOff();

        OnTurnOff?.Invoke();
    }

    protected override void InteractionStart(Player interactor)
    {
        if (IsOn)
            TurnOff();
        else
            TurnOn();
    }

    public override InteractionFeedback Feedback(Player interactor)
    {
        return new InteractionFeedback
        {
            Status = InteractionStatus.Valid,
            Message = $"Turn {(IsOn ? "Off" : "On")} the lights"
        };
    }

    public void Save(GameData gameData)
    {
        LightswitchData data = new LightswitchData
        {
            Id = Id,

            IsOn = IsOn,
        };

        gameData.LightswitchesData.Add(data);
    }

    public void Load(GameData gameData)
    {
        LightswitchData data = gameData.LightswitchesData.Find(_data => _data.Id == Id);

        IsOn = data.IsOn;

        if(IsOn)
            TurnOn();
        else
            TurnOff();
    }
}