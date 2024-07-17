using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip strikeSound; // 再生する音声クリップ
    public AudioClip[] carvingSounds; // 削る音の配列
    private AudioSource audioSource; // AudioSourceの参照

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // AudioSourceコンポーネントを追加
    }

    public void PlayStrikeSound()
    {
        if (strikeSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(strikeSound);
        }
    }

    public void PlayCarvingSound(int carvingCount)
    {
        if (carvingSounds != null && carvingSounds.Length > 0 && audioSource != null)
        {
            int index = Mathf.Min(carvingCount, carvingSounds.Length - 1);
            audioSource.PlayOneShot(carvingSounds[index]);
        }
    }
}
