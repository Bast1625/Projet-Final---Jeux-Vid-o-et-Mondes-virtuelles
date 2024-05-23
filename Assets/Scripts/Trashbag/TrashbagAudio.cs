using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashbagAudio : MonoBehaviour
{
    [SerializeField] Trashbag trashbag;

    [SerializeField] AudioClip grabSound;
    [SerializeField] AudioClip disposeSound;
    
    private AudioSource audioSource;
}
