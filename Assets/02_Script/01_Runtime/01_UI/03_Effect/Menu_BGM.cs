using UnityEngine;

public class Menu_BGM : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _BGM;

    void Start()
    {
        if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _BGM;
        _audioSource.loop = true;
        _audioSource.Play();
    }
}
