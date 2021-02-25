using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    float speed = 10f;
    public float currentRotationSpeed;
    public float ControllerRotationSpeed = 0f;
    Vector3 MoveAxis = new Vector3(),  PrevAxis;
    public bool isRotating = false;

    Rigidbody playermove;

    float rotateMinThreadshold = 5f, rotateStartThreadshold = 10f;

    // Start is called before the first frame update
    void Start()
    {
        playermove = GetComponent<Rigidbody>();
        StartCoroutine(rotationRots());
    }

    // Update is called once per frame
    void Update()
    {
        playermove.AddForce(MoveAxis,ForceMode.VelocityChange);

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
}
