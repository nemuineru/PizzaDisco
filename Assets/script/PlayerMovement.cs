using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public float ControllerRotationSpeed = 0f;
    Vector3 MoveAxis = new Vector3(),  PrevAxis;
    public bool isRotating = false;

    public AudioClip Whooshsound;
    float whooshSoundmintime = 5f, currentWhooshRotate = 0f;

    AudioSource AudSource;
    Rigidbody playermove;
    Animator playeranimator;

    float rotateMinThreadshold = 35f, rotateStartThreadshold = 50f;

    // Start is called before the first frame update
    void Start()
    {
        playermove = GetComponent<Rigidbody>();
        StartCoroutine(rotationRots());
        AudSource = GetComponent<AudioSource>();
        playeranimator = GetComponent<Animator>();
        AudSource.clip = Whooshsound;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playermove.AddForce(MoveAxis * speed,ForceMode.VelocityChange);
        Animset();
        //isRotatingは回転中か否かで次のステートが決定.
        if (isRotating && Mathf.Abs(ControllerRotationSpeed) < rotateMinThreadshold) {
            isRotating = false;
        }
        else if (!isRotating && Mathf.Abs(ControllerRotationSpeed) > rotateStartThreadshold)
        {
            isRotating = true;
        }
        
        if (isRotating)
        {
            Vector3 rotatevec = Quaternion.AngleAxis
                (Mathf.Sign(ControllerRotationSpeed) * (Mathf.Abs(ControllerRotationSpeed) - rotateMinThreadshold)
                * 2f, Vector3.up) * transform.forward;
            Quaternion FacingRotation = Quaternion.LookRotation(rotatevec, Vector3.up);

            playermove.MoveRotation(FacingRotation);
            if (360f < currentWhooshRotate)
            {
                AudSource.outputAudioMixerGroup.audioMixer.SetFloat
                    ("PitchShifter", Mathf.Pow(Mathf.Clamp(Mathf.Abs(ControllerRotationSpeed), 0f, Mathf.Infinity) / rotateStartThreadshold, 1/2f) / 8f);
                AudSource.pitch = 1f + 5f
                    * Mathf.Pow(Mathf.Clamp(Mathf.Abs(ControllerRotationSpeed), 0f, Mathf.Infinity) / rotateStartThreadshold,2f);
                if(!AudSource.isPlaying)
                AudSource.Play();
                currentWhooshRotate = 0f;
            }
            currentWhooshRotate += Mathf.Abs(ControllerRotationSpeed);
        }
        else
        {
            if (playermove.velocity.magnitude != 0)
            {
                Quaternion FacingRotation = Quaternion.LookRotation(playermove.velocity.normalized, Vector3.up);
                playermove.MoveRotation(FacingRotation);
            }
        }
    }

    IEnumerator rotationRots()
    {
        float ContCurrentAngle = 0f;
        while (true)
        {
            ContCurrentAngle = 0f;
            MoveAxis = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            if (PrevAxis == null)
            {
                PrevAxis = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            }
            if (MoveAxis.magnitude > 0.8f)
            {
                ContCurrentAngle = Vector3.SignedAngle(PrevAxis.normalized, MoveAxis.normalized, Vector3.up);
                if (Mathf.Abs(ContCurrentAngle) > 1f) {
                    ContCurrentAngle = 1f * Mathf.Sign(ContCurrentAngle);
                }
                PrevAxis = MoveAxis;
            }
            ControllerRotationSpeed = (ControllerRotationSpeed + ContCurrentAngle) / (1f + Time.deltaTime);

            yield return null;
        }
    }

    void Animset() {
        playeranimator.SetBool("isRotating", isRotating);
        playeranimator.SetFloat("speed",playermove.velocity.magnitude);
    }
}
