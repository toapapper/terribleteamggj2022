using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticObject : MonoBehaviour
{
	private Rigidbody2D rb;
	[SerializeField] private bool positiveCharge;

	public Rigidbody2D RB { get { return rb; } }
	public bool PositiveCharge { get { return positiveCharge; } }

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}
}
