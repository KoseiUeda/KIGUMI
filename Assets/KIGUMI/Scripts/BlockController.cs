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
        // �� ���N���b�N
        if (Input.GetMouseButtonDown(0))
        {
            // �����ʒu�̕ϐ��̍��W�Ƀu���b�N�𐶐�
            Instantiate(Block1, BlockPosition.position, Quaternion.identity);

        }

        // �� �E�N���b�N
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