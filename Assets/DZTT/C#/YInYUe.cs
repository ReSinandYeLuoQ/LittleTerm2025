using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SceneMusicPair
{
    public string sceneName;
    public AudioClip music;
}

public class YInYUe : MonoBehaviour
{
    // 定义 AudioSource 组件，用于播放音乐
    private AudioSource audioSource;
    // 当前场景播放的音乐
    public AudioClip currentSceneMusic;
    // 存储不同场景对应的音乐
    private Dictionary<string, AudioClip> sceneMusicMap = new Dictionary<string, AudioClip>();
    [SerializeField] private List<SceneMusicPair> sceneMusicPairs;

    void Awake()
    {
        // 添加 AudioSource 组件
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true; // 设置音乐循环播放

        // 注册场景加载完成的事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Start is called before the first frame update
    void Start()
    {
        // 初始化不同场景对应的音乐
        InitializeSceneMusicMap();
        // 获取当前场景名称
        string currentSceneName = SceneManager.GetActiveScene().name;
        // 根据当前场景名称获取对应的音乐
        if (sceneMusicMap.TryGetValue(currentSceneName, out currentSceneMusic))
        {
            // 设置当前音乐
            audioSource.clip = currentSceneMusic;
            // 播放音乐
            audioSource.Play();
        }
    }

    // 初始化不同场景对应的音乐
    private void InitializeSceneMusicMap()
    {
        foreach (var pair in sceneMusicPairs)
        {
            sceneMusicMap[pair.sceneName] = pair.music;
        }
    }

    // 场景加载完成时调用
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 根据新场景名称获取对应的音乐
        if (sceneMusicMap.TryGetValue(scene.name, out currentSceneMusic))
        {
            // 设置新的音乐
            audioSource.clip = currentSceneMusic;
            // 播放新音乐
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        // 取消注册场景加载完成的事件
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
