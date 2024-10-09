using UnityEngine;

[System.Serializable]
public class MoveCountSound
{
    public AudioClip[] sounds; // 移動回数に応じた音源
}

public class HammerSoundPlayer : MonoBehaviour
{
    public MoveCountSound totalSounds; // 全体の移動回数に応じた音源
    public int[] faceIndices; // 叩いた時に音を鳴らす面のインデックス
    private AudioSource audioSource;
    private FaceHighlight faceHighlight;

    void Start()
    {
        faceHighlight = GetComponent<FaceHighlight>(); // FaceHighlightスクリプトの取得
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    int triangleIndex = hit.triangleIndex;
                    if (System.Array.Exists(faceIndices, index => index == triangleIndex))
                    {
                        PlayHammerSound();
                    }
                }
            }
        }
    }

    void PlayHammerSound()
    {
        // 全体の移動回数に応じた音を再生
        int totalMoveCount = faceHighlight.GetTotalMoveCount();
        int totalSoundIndex = Mathf.Clamp(totalMoveCount, 0, totalSounds.sounds.Length - 1);
        audioSource.clip = totalSounds.sounds[totalSoundIndex];
        audioSource.Play();
    }
}
