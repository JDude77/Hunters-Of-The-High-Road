using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reticle : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image rotator;
    [Tooltip("Speed at which the rotator will rotate")]
    [SerializeField] private float rotationSpeed;
    [Tooltip("The angle from 90 degrees at which the deadshot will return true")]
    [SerializeField] private float skillCheckAngleSize;

    // Start is called before the first frame update
    void Start()
    {
        background.gameObject.SetActive(false);
        rotator.gameObject.SetActive(false);
        rotator.transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Move to mouse position
        transform.position = Input.mousePosition;
        //Rotate the reticle
        float currentRot = rotator.rectTransform.rotation.eulerAngles.z;
        rotator.rectTransform.rotation = Quaternion.AngleAxis(currentRot + rotationSpeed * Time.deltaTime, Vector3.forward);
        //Reset the rotation
        if(Mathf.Abs(rotator.rectTransform.rotation.eulerAngles.z) >= 180f) {
            rotator.transform.rotation = Quaternion.identity;
        }
    }

    public void Activate() {   
        //Enable the reticle and reset the rotation
        Cursor.visible = false;
        rotator.transform.rotation = Quaternion.identity;
        enabled = true;
        background.gameObject.SetActive(true);
        rotator.gameObject.SetActive(true);
    }

    public void Deactivate() {
        //Disable the reticle
        Cursor.visible = true;
        enabled = false;
        background.gameObject.SetActive(false);
        rotator.gameObject.SetActive(false);
    }

    public bool IsSuccessful() {
        float currentRot = Mathf.Abs(rotator.rectTransform.rotation.eulerAngles.z);

        return currentRot >= 90f - skillCheckAngleSize 
            && currentRot <= 90f + skillCheckAngleSize;
    }

    public void OverrideValues(float rotationSpeed, float angle) {
        skillCheckAngleSize = angle;
        this.rotationSpeed = rotationSpeed;
    }

    void OnDrawGizmos() {
        // Draws a blue line from this transform to the target
        Vector3 dir = Quaternion.Euler(0, 0, skillCheckAngleSize) * Vector3.right;
        dir = Quaternion.Euler(0, 0, skillCheckAngleSize) * Vector3.right;
        float dist = 100f;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + dir * dist);        
        Gizmos.DrawLine(transform.position, transform.position - dir * dist);
        dir = Quaternion.Euler(0, 0, -skillCheckAngleSize) * Vector3.right;
        Gizmos.DrawLine(transform.position, transform.position + dir * dist);
        Gizmos.DrawLine(transform.position, transform.position - dir * dist);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.left * dist, transform.position + Vector3.right * dist);

    }
}
