using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[RequireComponent(typeof(AudioSource))]
public class SoundsMgr : MonoBehaviour
{
    public static SoundsMgr _Instance;

    /// <summary>
    /// 音乐播放器
    /// </summary>
    public AudioSource currAudio;

    /// <summary>
    /// 背景音乐链表
    /// </summary>
    public List<AudioClip> backgroundAudioList;

    /// <summary>
    /// 音效链表
    /// </summary>
    public List<AudioClip> soundEffectsList;

    /// <summary>
    /// 音源池(因为音效是可以和背景音共存的)
    /// </summary>
    public List<AudioSource> soundEffectsPool;

    /// <summary>
    /// 音乐控制
    /// </summary>
    public bool isOpenBackgroundMusic;
    public float lastBackgroundRate;
    public float backgroundVolume;
    public float soundEffectVolume;
    public bool isOpenSoundEffects;

    private void Awake()
    {
        _Instance = this;
        currAudio = GetComponent<AudioSource>();
        DontDestroyOnLoad(this);

        GameObject obj = new GameObject("SoundEffect_" + (soundEffectsPool.Count + 1), typeof(AudioSource));
        obj.transform.parent = this.transform;
        AudioSource audio = obj.GetComponent<AudioSource>();
        audio.loop = false;
        audio.rolloffMode = AudioRolloffMode.Linear;
        audio.minDistance = 30;
        audio.maxDistance = 200;
        soundEffectsPool.Add(audio);
        DontDestroyOnLoad(obj);
    }

    private void Update()
    {
        //音效：点击鼠标触发的音效
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlaySoundEffect("Sound_MouseClick");
        }

        //
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlaySoundEffect("soundEffect2");
        }


        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlaySoundEffect("soundEffect3");
        }


        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlaySoundEffect("soundEffect4");
        }


        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PlaySoundEffect("soundEffect5");
        }


        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            PlaySoundEffect("soundEffect6");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            PlaySoundEffect("soundEffect7");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("背景音乐");
            PlayBackgroundMusic("BG");
            MeetEventGameCtrl._Instance.Init();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayBackgroundMusic("Win");
        }
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name">音效名</param>
    /// <param name="pos">音效播放位置</param>
    /// <param name="is3D">是否是3D音效</param>
    public void PlaySoundEffect(string name,Vector3 pos=default(Vector3), bool is3D = false)
    {
        if (!isOpenSoundEffects)
        { return; }
        AudioSource audioSource = null;
        bool isExist = false;
        //1.找到空闲的音效播放器
        foreach (AudioSource audio in soundEffectsPool)
        {
            if (audio.clip==null||!audio.isPlaying)
            {
                //空闲的：
                audioSource = audio;
                break;
            }
        }
        //没有空闲的，扩充队列
        if (audioSource == null)
        {
            GameObject obj = new GameObject("SoundEffect_" + (soundEffectsPool.Count+1), typeof(AudioSource));
            obj.transform.parent = this.transform;
            AudioSource audio = obj.GetComponent<AudioSource>();
            audio.loop = false;
            audio.rolloffMode = AudioRolloffMode.Linear;
            audio.minDistance = 30;
            audio.maxDistance = 200;
            soundEffectsPool.Add(audio);
            DontDestroyOnLoad(obj);
        }
        //2.找到对应音效并播放
        foreach (AudioClip clip in soundEffectsList)
        {
            if (clip.name == name)
            {
                audioSource.clip = clip;
                audioSource.gameObject.transform.localPosition = pos;
                audioSource.spatialBlend = is3D?1:0;
                audioSource.volume = soundEffectVolume;
                audioSource.Play();
                isExist = true;
                break;
            }
        }
        if(!isExist)
        {
            Debug.LogWarning("音效名：" + name + "不存在");
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name"></param>
    public void PlayBackgroundMusic(string name)
    {
        //关闭了或者正在播放的就是目标：直接返回
        if (!isOpenBackgroundMusic)
        { return; }
        if (currAudio.clip != null && currAudio.clip.name == name)
        { return; }
        StartCoroutine(PlayBackgroundMusicIE(name));

    }

    private IEnumerator PlayBackgroundMusicIE(string name)
    {
        float fadeTime = 0.5f;
        bool isExist = false;   
        //音乐渐出
        if (currAudio.clip != null)
        {
            yield return StartCoroutine(MusicFadeOut(fadeTime));
        }

   
        //寻找音乐
        foreach (AudioClip clip in backgroundAudioList)
        {
            if (clip.name == name)
            {
                currAudio.clip = clip;
                currAudio.Play();
                isExist = true;
                break;
            }
        }
        if(!isExist)
        {
            Debug.LogWarning("音乐不存在：" + name);
        }

        yield return StartCoroutine(MusicFadeIn(fadeTime));
    }

    /// <summary>
    /// 音乐渐出协程
    /// </summary>
    /// <param name="fadeOutTime"></param>
    /// <returns></returns>
    private IEnumerator MusicFadeOut(float fadeOutTime)
    {
        float time = 0;
        while (fadeOutTime > time)
        {
            if (time != 0)
            {
                currAudio.volume = Mathf.Lerp(backgroundVolume, 0f, time / fadeOutTime);
            }
            time += Time.deltaTime;
            yield return 1;
        }
        currAudio.volume = 0;
    }

    /// <summary>
    /// 声音渐入协程
    /// </summary>
    /// <param name="fadeInTime"></param>
    /// <returns></returns>
    private IEnumerator MusicFadeIn(float fadeInTime)
    {
        float time = 0f;
        while (time <= fadeInTime)
        {
            if (time != 0)
            {
                currAudio.volume = Mathf.Lerp(0f, backgroundVolume, time / fadeInTime);
            }
            time += Time.deltaTime;
            yield return 1;
        }
        currAudio.volume = backgroundVolume;
    }
}
