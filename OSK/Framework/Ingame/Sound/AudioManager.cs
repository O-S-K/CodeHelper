using System.Collections;
using UnityEngine;
using System.Linq;
using OSK;

public class AudioManager : SingletonMono<AudioManager>
{
    public AudioSource curMusic;
    public AudioClip mainMusic;
 
    public SoundEffect effectPrefab;
    public AudioClip[] effects;
    
    public AudioClip[] effectsBattle;

    private Animator anim;
    private AudioSource prevMusic;
  
    
    public void PlayOneShot(string clipName,float volume = 1f, float pitch = 1)
    {
        AudioClip clip = effects.FirstOrDefault(e => e.name == clipName);
        if (clip != null)
        {
            curMusic.PlayOneShot(clip, volume);
            curMusic.pitch = pitch;
        }
    } 
    
    public IEnumerator IEPlayOneShot(string clipName, float delay, float volume = 1f)
    {
        yield return new WaitForSeconds(delay);
        AudioClip clip = effects.FirstOrDefault(e => e.name == clipName);
        if (clip != null)
            curMusic.PlayOneShot(clip, volume);
    }
    
    public void PlayBattle(string clipName)
    {
        AudioClip clip = effectsBattle.FirstOrDefault(e => e.name == clipName);
        if (clip != null)
            curMusic.PlayOneShot(clip, 1);
    } 
    
    public void PlayMusic(AudioClip clipName,float volume = 1f)
    {
        curMusic.Stop();
        if (clipName != null)
        {
            curMusic.clip = clipName;
            curMusic.loop = true;
            curMusic.Play();
        }
    }
    
    public void StopMusic()
    {
        curMusic.Stop();
    }
    
    public void PlayEffect(AudioClip clip, float vol = 1f)
    {
        if (clip == null)
            return;
        SoundEffect effect = Instantiate(effectPrefab);
        effect.transform.SetParent(Camera.main.transform);
        effect.transform.localPosition = Vector3.zero;
        effect.Play(clip, vol);
    }
    
    
    public void PlayEffectAt(AudioClip clip, Vector3 pos, float volume, bool pitchShift = true, int priority = 128)
    {
        SoundEffect se = Instantiate(effectPrefab, pos, Quaternion.identity);
        se.Play(clip, volume, pitchShift);
        se.transform.parent = transform;
        se.audioSource.priority = priority;
    }
}