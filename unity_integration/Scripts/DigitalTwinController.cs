using UnityEngine;
using System;
using System.Text;
using System.Collections;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

[Serializable]
public class MachineData
{
    public string machine_id;
    public float vibration;
    public float temperature;
    public float current;
    public bool is_anomaly;
    public float confidence;
    public string recommendation;
}

public class DigitalTwinController : MonoBehaviour
{
    [Header("MQTT Configuration")]
    public string brokerAddress = "broker.hivemq.com";
    public int brokerPort = 1883;
    public string topic = "pfe/machine01/analysis";

    [Header("Visuals")]
    public Renderer machineRenderer;
    public Color normalColor = Color.green;
    public Color anomalyColor = Color.red;
    public float shakeMultiplier = 0.1f;

    private MqttClient client;
    private MachineData currentData;
    private bool dataReceived = false;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
        ConnectToMQTT();
    }

    void ConnectToMQTT()
    {
        try
        {
            client = new MqttClient(brokerAddress);
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            Debug.Log("Connected to MQTT Broker!");
        }
        catch (Exception e)
        {
            Debug.LogError("Connection failed: " + e.Message);
        }
    }

    private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        string json = Encoding.UTF8.GetString(e.Message);
        Debug.Log("Received: " + json);

        try
        {
            // Parse JSON (Ensure fields match your JSON structure)
            currentData = JsonUtility.FromJson<MachineData>(json);
            dataReceived = true;
        }
        catch (Exception ex)
        {
            Debug.LogError("JSON Parse Error: " + ex.Message);
        }
    }

    public MachineData GetCurrentData()
    {
        return currentData;
    }

    void Update()
    {
        if (dataReceived)
        {
            UpdateVisuals();
            dataReceived = false; // Reset flag
        }

        // Continuous Shake Effect based on vibration value
        if (currentData != null && currentData.vibration > 0)
        {
            float shakeAmount = currentData.vibration * shakeMultiplier * 0.05f;
            transform.position = initialPosition + UnityEngine.Random.insideUnitSphere * shakeAmount;
        }
    }

    void UpdateVisuals()
    {
        if (machineRenderer != null)
        {
            if (currentData.is_anomaly)
            {
                machineRenderer.material.color = anomalyColor;
            }
            else
            {
                machineRenderer.material.color = normalColor;
            }
        }
    }

    void OnApplicationQuit()
    {
        if (client != null && client.IsConnected)
        {
            client.Disconnect();
        }
    }
}
