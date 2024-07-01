using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueUwakiClick : MonoBehaviour
{
    private Animator animator;
    private int clickCount = 0;

    [SerializeField] private AudioSource audioSource1;
    [SerializeField] private AudioSource audioSource2;
    [SerializeField] private AudioSource audioSource3;
    [SerializeField] private AudioSource audioSource4;
    [SerializeField] private AudioSource audioSource5;
    [SerializeField] private AudioSource audioSource6;
    [SerializeField] private AudioSource audioSource7;
    [SerializeField] private AudioSource audioSource8;
    [SerializeField] private AudioSource audioSource9;
    [SerializeField] private AudioSource audioSource10;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Hammer")
        {
            clickCount++;
            // Down1��Down2�����Z�b�g
            animator.SetBool("Down1", false);
            animator.SetBool("Down2", false);
            animator.SetBool("Down3", false);
            animator.SetBool("Down4", false);
            animator.SetBool("Down5", false);
            animator.SetBool("Down6", false);
            animator.SetBool("Down7", false);
            animator.SetBool("Down8", false);
            animator.SetBool("Down9", false);
            animator.SetBool("Down10", false);

            // �N���b�N�񐔂ɉ����āA�قȂ�A�j���[�V�������Đ�����
            switch (clickCount)
            {
                case 1:
                    animator.SetBool("Down1", true);
                    audioSource1.Play();
                    break;
                case 2:
                    animator.SetBool("Down2", true);
                    audioSource2.Play();
                    break;
                case 3:
                    animator.SetBool("Down3", true);
                    audioSource3.Play();
                    break;
                case 4:
                    animator.SetBool("Down4", true);
                    audioSource4.Play();
                    break;
                case 5:
                    animator.SetBool("Down5", true);
                    audioSource5.Play();
                    break;
                case 6:
                    animator.SetBool("Down6", true);
                    audioSource6.Play();
                    break;
                case 7:
                    animator.SetBool("Down7", true);
                    audioSource7.Play();
                    break;
                case 8:
                    animator.SetBool("Down8", true);
                    audioSource8.Play();
                    break;
                case 9:
                    animator.SetBool("Down9", true);
                    audioSource9.Play();
                    break;
                case 10:
                    animator.SetBool("Down10", true);
                    audioSource10.Play();
                    break;
                default:
                    // 6��ڈȍ~�̃N���b�N�͖�������
                    break;
            }
        }
    }
}