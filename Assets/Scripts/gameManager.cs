using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public GameObject[] spawns;
    public robot[] robots;

    public boxPile spawnRef;

    int counter = 0;
    public int boxNumber = 20;

    int[] positions = new int[]{0, 1, 2, 3, 4};
    string[] robotMessages = new string[5];
    string[] pythonInstruction = new string[5];
    NetworkStream stream;
    byte[] receivedBuffer = new byte[1024];

    public static gameManager Instance {
        get;
        private set;
    }
    void Awake(){
        if(Instance != null){
            Destroy(gameObject);
        } else {
            // If not, we pick the space
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ShufflePositions();
        spawnRobots();
        spawnBoxes();

        TcpClient client = new TcpClient("localhost", 65432);
        stream = client.GetStream();   
    }

    void Update(){
        if(counter == 5){
            counter = 0;
            sendToPython();
        }
        int bytes = stream.Read(receivedBuffer, 0, receivedBuffer.Length);
        string responseData = Encoding.ASCII.GetString(receivedBuffer, 0, bytes);
        // Fill the robotMessages array with the responseData
        //controlRobots(); 
        Debug.Log("Recibido: " + responseData);
    }

    void ShufflePositions(){
        int temp_value;
        int random_index;
        int size = positions.Length;
        for (int i = 0; i < size; i++){
            temp_value = positions[i];
            random_index = Random.Range(0, size);
            positions[i] = positions[random_index];
            positions[random_index] = temp_value;
        }
    }

    void spawnRobots(){
        for (int i = 0; i < robots.Length; i++){
            robots[i].transform.position = spawns[positions[i]].transform.position;
            robots[i].transform.rotation = spawns[positions[i]].transform.rotation;
            robots[i].id += i;
        }
    }

    void spawnBoxes(){
        float xPos = 0.5f;
        float zPos = 1.5f;
        int numOfRow = 0;
        int counter = 0;
        Quaternion defRot = new Quaternion();
        while(counter < boxNumber && numOfRow < 9){
            zPos += Random.Range(2, 5);
            if(zPos <= 9.5){
                counter++;
                Instantiate(
                    spawnRef,
                    new Vector3(xPos, 0.5f, zPos),
                    defRot
                );
            } else {
                zPos = 1.5f;
                numOfRow++;
                xPos++;
            }   
        }
    }

    public void answerFromRobot(string message, int id){
        robotMessages[id] = message;
        counter++;
    }

    void controlRobots(){
        for (int i = 0; i < robots.Length; i++){
            robots[i].getMessage(pythonInstruction[i]);
        }
    }

    void sendToPython(){
        // use the robotMessages array to send the message to python
        string fullMessage = ""; // Enter message here
        byte[] dataToSend = Encoding.ASCII.GetBytes(fullMessage);
        stream.Write(dataToSend, 0, dataToSend.Length);
    }
}
