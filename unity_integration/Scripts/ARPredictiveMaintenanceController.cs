using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using System.Collections.Generic;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Text;
using System;

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

public class ARPredictiveMaintenanceController : MonoBehaviour
{
    [Header("AR Configuration")]
    public ARTrackedImageManager trackedImageManager;
    public GameObject arContentPrefab; // Le moteur 3D + UI

    [Header("MQTT Configuration")]
    public string brokerAddress = "broker.hivemq.com";
    public int brokerPort = 1883;
    public string topic = "pfe/machine01/analysis";

    [Header("3D Model")]
    public Renderer machineRenderer;
    public Color normalColor = Color.green;
    public Color anomalyColor = Color.red;

    [Header("UI Elements")]
    public Text vibrationText;
    public Text temperatureText;
    public Text currentText;
    public Text statusText;
    public Text recommendationText;
    public Image statusIcon;

    private MqttClient mqttClient;
    private MachineData currentData;
    private bool dataReceived = false;
    private GameObject spawnedObject;
    private Vector3 initialPosition;

    void Awake()
    {
        // Subscribe to image tracking events
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void Start()
    {
        ConnectToMQTT();
    }

    void ConnectToMQTT()
    {
        try
        {
            mqttClient = new MqttClient(brokerAddress);
            mqttClient.MqttMsgPublishReceived += OnMqttMessageReceived;
            string clientId = Guid.NewGuid().ToString();
            mqttClient.Connect(clientId);
            mqttClient.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            Debug.Log("✅ Connected to MQTT Broker!");
        }
        catch (Exception e)
        {
            Debug.LogError("❌ MQTT Connection failed: " + e.Message);
        }
    }

    private void OnMqttMessageReceived(object sender, MqttMsgPublishEventArgs e)
    {
        string json = Encoding.UTF8.GetString(e.Message);
        Debug.Log("📩 Received: " + json);

        try
        {
            currentData = JsonUtility.FromJson<MachineData>(json);
            dataReceived = true;
        }
        catch (Exception ex)
        {
            Debug.LogError("JSON Parse Error: " + ex.Message);
        }
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Image détectée pour la première fois
        foreach (var trackedImage in eventArgs.added)
        {
            Debug.Log($"🎯 Image détectée: {trackedImage.referenceImage.name}");
            SpawnARContent(trackedImage);
        }

        // Image suivie (mise à jour position)
        foreach (var trackedImage in eventArgs.updated)
        {
            if (spawnedObject != null)
            {
                spawnedObject.transform.position = trackedImage.transform.position;
                spawnedObject.transform.rotation = trackedImage.transform.rotation;
                
                // Afficher/Masquer selon le tracking
                spawnedObject.SetActive(trackedImage.trackingState == TrackingState.Tracking);
            }
        }

        // Image perdue
        foreach (var trackedImage in eventArgs.removed)
        {
            if (spawnedObject != null)
            {
                spawnedObject.SetActive(false);
            }
        }
    }

    void SpawnARContent(ARTrackedImage trackedImage)
    {
        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(arContentPrefab, trackedImage.transform);
            initialPosition = spawnedObject.transform.localPosition;
            
            // Récupérer les références UI et 3D depuis le prefab
            SetupReferences();
        }
    }

    void SetupReferences()
    {
        if (spawnedObject != null)
        {
            // Chercher les composants dans le prefab spawné
            machineRenderer = spawnedObject.GetComponentInChildren<Renderer>();
            
            // Chercher les Text UI (doivent être dans le prefab)
            Text[] texts = spawnedObject.GetComponentsInChildren<Text>();
            foreach (var text in texts)
            {
                if (text.name == "VibrationText") vibrationText = text;
                if (text.name == "TemperatureText") temperatureText = text;
                if (text.name == "CurrentText") currentText = text;
                if (text.name == "StatusText") statusText = text;
                if (text.name == "RecommendationText") recommendationText = text;
            }

            Image[] images = spawnedObject.GetComponentsInChildren<Image>();
            foreach (var img in images)
            {
                if (img.name == "StatusIcon") statusIcon = img;
            }
        }
    }

    void Update()
    {
        if (dataReceived && spawnedObject != null && spawnedObject.activeSelf)
        {
            UpdateVisuals();
            dataReceived = false;
        }

        // Animation vibration
        if (currentData != null && spawnedObject != null && currentData.vibration > 0)
        {
            float shake = currentData.vibration * 0.001f;
            spawnedObject.transform.localPosition = initialPosition + UnityEngine.Random.insideUnitSphere * shake;
        }
    }

    void UpdateVisuals()
    {
        // Mise à jour de la couleur du modèle 3D
        if (machineRenderer != null)
        {
            machineRenderer.material.color = currentData.is_anomaly ? anomalyColor : normalColor;
        }

        // Mise à jour des textes UI
        if (vibrationText != null)
            vibrationText.text = $"Vibration: {currentData.vibration:F2} mm/s";

        if (temperatureText != null)
            temperatureText.text = $"Température: {currentData.temperature:F1} °C";

        if (currentText != null)
            currentText.text = $"Courant: {currentData.current:F1} A";

        if (statusText != null)
        {
            statusText.text = currentData.is_anomaly ? "⚠️ ANOMALIE" : "✅ NORMAL";
            statusText.color = currentData.is_anomaly ? Color.red : Color.green;
        }

        if (recommendationText != null)
        {
            recommendationText.text = currentData.recommendation;
            recommendationText.gameObject.SetActive(currentData.is_anomaly);
        }

        if (statusIcon != null)
        {
            statusIcon.color = currentData.is_anomaly ? Color.red : Color.green;
        }

        Debug.Log($"📊 UI Updated - Vib: {currentData.vibration:F2}, Temp: {currentData.temperature:F1}, Anomaly: {currentData.is_anomaly}");
    }

    void OnApplicationQuit()
    {
        if (mqttClient != null && mqttClient.IsConnected)
        {
            mqttClient.Disconnect();
        }
    }

    void OnDestroy()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }
}
