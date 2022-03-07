using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateBurrow : AttackState
{
    [SerializeField] private float goundedYPosition;
    [SerializeField] private float burrowYPosition;
    [SerializeField] private float particleYPosition;
    [SerializeField] private float maxRotationSpeed;
    [SerializeField] private float burrowSpeed;
    [SerializeField] private GameObject particles;

    public void Awake()
    {
        BossAnimationEventsHandler.current.OnBurrowDownFinished += BeginBurrowCoroutine;
    }

    public void Start()
    {
        base.Start();
    }//End Start

    public override void OnEnter()
    {
        base.OnEnter();
        //Start animation 
        StartCoroutine(StartBurrow());
    } //End OnEnter

    public override void OnExit()
    {
        base.OnExit(); 
    } //End OnExit

    public override void FixedRun()
    {
        base.FixedRun();
    }

    private void BeginBurrowCoroutine()
    {
        StartCoroutine(StartBurrow());
    }

    IEnumerator StartBurrow()
    {
        yield return new WaitForSeconds(1f);
        Vector3 newPosition = transform.position;
        newPosition.y = burrowYPosition;
        transform.position = newPosition;
        newPosition.y = particleYPosition;
        Instantiate(particles, newPosition, Quaternion.identity);
    }
}
