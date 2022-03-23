using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolderBoss : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    private bool doDramaticTurn = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(doDramaticTurn)
        {
            Vector3 targetPos = player.position;
            targetPos.y = transform.position.y;

            Quaternion lookPos = Quaternion.LookRotation(targetPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookPos, 1 * Time.deltaTime);

        }
    }

    public void activateDramaticTurn()
    {
        doDramaticTurn = true;
    }
}
