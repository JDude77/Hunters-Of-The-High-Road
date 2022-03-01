using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeTutorialText : MonoBehaviour
{
    private TextMeshPro text;
    private bool countUp = false;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        text.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(countUp)
        {
            if (timer < 1)
            {
                timer += Time.deltaTime;
                text.alpha = timer;
            }
        }
        else
        {
            if(timer > 0)
            {
                timer -= Time.deltaTime;
                text.alpha = timer;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            countUp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            countUp = false;
        }
    }
}
