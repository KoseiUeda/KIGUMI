using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip strikeSound; // 再生する音声クリップ
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
}
