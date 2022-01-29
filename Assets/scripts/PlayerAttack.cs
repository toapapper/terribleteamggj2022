using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCD = 0.5f;
    private float timer;
    private List<GameObject> objectsWithin;
    // Start is called before the first frame update
    void Start()
    {
        objectsWithin = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("tried to attack");
            if (timer >= attackCD)
            {
                timer = 0;
                Debug.Log("attacked");
                Attack();
            }
        }
    }
    private void Attack()
    {
        Vector3 direction = Vector3.zero;
        for (int i = 0; i < objectsWithin.Count; i++)
        {
            direction += transform.position - objectsWithin[i].transform.position;
        }
        direction.Normalize();
        direction.z = 0;
        Debug.Log("DIR = " + direction);
        GetComponentInParent<Rigidbody2D>().AddForce(direction * 3000);
        GetComponentInParent<PlayerMovement>().ChangePole();

    }


    IEnumerator AttackIEnum(float time)
    {
        //visual start
        yield return new WaitForSeconds(time);
        //visual end or something
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MagneticGround") && !objectsWithin.Contains(collision.gameObject))
        {
            objectsWithin.Add(collision.gameObject);
            Debug.Log("ADDED");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("MagneticGround") && objectsWithin.Contains(collision.gameObject))
        {
            objectsWithin.Remove(collision.gameObject);
            Debug.Log("Removed");
        }
    }

}
