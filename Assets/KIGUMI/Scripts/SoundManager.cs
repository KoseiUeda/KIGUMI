using System.Collections.Generic;
using UnityEngine;
[System.Serializable] // インスペクタで表示させるためにSerializable属性を指定
public class AudioData
{
    public float minStep; // この値以上
    public float maxStep; // この値以下でこの音源を再生
    public AudioSource source; // この範囲に対応するAudioSource
}


public class SoundManager : MonoBehaviour
{
    public List<AudioData> audioDataList; // AudioDataのリスト

    // currentMoveStepに基づいて適切なAudioSourceを選択して再生
    public void PlaySound(float stepAmount)
    {
        foreach (var data in audioDataList)
        {
            if (stepAmount >= data.minStep && stepAmount <= data.maxStep)
            {
                data.source.Play();
                break;
            }
        }
    }
}
