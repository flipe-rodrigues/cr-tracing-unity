using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    // singleton instance
    public static AudioManager instance = null;

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

    public void PlayClip(AudioClip clip)
    {
        _audioSource.clip = clip;

        if (Time.time > .1f)
        {
            _audioSource.Play();
        }
    }
}
