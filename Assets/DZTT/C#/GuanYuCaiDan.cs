using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 引入 TextMeshPro 命名空间，用于使用 TextMeshProUGUI 组件
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 该脚本用于处理按钮点击事件，实现点击按钮后渐显一个 Canvas，
/// 并依次对 Canvas 内的多个 TextMeshProUGUI 文字对象进行渐显渐隐效果。
/// </summary>
public class GuanYuCaiDan : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// 要渐显的 Canvas 对象，通过 Inspector 面板序列化赋值。
    /// </summary>
    [SerializeField] private Canvas targetCanvas;

    /// <summary>
    /// Canvas 里的文字对象列表，使用 TextMeshProUGUI 组件，通过 Inspector 面板序列化赋值。
    /// </summary>
    [SerializeField] private List<TextMeshProUGUI> textObjects;

    /// <summary>
    /// 渐显渐隐效果的持续时间，单位为秒，默认值为 1 秒，可在 Inspector 面板调整。
    /// </summary>
    [SerializeField] private float fadeDuration = 1f;

    /// <summary>
    /// 目标 Canvas 的 CanvasGroup 组件，用于控制 Canvas 的透明度。
    /// </summary>
    private CanvasGroup canvasGroup;

    /// <summary>
    /// 在脚本实例被启用时调用，初始化 Canvas 和文字对象的透明度。
    /// </summary>
    void Start()
    {
        // 检查目标 Canvas 是否存在
        if (targetCanvas != null)
        {
            // 获取目标 Canvas 的 CanvasGroup 组件
            canvasGroup = targetCanvas.GetComponent<CanvasGroup>();
            // 如果 Canvas 没有 CanvasGroup 组件，则添加一个
            if (canvasGroup == null)
            {
                canvasGroup = targetCanvas.gameObject.AddComponent<CanvasGroup>();
            }
            // 将 Canvas 的透明度设置为 0，使其初始状态不可见
            canvasGroup.alpha = 0f;
            // 禁用目标 Canvas 对象
            targetCanvas.gameObject.SetActive(false);
        }

        // 遍历文字对象列表
        foreach (var text in textObjects)
        {
            // 获取每个文字对象的 CanvasGroup 组件
            CanvasGroup textCanvasGroup = text.GetComponent<CanvasGroup>();
            // 如果文字对象没有 CanvasGroup 组件，则添加一个
            if (textCanvasGroup == null)
            {
                textCanvasGroup = text.gameObject.AddComponent<CanvasGroup>();
            }
            // 将文字对象的透明度设置为 0，使其初始状态不可见
            textCanvasGroup.alpha = 0f;
        }
    }

    /// <summary>
    /// 实现 IPointerClickHandler 接口的方法，在按钮被点击时调用。
    /// </summary>
    /// <param name="eventData">点击事件的数据。</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        // 检查目标 Canvas 是否存在
        if (targetCanvas != null)
        {
            // 启用目标 Canvas 对象
            targetCanvas.gameObject.SetActive(true);
            // 启动协程，开始执行 Canvas 渐显和文字渐显渐隐的效果
            StartCoroutine(FadeCanvasIn());
        }
    }

    /// <summary>
    /// 协程方法，用于实现目标 Canvas 的渐显效果，并依次调用文字渐显渐隐的协程。
    /// </summary>
    /// <returns>IEnumerator 迭代器，用于协程控制。</returns>
    private IEnumerator FadeCanvasIn()
    {
        // 记录已经过去的时间
        float elapsedTime = 0f;
        // 在过去的时间小于渐显持续时间时循环
        while (elapsedTime < fadeDuration)
        {
            // 使用线性插值计算当前 Canvas 的透明度
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            // 累加过去的时间
            elapsedTime += Time.deltaTime;
            // 等待下一帧
            yield return null;
        }
        // 确保 Canvas 的透明度最终为 1
        canvasGroup.alpha = 1f;

        // 遍历文字对象列表
        foreach (var text in textObjects)
        {
            // 启动协程，对每个文字对象执行渐显渐隐效果，并等待该协程完成
            yield return StartCoroutine(FadeTextInAndOut(text));
        }

        // 所有文字播放完毕，渐隐 Canvas
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;

        // 禁用目标 Canvas 对象
        targetCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// 协程方法，用于实现单个 TextMeshProUGUI 文字对象的渐显渐隐效果。
    /// </summary>
    /// <param name="text">要进行渐显渐隐效果的 TextMeshProUGUI 对象。</param>
    /// <returns>IEnumerator 迭代器，用于协程控制。</returns>
    private IEnumerator FadeTextInAndOut(TextMeshProUGUI text)
    {
        // 获取文字对象的 CanvasGroup 组件
        CanvasGroup textCanvasGroup = text.GetComponent<CanvasGroup>();

        // 文字渐显部分
        // 记录已经过去的时间
        float elapsedTime = 0f;
        // 在过去的时间小于渐显持续时间时循环
        while (elapsedTime < fadeDuration)
        {
            // 使用线性插值计算当前文字的透明度
            textCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            // 累加过去的时间
            elapsedTime += Time.deltaTime;
            // 等待下一帧
            yield return null;
        }
        // 确保文字的透明度最终为 1
        textCanvasGroup.alpha = 1f;

        // 等待一段时间，时间长度为渐显渐隐的持续时间
        yield return new WaitForSeconds(fadeDuration);

        // 文字渐隐部分
        // 重置已经过去的时间
        elapsedTime = 0f;
        // 在过去的时间小于渐隐持续时间时循环
        while (elapsedTime < fadeDuration)
        {
            // 使用线性插值计算当前文字的透明度
            textCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            // 累加过去的时间
            elapsedTime += Time.deltaTime;
            // 等待下一帧
            yield return null;
        }
        // 确保文字的透明度最终为 0
        textCanvasGroup.alpha = 0f;
    }
}
