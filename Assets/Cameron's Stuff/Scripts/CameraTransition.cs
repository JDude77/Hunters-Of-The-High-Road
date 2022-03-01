using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    [SerializeField]
    private Transform cineCamPos;

    private Vector3 startPos;
    private Quaternion startRot;

    [SerializeField]
    private Quaternion rotateTo;


    private bool hasGameStarted = false, hasTransitioned = false;

    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = cineCamPos;

        startPos = transform.localPosition;
        startRot = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(hasGameStarted && !hasTransitioned)
        {
            timer += 0.1f * Time.deltaTime;

            transform.localPosition = Vector3.Lerp(startPos, Vector3.zero, timer);
            transform.localRotation = Quaternion.Lerp(startRot, rotateTo, timer);
        }
    }

    public void startGame()
    {
        hasGameStarted = true;
    }
}
