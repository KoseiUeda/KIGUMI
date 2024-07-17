using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] carvingSounds; // 異なる音声クリップの配列
    private AudioSource audioSource; // AudioSourceの参照

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // AudioSourceコンポーネントを追加
        audioSource.volume = 0.4f; // 音量を0.7に設定
    }

    public void PlayCarvingSound(int carvingCount)
    {
        int index = Mathf.Clamp(carvingCount, 0, carvingSounds.Length - 1);
        if (carvingSounds[index] != null && audioSource != null)
        {
            audioSource.PlayOneShot(carvingSounds[index]);
        }
    }
}
