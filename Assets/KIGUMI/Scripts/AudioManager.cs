using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip strikeSound; // 再生する音声クリップ
    private AudioSource audioSource; // AudioSourceの参照

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // AudioSourceコンポーネントを追加
        audioSource.volume = 0.4f; // 音量を0.7に設定
    }

    public void PlayStrikeSound()
    {
        if (strikeSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(strikeSound, 0.4f); // 音量0.7で音声クリップを再生
        }
    }
}
