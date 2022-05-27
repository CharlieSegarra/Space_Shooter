using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
[SerializeField] private float _speed =3;
//0= triple shot
//1= speed
//2 == shields
[SerializeField] private int powerupId;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move down at the speed of 3
        //when leave screen destroy 
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // if the object goes off screen
        if(transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    //Did player collide with powerup
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            //comunicate with the player script
            //handle to the component
            //assign handle to the component
            Player player = other.transform.GetComponent<Player>();

                switch(powerupId)
                {
                case 0:
                    player.TripleShotActive();
                    break;
                case 1:
                    player.SpeedBoostActive();
                    break;
                case 2:
                   player.ShieldsActive();
                    break;
                default:
                    Debug.Log("Default");
                    break;
                //else if powerUP is 1
                //play speed powerUp
                //else if powerUp is 2
                //play shields pwerUp
                }

        }
                Destroy(this.gameObject);
    }
}

  

