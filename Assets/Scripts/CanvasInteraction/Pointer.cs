using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public float defaultLength = 5.0f;
    public GameObject Dot;
    public VRInputModule inputModule;

    private LineRenderer lineRenderer = null;

    private void Awake()
    {
        //referencing line renderer in inspector
        lineRenderer= GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        //Use default length for length of line
        float targetLength = defaultLength;

        //call raycast
        RaycastHit raycast = CreateRaycast(targetLength);

        //default end, if our raycast doesnt hit anything, the location of the dot will be at the end
        Vector3 endPosition = transform.position + (transform.forward * targetLength);

        //Check for any collider hit
        Ray ray = new Ray (transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit) )
        {
            if(hit.collider != null )
            {
                endPosition= hit.point;
            }
        }

        //Set position of dot
        Dot.transform.position = endPosition;

        //Set position for line renderer
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);

    }


    //Create Raycast to detect if pointer hits something
    private RaycastHit CreateRaycast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast (ray, out hit, defaultLength);

        return hit;
    }
}
