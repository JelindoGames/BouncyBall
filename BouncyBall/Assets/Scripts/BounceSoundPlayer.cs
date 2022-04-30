using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceSoundPlayer : MonoBehaviour
{
    public enum BounceType
    {
        Landing,
        Bounce,
        PerfectBounce,
        Jump
    }

    [SerializeField] AudioClip perfectBounce;
    [SerializeField] AudioClip stdBounce;
    [SerializeField] AudioClip jumpBounce;
    [SerializeField] AudioClip landing;
    [SerializeField] GameObject audioPlayer;
    [SerializeField] float minTimeBtwnLands; // Minimum time between plays (stops multiple frames in a row w/same sfx)
    bool landPlayable = true;

    // Plays the appropriate sound depending on the way the player just bounced.
    // (The player can do a regular bounce, a perfect bounce, or a landing (this
    // is considered a bounce in this context))
    public void Play(BounceType bt)
    {
        switch (bt)
        {
            case BounceType.Landing:
                if (landPlayable)
                {
                    landPlayable = false;
                    SpawnSound(landing);
                    Invoke("MakeLandPlayable", minTimeBtwnLands);
                }
                break;
            case BounceType.Bounce:
                SpawnSound(stdBounce);
                break;
            case BounceType.Jump:
                SpawnSound(jumpBounce);
                break;
            case BounceType.PerfectBounce:
                SpawnSound(perfectBounce);
                break;
            default:
                break;
        }
    }

    void MakeLandPlayable()
    {
        landPlayable = true;
    }

    // Creates a 2D sound in a way that AudioSource.PlayClipAtPoint does not.
    void SpawnSound(AudioClip sound)
    {
        AudioSource audio = Instantiate(audioPlayer, Camera.main.transform.position, Quaternion.identity).GetComponent<AudioSource>();
        audio.clip = sound;
        audio.Play();
    }
}
