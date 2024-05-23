using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CartAudio : MonoBehaviour
{
    [SerializeField] Cart cart;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        cart.OnDragEnter += () => audioSource.Play();
        cart.OnDragExit += () => audioSource.Stop();
    }   
}
