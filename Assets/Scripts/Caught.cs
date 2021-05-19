using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caught : MonoBehaviour
{
    private TornadoScript tornadoReference;
    private SpringJoint spring;
    private Rigidbody rigid;


	void Start ()
	{
	    rigid = GetComponent<Rigidbody>();
	}
	
	void Update ()
	{
        Vector3 newPosition = spring.connectedAnchor;
        newPosition.y = transform.position.y;
        spring.connectedAnchor = newPosition;
        var newScale = Mathf.Lerp(transform.localScale.x, 0, Time.deltaTime*.7f);
        transform.localScale = new Vector3(newScale, newScale, newScale);
        if (transform.localScale.x<.3f)
        {
            Destroy(gameObject);
        }

        Debug.Log(rigid.velocity.magnitude);

    }

	Vector3 norm;

    void FixedUpdate()
    {
        Vector3 direction = transform.position - tornadoReference.transform.position;
        Vector3 projection = Vector3.ProjectOnPlane(direction, tornadoReference.GetRotationAxis());
        projection.Normalize();
	    Vector3 normal = Quaternion.AngleAxis(130, tornadoReference.GetRotationAxis()) * projection;
        normal = Quaternion.AngleAxis(tornadoReference.lift, projection) * normal;
		norm = normal;
        rigid.AddForce(normal*tornadoReference.GetStrength(), ForceMode.Force);
        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, 18);
    }

	void LateUpdate()
	{
		Debug.DrawRay(transform.position, norm*10, Color.red);

	}

    //call this when tornadoReference already exists
    public void Init(TornadoScript tornadoRef, float springForce, float damper, float maxDistance, float minDistance)
    {
        //make sure this s enabled (for reentrance)
        enabled = true;

        //save tornado reference
        tornadoReference = tornadoRef;

        //initialize the spring
        spring = gameObject.AddComponent<SpringJoint>();
        spring.spring = springForce;
        spring.damper = damper;
        spring.maxDistance = maxDistance;
        spring.minDistance = minDistance;
        spring.connectedBody = tornadoRef.gameObject.GetComponent<Rigidbody>();



        spring.autoConfigureConnectedAnchor = false;

        //set initial position of the caught object relative to its position and the tornado
        Vector3 initialPosition = Vector3.zero;
        initialPosition.y = transform.position.y;
        spring.connectedAnchor = initialPosition;

    }

    public void Release()
    {
        enabled = false;
        Destroy(spring);
    }

}
