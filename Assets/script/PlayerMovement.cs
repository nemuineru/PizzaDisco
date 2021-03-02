using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
<<<<<<< HEAD
    public float speed = 250f;
    public float InitSpeed = 250f;
    public float SuperSpeed = 350f;
    public int InitPower = 1;
    public int SuperPower = 5;
    public int Power = 1;
    public float PowerUpTime;
    public float SpeedUpTime;
    public float ControllerRotationSpeed = 0f;
    Vector3 MoveAxis = new Vector3(),  PrevAxis;
    public bool isRotating = false;
    public ParticleSystem SuperEffect_Speed,SuperEffect_Power;

    public Systems sys;

    public AudioClip Whooshsound;
    float whooshSoundmintime = 5f, currentWhooshRotate = 0f;
    
    public float invisibletime = 1f, currentinvistime = 1000f;
    public float recovreqtime = 1f, recoverytime = 100f;
=======
    public float speed = 10f;
    public float ControllerRotationSpeed = 0f;
    Vector3 MoveAxis = new Vector3(),  PrevAxis;
    public bool isRotating = false;

    public AudioClip Whooshsound;
    float whooshSoundmintime = 5f, currentWhooshRotate = 0f;
>>>>>>> 2fa4ebd4cacf47228710a90b50325da8bcd412fc

    AudioSource AudSource;
    Rigidbody playermove;
    Animator playeranimator;

    float rotateMinThreadshold = 35f, rotateStartThreadshold = 50f;

<<<<<<< HEAD
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

=======
>>>>>>> 2fa4ebd4cacf47228710a90b50325da8bcd412fc
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
<<<<<<< HEAD
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
                            ("PitchShifter", Mathf.Pow(Mathf.Clamp(Mathf.Abs(ControllerRotationSpeed), 0f, Mathf.Infinity) / rotateStartThreadshold, 1 / 2f) / 32f);
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
        else {
            playermove.Sleep();
=======
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
>>>>>>> 2fa4ebd4cacf47228710a90b50325da8bcd412fc
        }
    }

    IEnumerator rotationRots()
    {
        float ContCurrentAngle = 0f;
<<<<<<< HEAD
        while (sys.RestTime > 0f)
=======
        while (true)
>>>>>>> 2fa4ebd4cacf47228710a90b50325da8bcd412fc
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
<<<<<<< HEAD
            ControllerRotationSpeed = (ControllerRotationSpeed + ContCurrentAngle) / (1f + Time.fixedDeltaTime);
=======
            ControllerRotationSpeed = (ControllerRotationSpeed + ContCurrentAngle) / (1f + Time.deltaTime);
>>>>>>> 2fa4ebd4cacf47228710a90b50325da8bcd412fc

            yield return null;
        }
    }

    void Animset() {
        playeranimator.SetBool("isRotating", isRotating);
<<<<<<< HEAD
        playeranimator.SetBool("isDamaged", recoverytime < recovreqtime);
=======
>>>>>>> 2fa4ebd4cacf47228710a90b50325da8bcd412fc
        playeranimator.SetFloat("speed",playermove.velocity.magnitude);
    }
}
