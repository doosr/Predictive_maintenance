const express = require('express');
const http = require('http');
const socketIo = require('socket.io');
const mqtt = require('mqtt');
const cors = require('cors');
const path = require('path');

// ==========================================
// ⚙️ CONFIGURATION
// ==========================================

const MQTT_BROKER = 'mqtt://broker.hivemq.com:1883';
const TOPIC_ANALYSIS = 'pfe/machine01/analysis';
const PORT = 3000;

// ==========================================

const app = express();
app.use(cors());
app.use(express.static(path.join(__dirname, 'public'))); // Serve frontend

const server = http.createServer(app);
const io = socketIo(server);

// Stockage en mémoire pour l'historique récent (pour les nouveaux connectés)
let alertHistory = [];
const MAX_HISTORY = 50;

console.log('⏳ Démarrage du Backend IoT Pro...');

// --- MQTT SETUP ---
const client = mqtt.connect(MQTT_BROKER);

client.on('connect', () => {
    console.log(`✅ Connecté au Broker MQTT : ${MQTT_BROKER}`);
    client.subscribe(TOPIC_ANALYSIS);
});

client.on('message', (topic, message) => {
    if (topic === TOPIC_ANALYSIS) {
        try {
            const data = JSON.parse(message.toString());

            // Ajouter timestamp lisible
            data.receivedAt = new Date().toLocaleTimeString();

            // 🧠 IA Générative : Recommandation textuelle
            let recommendation = "Aucune action requise.";
            if (data.is_anomaly) {
                if (data.vibration > 5) recommendation = "⚠️ URGENCE : Vérifier l'alignement de l'arbre et les roulements.";
                if (data.temperature > 60) recommendation = "🔥 ALERTE : Surchauffe détectée. Vérifier le système de refroidissement.";
                if (data.current > 10) recommendation = "⚡ DANGER : Surcharge électrique. Vérifier l'alimentation.";
            }
            data.recommendation = recommendation;

            // Log console
            if (data.is_anomaly) {
                console.log(`🔴 ANOMALIE DETECTEE sur ${data.machine_id} (Confiance: ${(data.confidence * 100).toFixed(1)}%)`);
                console.log(`💡 Conseil IA : ${recommendation}`);

                // Ajouter à l'historique
                alertHistory.unshift(data);
                if (alertHistory.length > MAX_HISTORY) alertHistory.pop();
            }

            // 🚀 Envoyer au Frontend via WebSocket
            io.emit('machine_update', data);

        } catch (e) {
            console.error('❌ Erreur parsing JSON:', e);
        }
    }
});

// --- SOCKET.IO SETUP ---
io.on('connection', (socket) => {
    console.log('👤 Nouveau client Dashboard connecté');

    // Envoyer l'historique des alertes à la connexion
    socket.emit('alert_history', alertHistory);

    socket.on('disconnect', () => {
        console.log('👤 Client déconnecté');
    });
});

// --- SERVER START ---
server.listen(PORT, () => {
    console.log(`\n🚀 SERVEUR WEB LANCE !`);
    console.log(`👉 Accédez au Dashboard ici : http://localhost:${PORT}`);
    console.log(`---------------------------------------------------\n`);
});
