using System;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class Lightbulb : Interactable, ITaskable, ISavable
{
    public event Action OnTurnOn;
    public event Action OnTurnOff;

    public event Action OnBreak;
    public event Action OnFix;

    public string Id { get => $"{GetType()}_{name}"; }
    
    [SerializeField] new Light light;

    [SerializeField] Animator animator;

    [SerializeField] AudioSource lightBreaksSound;
    [SerializeField] ParticleSystem sparkParticles; 

    public bool IsOn { get; private set; }
    public bool IsOff { get => !IsOn; }

    public bool IsBroken { get; private set; }
    public bool IsFixed { get => !IsBroken; }

    public void TurnOn()
    {
        IsOn = true;

        light.gameObject.SetActive(true && IsFixed);

        OnTurnOn?.Invoke();
    }

    public void TurnOff()
    {
        IsOn = false;

        light.gameObject.SetActive(false);

        OnTurnOff?.Invoke();
    }

    public void Break()
    {
        IsBroken = true;
        
        animator.SetTrigger("Break");

        Invoke(nameof(PlayBreakEffects), 2f);

        OnBreak?.Invoke();
    }

    private void PlayBreakEffects()
    {
        sparkParticles.Play();
        lightBreaksSound.Play();

        Invoke(nameof(Test), 0.8f);
    }
    private void Test() => light.gameObject.SetActive(false);
    
    public void Fix()
    {
        IsBroken = false;

        animator.SetTrigger("Fix");

        light.gameObject.SetActive(IsOn);

        OnFix?.Invoke();
    }

    protected override void InteractionStart(Player interactor)
    {
        if(IsBroken && IsOff)
            Fix();
    }

    public override InteractionFeedback Feedback(Player interactor)
    {
        if(IsBroken)
            if(IsOff)
                return new InteractionFeedback {
                    Status = InteractionStatus.Valid,
                    Message = "Fix the light"
                };
            else
                return new InteractionFeedback {
                    Status = InteractionStatus.Invalid,
                    Message = "Turn off the lights before trying to fix it."
                };

        return InteractionFeedback.None;
    }

    public void Save(GameData gameData)
    {
        LightbulbData data = new LightbulbData
        {
            Id = Id,

            IsBroken = IsBroken,
            IsLightActive = light.gameObject.activeInHierarchy
        };

        gameData.LightbulbsData.Add(data);
    }   

    public void Load(GameData gameData)
    {
        LightbulbData data = gameData.LightbulbsData.Find(_data => _data.Id == Id);

        IsBroken = data.IsBroken;
        light.gameObject.SetActive(data.IsLightActive);
    }
}