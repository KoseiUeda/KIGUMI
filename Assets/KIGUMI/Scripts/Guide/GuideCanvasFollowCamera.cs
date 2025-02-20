using UnityEngine;

public class GuideCanvasFacePlayer : MonoBehaviour
{
    public Transform player; // プレイヤーのTransform

    void Update()
    {
        if (player != null)
        {
            // プレイヤーの方向を取得（Y軸だけ回転）
            Vector3 direction = player.position - transform.position;
            direction.y = 0; // Y軸の回転を固定

            // 向きを調整
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(-direction);
            }
        }
    }
}
