using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    // singleton instance
    public static AudioManager instance = null;

    // public fields
    public AudioClip roomToggleClip;
    public AudioClip roomUntoggleClip;
    public AudioClip uiClickClip;

    // private fields
    private AudioSource _audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _audioSource = this.GetComponent<AudioSource>();
    }

    public void PlayRoomToggleClip()
    {
        _audioSource.clip = roomToggleClip;

        _audioSource.Play();
    }

    public void PlayRoomUntoggleClip()
    {
        _audioSource.clip = roomUntoggleClip;

        _audioSource.Play();
    }

    public void PlayUiClickClip()
    {
        _audioSource.clip = uiClickClip;

        _audioSource.Play();
    }

    public void PlayClip(AudioClip clip)
    {
        _audioSource.clip = clip;

        _audioSource.Play();
    }
}
