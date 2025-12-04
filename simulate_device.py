import time
import json
import random
import math
import paho.mqtt.client as mqtt

# ==========================================
# ⚙️ CONFIGURATION SIMULATEUR
# ==========================================

MQTT_BROKER = "broker.hivemq.com"
MQTT_PORT = 1883
TOPIC_SENSORS = "pfe/machine01/sensors"

# ==========================================

def simulate():
    client = mqtt.Client()
    
    print(f"⏳ Connexion au simulateur vers {MQTT_BROKER}...")
    try:
        client.connect(MQTT_BROKER, MQTT_PORT, 60)
        print("✅ Connecté !")
    except Exception as e:
        print(f"❌ Erreur de connexion : {e}")
        return

    print("\n🚀 Démarrage de la simulation de machine industrielle...")
    print("ℹ️  Appuyez sur Ctrl+C pour arrêter.\n")
    
    while True:
        try:
            now = time.time()
            
            # --- GENERATION DES DONNEES ---
            
            # 1. Vibration
            # Base sinusoïdale + bruit
            vibration = 1.0 + math.sin(now) * 0.5 + random.uniform(0, 1)
            
            # Injection d'anomalie (10% de chance)
            is_anomaly = False
            if random.random() > 0.90:
                vibration += 5.0 # Pic important
                is_anomaly = True

            # 2. Température
            temperature = 50.0 + random.uniform(-2, 2)

            # 3. Courant
            current = 7.0 + random.uniform(-1, 1)

            # --- ENVOI MQTT ---
            payload = {
                "machine_id": "machine01",
                "vibration": vibration,
                "temperature": temperature,
                "current": current,
                "timestamp": now
            }

            client.publish(TOPIC_SENSORS, json.dumps(payload))
            
            # Affichage console
            status = "🔴 ANOMALIE GENEREE" if is_anomaly else "🟢 Normal"
            print(f"📤 Envoi : Vib={vibration:.2f} | {status}")
            
            time.sleep(2) # Pause de 2 secondes

        except KeyboardInterrupt:
            print("\n🛑 Arrêt de la simulation.")
            break
        except Exception as e:
            print(f"❌ Erreur : {e}")
            time.sleep(1)

if __name__ == "__main__":
    simulate()
