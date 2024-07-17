using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] carvingSounds; // 異なる音声クリップの配列
    private AudioSource audioSource; // AudioSourceの参照
    public float carvingSoundVolume = 0.4f; // 削る音の音量 (0.0 - 1.0)

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // AudioSourceコンポーネントを追加
        audioSource.volume = carvingSoundVolume; // 音量を設定
    }

    public void PlayCarvingSound(int carvingCount)
    {
        int index = Mathf.Clamp(carvingCount, 0, carvingSounds.Length - 1);
        if (carvingSounds[index] != null && audioSource != null)
        {
            audioSource.PlayOneShot(carvingSounds[index], carvingSoundVolume); // 音量を指定して再生
        }
    }
}
