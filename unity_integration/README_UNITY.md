# Intégration Unity 3D - Jumeau Numérique Avancé

Ce dossier contient les scripts nécessaires pour connecter un projet **Unity 3D** à votre plateforme IoT. Cela permet de créer une visualisation industrielle ultra-réaliste (Jumeau Numérique) qui réagit en temps réel aux données capteurs.

---

## 🎯 Résultat Final

Le jumeau numérique Unity affichera :
- ✅ Un modèle 3D de moteur industriel qui **change de couleur** (Vert ➔ Rouge) selon l'état
- ✅ Des **vibrations visuelles** proportionnelles aux données réelles du capteur
- ✅ Des **panneaux UI flottants** affichant température, vibration, statut
- ✅ Une connexion **MQTT temps réel** sans latence perceptible

---

## 🛠️ Prérequis Unity

### Logiciels nécessaires :
1.  **Unity Hub** (dernière version)
2.  **Unity Editor** 2020.3 LTS ou supérieur
3.  **Plugin M2Mqtt** pour la communication MQTT

### Installation du Plugin MQTT :

**Option 1 : Asset Store (Recommandé)**
1. Ouvrez Unity Asset Store dans l'éditeur
2. Recherchez "M2Mqtt" ou "MQTT Client"
3. Importez le package gratuit

**Option 2 : Manuel**
1. Téléchargez `M2Mqtt.dll` depuis [GitHub M2Mqtt](https://github.com/eclipse/paho.mqtt.m2mqtt)
2. Dans Unity, créez un dossier `Assets/Plugins`
3. Glissez le fichier `.dll` dans ce dossier

---

## 🚀 Guide de Configuration Pas à Pas

### Étape 1 : Créer le Projet Unity

1. Ouvrez **Unity Hub** ➔ **New Project**
2. Template : **3D (URP)** ou **3D Core**
3. Nom : `IoT_PredictiveMaintenance`
4. Cliquez sur **Create**

### Étape 2 : Importer les Scripts

1. Dans Unity, localisez le dossier `Assets`
2. Créez un dossier `Assets/Scripts`
3. Copiez les fichiers suivants depuis ce dossier :
   - `DigitalTwinController.cs`
   - `AROverlayController.cs` (optionnel pour AR)

### Étape 3 : Créer la Scène 3D

#### A. Créer le Moteur (Objet Principal)

```
Hierarchy ➔ Clic droit ➔ 3D Object ➔ Cylinder
```

Configuration de l'objet :
- **Nom** : `IndustrialMotor`
- **Position** : X=0, Y=1, Z=0
- **Rotation** : X=0, Y=0, Z=90 (horizontal)
- **Scale** : X=1, Y=3, Z=1

#### B. Ajouter un Matériau

1. `Assets ➔ Clic droit ➔ Create ➔ Material`
2. Nommez-le `MotorMaterial`
3. Couleur : **Vert** (#4CAF50)
4. Glissez ce matériau sur l'objet `IndustrialMotor`

#### C. Créer une Base/Plateforme

```
Hierarchy ➔ 3D Object ➔ Cube
```

- **Nom** : `Platform`
- **Position** : X=0, Y=0, Z=0
- **Scale** : X=5, Y=0.2, Z=3
- **Couleur** : Gris foncé (#333333)

### Étape 4 : Attacher le Script

1. Sélectionnez l'objet `IndustrialMotor` dans la Hierarchy
2. Dans l'**Inspector**, cliquez sur **Add Component**
3. Recherchez `DigitalTwinController` et ajoutez-le

### Étape 5 : Configurer le Script dans l'Inspector

Vous verrez apparaître les champs suivants :

#### 📡 MQTT Configuration
- **Broker Address** : `broker.hivemq.com` (ou `localhost` si local)
- **Broker Port** : `1883`
- **Topic** : `pfe/machine01/analysis`

#### 🎨 Visuals
- **Machine Renderer** : Glissez le `MeshRenderer` de l'objet `IndustrialMotor` ici
  - *(Trouvez-le en cliquant sur le composant MeshRenderer dans l'Inspector)*
- **Normal Color** : Vert (#00FF00)
- **Anomaly Color** : Rouge (#FF0000)
- **Shake Multiplier** : `0.1`

### Étape 6 : Configurer la Caméra

Positionnez la caméra pour bien voir le moteur :

- **Position** : X=0, Y=2, Z=-8
- **Rotation** : X=10, Y=0, Z=0

---

## ▶️ Lancement et Test

### 1. Démarrer le système IoT complet

Sur votre PC, lancez dans 3 terminaux :

```bash
# Terminal 1 : Backend
cd backend_node
npm start

# Terminal 2 : IA Edge
cd edge_computing/inference_service
python main.py

# Terminal 3 : Simulateur
python simulate_device.py
```

### 2. Lancer Unity

1. Dans Unity, cliquez sur le bouton **Play** ▶️
2. Observez la Console Unity : Vous devriez voir `Connected to MQTT Broker!`

### 3. Observer le Comportement

**État Normal (Vibration < 2.0)** :
- ✅ Moteur **vert**
- ✅ Rotation fluide
- ✅ Pas de vibration visible

**État Anomalie (Vibration > 5.0)** :
- 🔴 Moteur devient **rouge** instantanément
- 🔴 L'objet **tremble** de manière visible
- 🔴 Console affiche : `Anomaly detected!`

---

## 🎨 Captures d'Écran Attendues

### Vue Unity Editor (Mode Edition)

```
┌─────────────────────────────────────────────────────┐
│  Hierarchy          │    Scene View    │ Inspector │
│  ─────────         │                  │───────────│
│  ▸ Main Camera      │   [Moteur 3D]    │ DigitalTwin│
│  ▸ Directional Light│   sur plateforme │Controller │
│  ▸ IndustrialMotor │                  │           │
│  ▸ Platform         │                  │ Broker:   │
│                     │                  │ hivemq.com│
└─────────────────────────────────────────────────────┘
```

### Vue Play Mode (Normal)

```
Écran 3D : Moteur VERT qui tourne lentement
Console : "Connected to MQTT Broker!"
          "Received: Normal (Vibration: 1.2)"
```

### Vue Play Mode (Anomalie)

```
Écran 3D : Moteur ROUGE qui vibre rapidement
Console : "Received: Anomaly (Vibration: 6.5)"
          "⚠️ Machine needs inspection!"
```

---

## 💡 Améliorations Possibles (Pour Aller Plus Loin)

### 1. Ajouter une UI WorldSpace

Créez un Canvas en mode **World Space** au-dessus du moteur :

```
Hierarchy ➔ UI ➔ Canvas
```

Configurez :
- **Render Mode** : World Space
- **Position** : X=0, Y=3, Z=0

Ajoutez des **Text** ou **TextMeshPro** pour afficher :
- Vibration actuelle
- Température
- Statut (OK / ALERT)

### 2. Mode Réalité Augmentée (AR)

Utilisez **AR Foundation** de Unity :

1. Installez les packages Unity AR
2. Remplacez la scène par une AR Session
3. Le moteur apparaîtra sur une **surface détectée** (table, sol)
4. Utilisez le script `AROverlayController.cs` fourni

### 3. Exporter vers Mobile/WebGL

- **Android** : File ➔ Build Settings ➔ Android ➔ Build
- **WebGL** : Pour une version navigateur embarquée dans votre site

---

## 🐛 Dépannage

| Problème | Solution |
|----------|----------|
| Erreur `M2Mqtt not found` | Vérifiez que le .dll est dans `Assets/Plugins` |
| `Connection refused` | Vérifiez que le broker MQTT est accessible (broker.hivemq.com) |
| Le moteur ne change pas de couleur | Vérifiez que `Machine Renderer` est bien assigné dans l'Inspector |
| Pas de vibration visible | Augmentez `Shake Multiplier` à 0.5 |

---

## 📚 Ressources Supplémentaires
