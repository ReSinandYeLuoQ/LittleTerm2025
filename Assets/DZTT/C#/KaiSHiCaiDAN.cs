using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 引入 SceneManagement 命名空间用于场景管理

public class KaiSHiCaiDAN : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 切换到下一个场景
    /// </summary>
    public void SwitchToNextScene()
    {
        // 获取当前活动场景的索引
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // 计算下一个场景的索引
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        // 加载下一个场景
        SceneManager.LoadScene(nextSceneIndex);
    }
}
