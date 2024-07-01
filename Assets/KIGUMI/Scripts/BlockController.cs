using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public Transform BlockPosition;
    public GameObject Block1;
    public GameObject DestroyBlock;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // ↓ 左クリック
        if (Input.GetMouseButtonDown(0))
        {
            // 生成位置の変数の座標にブロックを生成
            Instantiate(Block1, BlockPosition.position, Quaternion.identity);

        }

        // ↓ 右クリック
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(DestroyBlock);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag== "Block")
        {
            DestroyBlock = other.gameObject;
        }
    }
}