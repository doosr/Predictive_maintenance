#include <WiFi.h>
#include <PubSubClient.h>
#include <ArduinoJson.h>

// ==========================================
// ⚙️ CONFIGURATION
// ==========================================

// 📶 WiFi
const char* ssid = "VOTRE_WIFI_SSID";
const char* password = "VOTRE_WIFI_PASSWORD";

// ☁️ MQTT Broker
// Option 1 : Public (Pour test rapide)
const char* mqtt_server = "broker.hivemq.com";
const int mqtt_port = 1883;

// Option 2 : Local (Raspberry Pi / PC)
// const char* mqtt_server = "192.168.1.XX"; 
// const int mqtt_port = 1883;

// 📢 Topics
const char* topic_sensors = "pfe/machine01/sensors";

// ==========================================

WiFiClient espClient;
PubSubClient client(espClient);

// Variables de simulation
float vibration = 0.0;
float temperature = 45.0;
float current = 5.0;
unsigned long lastMsg = 0;

void setup_wifi() {
  delay(10);
  Serial.println();
  Serial.print("Connexion au WiFi : ");
  Serial.println(ssid);

  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("");
  Serial.println("WiFi connecté !");
  Serial.print("Adresse IP : ");
  Serial.println(WiFi.localIP());
}

void reconnect() {
  while (!client.connected()) {
    Serial.print("Connexion au Broker MQTT...");
    String clientId = "ESP32Client-";
    clientId += String(random(0xffff), HEX);
    
    if (client.connect(clientId.c_str())) {
      Serial.println("Connecté !");
    } else {
      Serial.print("Echec, rc=");
      Serial.print(client.state());
      Serial.println(" nouvelle tentative dans 5s");
      delay(5000);
    }
  }
}

void setup() {
  Serial.begin(115200);
  setup_wifi();
  client.setServer(mqtt_server, mqtt_port);
}

void loop() {
  if (!client.connected()) {
    reconnect();
  }
  client.loop();

  unsigned long now = millis();
  if (now - lastMsg > 2000) { // Envoi toutes les 2 secondes
    lastMsg = now;

    // --- SIMULATION DES CAPTEURS ---
    
    // 1. Vibration (Simule un moteur)
    // Normal : < 2.0 | Anomalie : > 5.0
    vibration = 1.0 + sin(now / 1000.0) * 0.5 + (random(0, 100) / 100.0);
    
    // Injection aléatoire d'anomalie (5% de chance)
    if (random(0, 100) > 95) {
      vibration += 5.0; 
      Serial.println("⚠️ SIMULATION : Anomalie de Vibration générée !");
    }

    // 2. Température (Normal ~50°C)
    temperature = 50.0 + (random(-20, 20) / 10.0);

    // 3. Courant (Normal ~7A)
    current = 7.0 + (random(-10, 10) / 10.0);

    // --- CREATION DU JSON ---
    StaticJsonDocument<256> doc;
    doc["machine_id"] = "machine01";
    doc["vibration"] = vibration;
    doc["temperature"] = temperature;
    doc["current"] = current;
    doc["timestamp"] = now;

    char buffer[512];
    serializeJson(doc, buffer);

    // --- PUBLICATION MQTT ---
    Serial.print("Envoi : ");
    Serial.println(buffer);
    client.publish(topic_sensors, buffer);
  }
}
