using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Pillar : MonoBehaviour
{
    private float waitTime, ascendSpeed, descendSpeed, stayTime;
    private Vector3 startPos, endPos;
    public event Action hitPlayer;

    [SerializeField] private AK.Wwise.Event spawnSound;
    [SerializeField] private AK.Wwise.Event breachGroundSound;
    [SerializeField] private AK.Wwise.Event hitSound;

    [SerializeField] private float groundPosition;
    private bool doOnce = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Move());
    }//End Start   

    IEnumerator Move()
    {
        spawnSound.Post(gameObject);
        yield return new WaitForSeconds(waitTime);

        //Move to the end position
        while (transform.position != endPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, ascendSpeed * Time.deltaTime); 

            if(gameObject.transform.position.y > groundPosition && doOnce)
            {
                doOnce = false;
                breachGroundSound.Post(gameObject);
            }

            yield return null;
        }

        yield return new WaitForSeconds(stayTime);

        //Move back to the start position
        while (transform.position != startPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, descendSpeed * Time.deltaTime);
            yield return null;
        }

        //Destroy the pillar
        Destroy(gameObject);
    }

    //Initialise the values of the pillar
    public void Initialise(float waitTime, float ascendSpeed, float descendSpeed, float stayTime, Vector3 startPos, Vector3 endPos)
    {
        this.waitTime = waitTime;
        this.ascendSpeed = ascendSpeed;
        this.descendSpeed = descendSpeed;
        this.stayTime = stayTime;
        this.startPos = startPos;
        this.endPos = endPos;

        transform.position = startPos;
    }

    private void OnDestroy()
    {
        hitPlayer = null;
        StopAllCoroutines();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Trigger the event if the player is hit
        if (other.tag == "Player")
        {
            hitPlayer?.Invoke();
            hitSound.Post(gameObject);
        }

        IDestructible obj = other.GetComponent<IDestructible>();
        if(obj != null) {
            obj.DestroyObject();
        }
    }
}
