using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jiaose : MonoBehaviour
{
    public float moveSpeed = 5f; // 移动速度

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 获取水平和垂直输入，W、S 键或上、下方向键控制垂直移动，A、D 键或左、右方向键控制水平移动
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 计算移动方向向量
        Vector2 movement = new Vector2(horizontalInput, verticalInput);

        // 应用移动
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
}
