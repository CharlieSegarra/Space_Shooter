using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    // Start is called before the first frame update
[SerializeField] private float _rotateSpeed = 3.0f;
[SerializeField] private GameObject _explosionPrefab;
private SpawnManager _spawnManager;



    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed *Time.deltaTime); //rotates astroid forward
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.25f);
            _spawnManager.StartSpawining();
            Destroy(other.gameObject);
        }
    }        



}
