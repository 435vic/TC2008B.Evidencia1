using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class collisionCheck : MonoBehaviour
{
    public string Colided;
    public GameObject over;
    public boxPile front;
    public GameObject objectToSpawn;
    public int boxAmount;
    
    void Start(){
        Colided = "E";
    }
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Box")){
            Colided = "B";
            over = other.gameObject;
            front = over.GetComponent<boxPile>();
            boxAmount = front.getBoxes();
        } else if (other.CompareTag("Robot")){
            Colided = "R";
        } else if (other.CompareTag("Wall")){
            Colided = "W";
        }
        
    }

    private void OnTriggerExit(Collider other) {
        Colided = "E";
        front = null;
        over = null;
        boxAmount = 0;
    }

    public string getCheck(){
        return Colided;
    }

    public int getAmountBoxes(){
        return boxAmount;
    }
    
    public void pickUp(){
        if(over.CompareTag("Box")){
            Destroy(over);
            Colided = "E";
            boxAmount = 0;
        }
    }

    public void addBox(){
        if(Colided == "E"){

            float xPos = this.transform.position.x;
            float zPos = this.transform.position.z;
            Instantiate(
                objectToSpawn,
                new Vector3(xPos, 0.5f, zPos),
                new Quaternion()
            );
        } else if (Colided == "B" && boxAmount < 5){
            boxAmount++;
            front.addBox();
        }
    }
}
