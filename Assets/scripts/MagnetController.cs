using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetController : MonoBehaviour
{
	[SerializeField]private float magneticForce = 500f;
	[SerializeField] private float collisionForce = 2000f;
	
	private Rigidbody2D rb;
    private List<MagneticObject> nearbyObjects;

    [SerializeField] private bool positiveCharge;

	public static bool falloff_mode = false;

	public bool PositiveCharge { get { return positiveCharge; } set { positiveCharge = value; } }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
		nearbyObjects = new List<MagneticObject>();
	}

	private void FixedUpdate()
	{
		int forceDirection;

		foreach (MagneticObject magneticObject in nearbyObjects)
		{
			forceDirection = positiveCharge != magneticObject.PositiveCharge ? 1 : -1;

			if (magneticObject.tag == "Enemy")
			{
				ApplyMagneticForce(rb, magneticObject.RB, forceDirection);
			}
			else if(magneticObject.tag == "MagneticGround")
			{
				ApplyMagneticForce(magneticObject.RB, rb, forceDirection);
			}
		}
	}

	/// <summary>
	/// Applies magnetic force
	/// </summary>
	/// <param name="effectingObject"></param>
	/// <param name="objectToEffect"></param>
	/// <param name="forceDirection"></param>
	public void ApplyMagneticForce(Rigidbody2D effectingObject, Rigidbody2D objectToEffect, int forceDirection)
	{
		Vector2 direction = new Vector2(effectingObject.position.x, effectingObject.position.y) - new Vector2(objectToEffect.position.x, objectToEffect.position.y);
        if (falloff_mode)
        {
			float distance = direction.magnitude;
			float forceMagnitude = magneticForce / Mathf.Pow(distance, 2);
			Vector2 force = direction.normalized * forceMagnitude * forceDirection;
			objectToEffect.AddForce(force);
        }
		else if (!falloff_mode)
        {
			objectToEffect.AddForce(magneticForce * forceDirection * direction.normalized);
        }
	}

	#region Colliders
	/// <summary>
	/// Adds repelling force to enemy on collision
	/// </summary>
	/// <param name="collision"></param>
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Enemy")
		{
			Rigidbody2D enemyRB = collision.gameObject.GetComponent<Rigidbody2D>();
			Vector2 direction = new Vector2(transform.position.x, transform.position.y) - new Vector2(collision.transform.position.x, collision.transform.position.y);
			Vector2 force = direction.normalized * collisionForce;
			enemyRB.velocity = Vector2.zero;
			collision.gameObject.GetComponent<MagneticObject>().RB.AddForce(-force);
		}
	}
	#endregion

	#region Trigger
	/// <summary>
	/// Enters magneticObject in nearbyObjects
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter2D(Collider2D other)
    {
		if (other.tag == "MagneticGround" || other.tag == "Enemy")
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
        if (other.tag == "MagneticGround" || other.tag == "Enemy")
        {
            if (nearbyObjects.Contains(other.GetComponent<MagneticObject>()))
            {
                nearbyObjects.Remove(other.GetComponent<MagneticObject>());
            }
        }
    }
    #endregion
}
