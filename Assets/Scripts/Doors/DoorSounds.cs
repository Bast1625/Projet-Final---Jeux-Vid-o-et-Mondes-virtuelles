using UnityEngine;

public class DoorSounds : MonoBehaviour
{
    [SerializeField] Door door;

    [SerializeField] AudioClip openSound;
    [SerializeField] AudioClip closeSound;
    
    [SerializeField] AudioClip lockSound;
    [SerializeField] AudioClip unlockSound;
    
    [SerializeField] AudioClip tryOpenSound;

    [SerializeField] AudioClip tryLockSound;
    [SerializeField] AudioClip tryUnlockSound;

    
    private AudioSource audioSource;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        //door.OnOpen += () => audioSource.PlayOneShot(openSound);
        //door.OnClose += () => audioSource.PlayOneShot(closeSound);
        
        //door.OnLock += () => audioSource.PlayOneShot(lockSound);
        //door.OnUnlock += () => audioSource.PlayOneShot(unlockSound);

        //door.OnTryOpen += () => audioSource.PlayOneShot(tryOpenSound);
        //door.OnTryLock += () => audioSource.PlayOneShot(tryLockSound);
        //door.OnTryUnlock += () => audioSource.PlayOneShot(tryUnlockSound);
    }
}
