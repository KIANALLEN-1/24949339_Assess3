using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRotation : MonoBehaviour
{
    public Animator animatorController;
    public AudioClip buttonSound;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = buttonSound;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (audioSource != null && buttonSound != null)
            {
                audioSource.Play();
            }
            if(animatorController != null)
            {
                animatorController.SetTrigger("RotateParam");

            }

        }
    }
}
