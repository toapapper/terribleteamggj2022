using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetController : MonoBehaviour
{
	[SerializeField]private float magneticForce_on_others = 150f;
	[SerializeField]private float magneticForce_on_me = 250f;
	[SerializeField] private float collisionForce = 2000f;

	private Rigidbody2D rb;
    private List<MagneticObject> nearbyObjects;
	private CircleCollider2D collider;
	[SerializeField] private SpriteRenderer circle;

    [SerializeField] private bool positiveCharge;

	[SerializeField] public bool falloff_mode = false;

	[SerializeField] private float radius = 2;

	[SerializeField] private float towards_ratio = .3f;

	Color currentFieldColor = Color.red;

	public bool PositiveCharge { get { return positiveCharge; } set { positiveCharge = value; } }

    void Awake()
    {
		rb = GetComponent<Rigidbody2D>();
		nearbyObjects = new List<MagneticObject>();
		collider = GetComponent<CircleCollider2D>();
		collider.radius = radius;


		if (circle != null)
		{
			if (positiveCharge)
			{
				currentFieldColor = Color.red;
				circle.color = Color.red;
			}
			else if (!positiveCharge)
			{
				currentFieldColor = Color.blue;
				circle.color = Color.blue;
			}

			circle.transform.localScale = new Vector3(radius, radius, radius);
		}
	}

	private void FixedUpdate()
	{
		int forceDirection;

		foreach (MagneticObject magneticObject in nearbyObjects)
		{
			forceDirection = positiveCharge != magneticObject.PositiveCharge ? 1 : -1;

			if (magneticObject.tag == "Enemy")
			{
				ApplyMagneticForce(rb, magneticObject.RB, forceDirection, magneticForce_on_others, false);
			}
			else if(magneticObject.tag == "MagneticGround")
			{
				ApplyMagneticForce(magneticObject.RB, rb, forceDirection, magneticForce_on_me, true);
			}
			else if(magneticObject.tag == "FinishTrigger")
            {
				ApplyMagneticForce(magneticObject.RB, rb, forceDirection, magneticForce_on_me, true);
			}
		}
		collider.radius = radius;

		if(circle != null)
        {
			if(positiveCharge && currentFieldColor != Color.red)
			{
				currentFieldColor = Color.red;
				circle.color = Color.red;
			}
			else if(!positiveCharge && currentFieldColor != Color.cyan)
			{
				currentFieldColor = Color.blue;
				circle.color = Color.blue;
			}

			circle.transform.localScale = new Vector3(radius, radius, radius);
        }

	}

	/// <summary>
	/// Applies magnetic force
	/// </summary>
	/// <param name="effectingObject"></param>
	/// <param name="objectToEffect"></param>
	/// <param name="forceDirection"></param>
	public void ApplyMagneticForce(Rigidbody2D effectingObject, Rigidbody2D objectToEffect, int forceDirection, float force, bool fallof)
	{
		if(forceDirection == 1)
        {
			force *= towards_ratio;
        }

		Vector2 direction = new Vector2(effectingObject.position.x, effectingObject.position.y) - new Vector2(objectToEffect.position.x, objectToEffect.position.y);
        if (fallof)
        {
			float distance = direction.magnitude;
			float forceMagnitude = force / Mathf.Pow(distance, 2);
			Vector2 forceV = direction.normalized * forceMagnitude * forceDirection;
			objectToEffect.AddForce(forceV);
        }
		else if (!fallof)
        {
			objectToEffect.AddForce(force * forceDirection * direction.normalized);
        }
	}

	//Metod som bara k�rs i editorn, n�r ett v�rde �ndrats
    private void OnValidate()
    {
		if(collider == null)
        {
			collider = GetComponent<CircleCollider2D>();
        }

		collider.radius = radius;

		if (circle != null)
		{
			if (positiveCharge && currentFieldColor != Color.red)
			{
				currentFieldColor = Color.red;
				circle.color = Color.red;
			}
			else if (!positiveCharge && currentFieldColor != Color.cyan)
			{
				currentFieldColor = Color.blue;
				circle.color = Color.blue;
			}

			circle.transform.localScale = new Vector3(radius, radius, radius);
		}
	}

//#region Colliders
//    /// <summary>
//    /// Adds repelling force to enemy on collision
//    /// </summary>
//    /// <param name="collision"></param>
//    private void OnCollisionEnter2D(Collision2D collision)
//	{
//		if (collision.gameObject.tag == "Enemy")
//		{
//			Rigidbody2D enemyRB = collision.gameObject.GetComponent<Rigidbody2D>();
//			Vector2 direction = new Vector2(transform.position.x, transform.position.y) - new Vector2(collision.transform.position.x, collision.transform.position.y);
//			Vector2 force = direction.normalized * collisionForce;
//			enemyRB.velocity = Vector2.zero;
//			collision.gameObject.GetComponent<MagneticObject>().RB.AddForce(-force);
//		}
//	}
//#endregion

#region Trigger
	/// <summary>
	/// Enters magneticObject in nearbyObjects
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter2D(Collider2D other)
    {
		if (other.tag == "MagneticGround" || other.tag == "Enemy" || other.tag == "FinishTrigger")
        {
			if (!nearbyObjects.Contains(other.GetComponent<MagneticObject>()))
            {
                nearbyObjects.Add(other.GetComponent<MagneticObject>());
            }
        }
    }
    /// <summary>
    /// Removes magneticObject from nearbyObjects
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "MagneticGround" || other.tag == "Enemy" || other.tag == "FinishTrigger")
        {
            if (nearbyObjects.Contains(other.GetComponent<MagneticObject>()))
            {
                nearbyObjects.Remove(other.GetComponent<MagneticObject>());
            }
        }
    }
#endregion
}
