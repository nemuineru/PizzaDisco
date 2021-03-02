using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 250f;
    public float InitSpeed = 250f;
    public float SuperSpeed = 350f;
    public int InitPower = 1;
    public int SuperPower = 5;
    public int Power = 1;
    public float PowerUpTime;
    public float SpeedUpTime;
    public float ControllerRotationSpeed = 0f;
    Vector3 MoveAxis = new Vector3(), PrevAxis;
    public bool isRotating = false;
    public ParticleSystem SuperEffect_Speed, SuperEffect_Power;

    public Systems sys;

    public AudioClip Whooshsound;
    float whooshSoundmintime = 5f, currentWhooshRotate = 0f;

    public float invisibletime = 1f, currentinvistime = 1000f;
    public float recovreqtime = 1f, recoverytime = 100f;

    AudioSource AudSource;
    Rigidbody playermove;
    Animator playeranimator;

    float rotateMinThreadshold = 5f, rotateStartThreadshold = 40f;

    private void OnTriggerEnter(Collider other)
    {
        if (sys.RestTime > 0f)
        {
            if (other.gameObject.tag == "enemyexpld" && currentinvistime > invisibletime)
            {
                playeranimator.SetTrigger("Damagedtrig");
                currentinvistime = 0f;
                if (sys.RestTime > 60f)
                {
                    sys.plustime = -Mathf.CeilToInt(10 + (sys.RestTime - 60f) / 4);
                }
                else
                {
                    sys.plustime = -Mathf.CeilToInt(10);
                }
                Quaternion FacingRotation = Quaternion.LookRotation(playermove.velocity.normalized, Vector3.up);
                playermove.MoveRotation(FacingRotation);
                ControllerRotationSpeed = 0f;
                playermove.velocity = Vector3.zero;
                recoverytime = 0f;
                isRotating = false;
            }
        }
    }

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
        if (sys.RestTime > 0f)
        {
            recoverytime += Time.fixedDeltaTime;
            currentinvistime += Time.fixedDeltaTime;
            //パワーアップとスピードアップ...
            if (PowerUpTime > 0f)
            {
                Power = SuperPower;
                SuperEffect_Power.Play(true);
            }
            else
            {
                Power = InitPower;
                SuperEffect_Power.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
            PowerUpTime -= Time.fixedDeltaTime;
            PowerUpTime = Mathf.Clamp(PowerUpTime, 0f, Mathf.Infinity);
            if (SpeedUpTime > 0f)
            {
                speed = SuperSpeed;
                SuperEffect_Speed.Play(true);
            }
            else
            {
                speed = InitSpeed;
                SuperEffect_Speed.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
            SpeedUpTime -= Time.fixedDeltaTime;
            SpeedUpTime = Mathf.Clamp(SpeedUpTime, 0f, Mathf.Infinity);

            Animset();
            if (recoverytime > recovreqtime)
            {
                playermove.AddForce(MoveAxis * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
                //isRotatingは回転中か否かで次のステートが決定.
                if (isRotating && Mathf.Abs(ControllerRotationSpeed) < rotateMinThreadshold)
                {
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
                            ("PitchShifter", Mathf.Pow(Mathf.Clamp(Mathf.Abs(ControllerRotationSpeed), 0f, Mathf.Infinity) / rotateStartThreadshold, 1 / 2f) / 1024f);
                        AudSource.pitch = 1f + 4f
                            * Mathf.Pow(Mathf.Clamp(Mathf.Abs(ControllerRotationSpeed), 0f, Mathf.Infinity) / rotateStartThreadshold, 2f);
                        if (!AudSource.isPlaying)
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
        }
        else
        {
            playermove.Sleep();
        }
    }
    IEnumerator rotationRots()
    {
        float ContCurrentAngle = 0f;
        while (sys.RestTime > 0f)
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
                if (Mathf.Abs(ContCurrentAngle) > 3f)
                {
                    ContCurrentAngle = 3f * Mathf.Sign(ContCurrentAngle);
                }
                PrevAxis = MoveAxis;
            }
            ControllerRotationSpeed = (ControllerRotationSpeed + ContCurrentAngle) / (1f + Time.fixedDeltaTime);

            yield return null;
        }
    }

    void Animset()
    {
        playeranimator.SetBool("isRotating", isRotating);
        playeranimator.SetBool("isDamaged", recoverytime < recovreqtime);
        playeranimator.SetFloat("speed", playermove.velocity.magnitude);
    }
}
