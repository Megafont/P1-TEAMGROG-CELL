using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{

    public float _speed;
    public float _damage;
    public float _size;
    public float _piercing;
    public Quaternion _direction;
    public Tower_Base _owner;

    private void Start()
    {
        transform.localScale *= _size;
    }

    void FixedUpdate()
    {
        transform.rotation = _direction;
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void Update()
    {
        if (_piercing <= 0 || (transform.position - _owner.transform.position).sqrMagnitude > 500.0f)
        {
            Destroy(gameObject);
        }
    }
}
