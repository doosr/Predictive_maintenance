import json
import joblib
import paho.mqtt.client as mqtt
import numpy as np
import time
import os
from influxdb_client import InfluxDBClient, Point
from influxdb_client.client.write_api import SYNCHRONOUS

# ==========================================
# ⚙️ CONFIGURATION
# ==========================================

# ☁️ MQTT
MQTT_BROKER = "broker.hivemq.com" # Ou "localhost"
MQTT_PORT = 1883
TOPIC_SENSORS = "pfe/machine01/sensors"
TOPIC_ANALYSIS = "pfe/machine01/analysis"

# 🗄️ InfluxDB (Optionnel si Docker ne marche pas)
ENABLE_INFLUXDB = False # Mettre à True si InfluxDB est installé
INFLUX_URL = "http://localhost:8086"
INFLUX_TOKEN = "my-super-secret-auth-token"
INFLUX_ORG = "myorg"
INFLUX_BUCKET = "iot_bucket"

# 🧠 Modèle IA
MODEL_PATH = "../model_training/anomaly_detector.pkl"

# ==========================================

# Chargement du Modèle
print(f"🔄 Chargement du modèle depuis {MODEL_PATH}...")
try:
    if os.path.exists(MODEL_PATH):
        model = joblib.load(MODEL_PATH)
        print("✅ Modèle IA chargé avec succès.")
    else:
        print("❌ ERREUR : Fichier modèle introuvable. Veuillez lancer train_model.py d'abord.")
        model = None
except Exception as e:
    print(f"❌ ERREUR lors du chargement du modèle : {e}")
    model = None

# Configuration InfluxDB
write_api = None
if ENABLE_INFLUXDB:
    try:
        client_influx = InfluxDBClient(url=INFLUX_URL, token=INFLUX_TOKEN, org=INFLUX_ORG)
        write_api = client_influx.write_api(write_options=SYNCHRONOUS)
        print("✅ Connexion InfluxDB configurée.")
    except Exception as e:
        print(f"⚠️ Attention : Impossible de configurer InfluxDB ({e})")

def on_connect(client, userdata, flags, rc):
    if rc == 0:
        print(f"✅ Connecté au Broker MQTT ({MQTT_BROKER})")
        client.subscribe(TOPIC_SENSORS)
        print(f"📡 Abonné au topic : {TOPIC_SENSORS}")
    else:
        print(f"❌ Echec connexion MQTT, code : {rc}")

def on_message(client, userdata, msg):
    try:
        payload = msg.payload.decode()
        data = json.loads(payload)
        
        # Extraction des features
        vibration = float(data.get('vibration', 0))
        temperature = float(data.get('temperature', 0))
        current = float(data.get('current', 0))
        machine_id = data.get('machine_id', 'unknown')
        timestamp = data.get('timestamp', time.time())

        print(f"📥 Reçu [{machine_id}] : Vib={vibration:.2f}, Temp={temperature:.2f}, Curr={current:.2f}")

        # --- INFERENCE IA ---
        is_anomaly = False
        confidence = 0.0
        
        if model:
            # Préparation du vecteur (doit correspondre à l'entraînement)
            features = np.array([[vibration, temperature, current]])
            
            # Prédiction
            prediction = model.predict(features)[0]
            probs = model.predict_proba(features)[0]
            
            is_anomaly = bool(prediction == 1)
            confidence = float(probs[1] if is_anomaly else probs[0])
            
            status_str = "🔴 ANOMALIE" if is_anomaly else "🟢 NORMAL"
            print(f"🧠 Analyse : {status_str} (Confiance: {confidence*100:.1f}%)")

            # --- PUBLICATION RESULTAT ---
            analysis_payload = {
                "machine_id": machine_id,
                "is_anomaly": is_anomaly,
                "confidence": confidence,
                "vibration": vibration, # On renvoie les données pour le dashboard
                "timestamp": timestamp
            }
            client.publish(TOPIC_ANALYSIS, json.dumps(analysis_payload))
        
        # --- STOCKAGE INFLUXDB ---
        if write_api:
            try:
                point = Point("machine_health") \
                    .tag("machine_id", machine_id) \
                    .field("vibration", vibration) \
                    .field("temperature", temperature) \
                    .field("current", current) \
                    .field("is_anomaly", int(is_anomaly)) \
                    .field("confidence", confidence)
                write_api.write(bucket=INFLUX_BUCKET, org=INFLUX_ORG, record=point)
            except Exception as e:
                print(f"⚠️ Erreur écriture InfluxDB : {e}")

    except Exception as e:
        print(f"❌ Erreur de traitement : {e}")

# Lancement du client MQTT
client = mqtt.Client()
client.on_connect = on_connect
client.on_message = on_message

print("⏳ Connexion au MQTT...")
try:
    client.connect(MQTT_BROKER, MQTT_PORT, 60)
    client.loop_forever()
except Exception as e:
    print(f"❌ Impossible de se connecter au broker MQTT : {e}")
