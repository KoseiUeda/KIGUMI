using UnityEngine;

public class ChiselContinuousCollider : MonoBehaviour
{
    [SerializeField] Transform _colliderObject; // Chiselのコライダーをアタッチする
    Vector3 _prePosition;
    float _minZ = 0;

    void Start()
    {
        // 初期値を保存
        _minZ = _colliderObject.localScale.z / 2;
        _prePosition = transform.position;
    }

    // 物理演算に関係するのでFixedUpdateで
    void FixedUpdate()
    {
        // 前フレームからの変位を計算
        Vector3 def = transform.position - _prePosition;

        // 前フレームからの変位をラケットの正面方向(transform.forward)に射影することで、ラケットがどれだけ前後に動いたか(defZ)を計算
        float defZ = Vector3.Dot(def, transform.forward);

        // ラケットが前に閾値(_minZ)以上動いた場合、あるいは後ろに閾値(_minZ)以上動いた場合、コライダーの位置とスケールをそれぞれ調整
        if (defZ > _minZ)
        {
            _colliderObject.position = transform.position - (defZ - _minZ) / 2 * transform.forward;
            _colliderObject.localScale = new Vector3(_colliderObject.localScale.x, _colliderObject.localScale.y, defZ + _minZ);
        }
        else if (defZ < -_minZ)
        {
            _colliderObject.position = transform.position + (-defZ - _minZ) / 2 * transform.forward;
            _colliderObject.localScale = new Vector3(_colliderObject.localScale.x, _colliderObject.localScale.y, -defZ + _minZ);
        }
        else
        {
            _colliderObject.position = transform.position;
            _colliderObject.localScale = new Vector3(_colliderObject.localScale.x, _colliderObject.localScale.y, _minZ * 2);
        }

        // 現在のラケットの位置を保持しておき次フレームでの比較に使用
        _prePosition = transform.position;
    }
}
