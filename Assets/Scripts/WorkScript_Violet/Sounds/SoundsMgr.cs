using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
[RequireComponent(typeof(AudioSource))]
public class SoundsMgr : MonoBehaviour
{
    public static SoundsMgr _Instance;

    /// <summary>
    /// 背景音乐
    /// </summary>
    public List<AudioSource> backgroundAudio;

    private void Awake()
    {
        _Instance = this;
    }

}
