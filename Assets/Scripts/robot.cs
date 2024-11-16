using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robot : MonoBehaviour
{
    public float speed = 1.0f;
    public bool move = false;

    public int facingDir = 1;

    int boxAmount = 0;

    public GameObject grabber;

    public collisionCheck frontHB;
    public string frontHBCheck; 
    public collisionCheck frontRightHB;
    public string frontRightHBCheck; 
    public collisionCheck rightHB;
    public string rightHBCheck; 
    public collisionCheck leftHB;
    public string leftHBCheck;
    bool carrying = false;

    private Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        
        if(move){
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }

        if(Input.GetKeyDown(KeyCode.W)){
            advance();
        }

        if(Input.GetKeyDown(KeyCode.S)){
            turnBack();
        }

        if(Input.GetKeyDown(KeyCode.D)){
            turnRight();
        }

        if(Input.GetKeyDown(KeyCode.A)){
            turnLeft();
        }
        if(Input.GetKeyDown(KeyCode.E)){
            grab();
        }
        if(Input.GetKeyDown(KeyCode.F)){
            drop();
        }

        if(Vector3.Distance(transform.position, target) == 0){
            move = false;
        }

        frontHBCheck = frontHB.getCheck();
        boxAmount = frontHB.getAmountBoxes();
        frontRightHBCheck = frontRightHB.getCheck();
        rightHBCheck = rightHB.getCheck();
        leftHBCheck = leftHB.getCheck();
        
    }

    void grab(){
        frontHB.pickUp();
        carrying = true;
        grabber.SetActive(true);
    }

    void drop(){
        carrying = false;
        grabber.SetActive(false);
        frontHB.addBox();
    }
    void advance(){
        move = true;
        target = transform.position + transform.forward;
    }

    void turnLeft(){
        transform.Rotate(0, -90, 0, Space.Self);
        if(facingDir == 1){
            facingDir = 4;
        } else {
            facingDir--;
        }
    }

    void turnRight(){
        transform.Rotate(0, 90, 0, Space.Self);

        if(facingDir == 4){
            facingDir = 1;
        } else {
            facingDir++;
        }
    }

    void turnBack(){
        transform.Rotate(0, 180, 0, Space.Self);
        if(facingDir > 2){
            facingDir -= 2;
        } else {
            facingDir+= 2;
        }
    }
}
