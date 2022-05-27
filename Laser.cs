using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    //speed variable 8
    [SerializeField] private float _speed = 8.0f;

    // Update is called once per frame
    void Update()
    {
        //translate laser up
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        //if laser (transform.position.y is connected to laser) positon is greater than 8 on y
        //destroy the object
        if(transform.position.y > 8f)
        {   
            //destorys THIS game object being laser
            Destroy(this.gameObject);
        }


    }
}
