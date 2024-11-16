using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;


public class robot : MonoBehaviour
{
    public int socket = 65432;
    public float speed = 1.0f;
    public bool move = false;
    public int facingDir = 1;
    string[] directions = new string[4]{"n", "e", "s", "w"};
    private Vector3 target;

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
    TcpClient client;
    NetworkStream stream;
    byte[] receivedBuffer = new byte[1024];
    int bytes;
    string responseData;


    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(1, 5);
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

        if(Vector3.Distance(transform.position, target) == 0){
            move = false;
            updateKnowledge();
            
        }
    }

    public IEnumerator Delay()
    {        
        yield return new WaitForSeconds(1);
        getMessage();    
    }

    public void setUpSocket(){
        //client = new TcpClient("localhost", socket);
        //stream = client.GetStream();
    }

    void getMessage(){
        bytes = stream.Read(receivedBuffer, 0, receivedBuffer.Length);
        responseData = Encoding.ASCII.GetString(receivedBuffer, 0, bytes);
        Debug.Log("Recibido: " + responseData);
    }

    void buildMessage(){
        message = directions[facingDir-1] + " " +
        carrying + " " +
        leftHBCheck + " " + frontHBCheck + " " + frontRightHBCheck + " " + rightHBCheck + " " +
        boxAmount;

        Debug.Log(message);
        byte[] dataToSend = Encoding.ASCII.GetBytes(message);
        stream.Write(dataToSend, 0, dataToSend.Length);
    }

    void updateKnowledge(){
        frontHBCheck = frontHB.getCheck();
        boxAmount = frontHB.getAmountBoxes();
        frontRightHBCheck = frontRightHB.getCheck();
        rightHBCheck = rightHB.getCheck();
        leftHBCheck = leftHB.getCheck();
        buildMessage();
        Delay();
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
        updateKnowledge();
    }
    void advance(){
        if(!move){
            move = true;
            target = transform.position + transform.forward;
        }
    }

    void turnLeft(){
        transform.Rotate(0, -90, 0, Space.Self);
        if(facingDir == 1){
            facingDir = 4;
        } else {
            facingDir--;
        }
        updateKnowledge();
    }

    void turnRight(){
        transform.Rotate(0, 90, 0, Space.Self);

        if(facingDir == 4){
            facingDir = 1;
        } else {
            facingDir++;
        }
        updateKnowledge();
    }

    void turnBack(){
        transform.Rotate(0, 180, 0, Space.Self);
        if(facingDir > 2){
            facingDir -= 2;
        } else {
            facingDir+= 2;
        }
        updateKnowledge();
    }
}
