using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionCheck : MonoBehaviour
{
    public string Colided;
    public GameObject over;

    void Start(){
        Colided = "E";
    }
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Box")){
            Colided = "B";
            over = other.gameObject;
        } else if (other.CompareTag("Robot")){
            Colided = "R";
        } else if (other.CompareTag("Wall")){
            Colided = "W";
        }
        
    }

    private void OnTriggerExit(Collider other) {
        Colided = "E";
        over = null;
    }

    public string getCheck(){
        return Colided;
    }
    
    public void pickUp(){
        if(over.CompareTag("Box")){
            Destroy(over);
        }
    }
}
