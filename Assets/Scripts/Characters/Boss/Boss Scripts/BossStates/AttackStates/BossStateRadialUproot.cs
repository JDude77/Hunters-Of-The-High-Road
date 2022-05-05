using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateRadialUproot : BossStatePillarAttack
{
    [Header("Radial attack settings")]
    [Tooltip("Length of the list = amount of circles that will spawn. Number of pillars should go from smallest to largest")]
    [SerializeField] private List<int> pillarCountPerCircle;
    [Tooltip("Radius of the smallest circle")]
    [SerializeField] private float innerRadius;
    [Tooltip("Radius of the largest circle")]
    [SerializeField] private float outerRadius;

    [Header("Sounds")]
    [SerializeField] private AK.Wwise.Event attackSound;

    private void Start() {
        base.Start();
        InitEvents();
    }//End Start

    public override void OnEnter() {
        base.OnEnter();
        canBeStunned = true;
        boss.animator.SetTrigger("DoRadialUproot");
    }//End OnEnter

    public override void OnExit() {
        base.OnExit();
        StopAllCoroutines();
    }//End OnExit

    IEnumerator DoAttack() {

        float circleInterval = (outerRadius - innerRadius) / (float)pillarCountPerCircle.Count;

        //for each circle
        for (int p = 0; p < pillarCountPerCircle.Count; p++) 
        {
            float distance = circleInterval * p + innerRadius;
            SpawnCircle(pillarCountPerCircle[p], distance);

            yield return new WaitForSeconds(delayBetweenPillars);
        }//End for

        boss.ReturnToMainState();
    }//End DoAttack

    void SpawnCircle(int pCount, float radius) {
        float rotationInterval = 1f / (float)pCount;
        Vector3 dir;
        float rot;
        //spawn all of its pillars
        for (int i = 0; i < pCount; i++) {
            rot = i * rotationInterval * (360f - rotationInterval);
            
            Quaternion newRotation = Quaternion.AngleAxis(rot, Vector3.up);
            dir = newRotation * transform.forward;

            Vector3 position = transform.position + dir * radius;            
            SpawnPillar(position, newRotation);
        }//End for
    }//End SpawnCircle

    void InitEvents() {
        eventResponder.AddAction("DoAttack", () => { StartCoroutine(DoAttack()); });
        eventResponder.AddSoundEffect("AttackSound", attackSound, gameObject);
    }//End InitEvents
}
