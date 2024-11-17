using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;


public class robot : MonoBehaviour
{
    public int id = 0;
    public float speed = 1.0f;
    public bool move = false;
    public int facingDir = 1;
    string[] directions = new string[4]{"n", "e", "s", "w"};
    private Vector3 target;
    public GameObject siren;
    public GameObject grabber;

    public collisionCheck frontHB;
    public string frontHBCheck; 
    public collisionCheck frontRightHB;
    public string frontRightHBCheck; 
    public collisionCheck rightHB;
    public string rightHBCheck; 
    public collisionCheck leftHB;
    public string leftHBCheck;
    public int boxAmount = 0;

    public bool carrying = false;

    string message;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(1, 5);
        StartCoroutine(Siren());
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

        if(Input.GetKeyDown(KeyCode.M)){
            buildMessage();
        }
    }


    public IEnumerator Siren(){
        while(true){
            siren.SetActive(!siren.activeSelf);
            yield return new WaitForSeconds(1);
        }
    }

    public IEnumerator TurnDelay()
    {        
        yield return new WaitForSeconds(0.1f);
        updateKnowledge();    
    }
    public IEnumerator Wait()
    {        
        yield return new WaitForSeconds(1);
        updateKnowledge();    
    }

    public IEnumerator Stop(){
        while(Vector3.Distance(transform.position, target) != 0){
            yield return Time.deltaTime;
        }
        move = false;
        updateKnowledge();
    }

    public void getMessage(string responseData){
        if (responseData == "F"){
            advance();
        } else if (responseData == "L"){
            turnLeft();
        } else if (responseData == "R"){
            turnRight();
        } else if (responseData == "B"){
            turnBack();
        } else if (responseData == "G"){
            grab();
        } else if (responseData == "D"){
            drop();
        } else if (responseData == "W"){
            StartCoroutine(Wait());
        }
        Debug.Log("Recibido: " + responseData);
    }

    void buildMessage(){
        message = directions[facingDir-1] + " " +
        carrying + " " +
        leftHBCheck + " " + frontHBCheck + " " + frontRightHBCheck + " " + rightHBCheck + " " +
        boxAmount;
        gameManager.Instance.answerFromRobot(message, id);
    }

    void updateKnowledge(){
        frontHBCheck = frontHB.getCheck();
        boxAmount = frontHB.getAmountBoxes();
        frontRightHBCheck = frontRightHB.getCheck();
        rightHBCheck = rightHB.getCheck();
        leftHBCheck = leftHB.getCheck();
        buildMessage();
    }

    void grab(){
        frontHB.pickUp();
        carrying = true;
        grabber.SetActive(true);
        boxAmount = 0;
        updateKnowledge();
    }

    void drop(){
        carrying = false;
        grabber.SetActive(false);
        frontHB.addBox();
        frontHBCheck = frontHB.getCheck();
        boxAmount = frontHB.getAmountBoxes();
        StartCoroutine(TurnDelay());
    }
    void advance(){
        if(!move){
            move = true;
            target = transform.position + transform.forward;
            StartCoroutine(Stop());
        }
    }

    void turnLeft(){
        transform.Rotate(0, -90, 0, Space.Self);
        if(facingDir == 1){
            facingDir = 4;
        } else {
            facingDir--;
        }
        StartCoroutine(TurnDelay());
    }

    void turnRight(){
        transform.Rotate(0, 90, 0, Space.Self);

        if(facingDir == 4){
            facingDir = 1;
        } else {
            facingDir++;
        }
        StartCoroutine(TurnDelay());
    }

    void turnBack(){
        transform.Rotate(0, 180, 0, Space.Self);
        if(facingDir > 2){
            facingDir -= 2;
        } else {
            facingDir+= 2;
        }
        StartCoroutine(TurnDelay());
    }
}
