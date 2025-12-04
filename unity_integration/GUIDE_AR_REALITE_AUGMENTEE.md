# 📱 Guide Réalité Augmentée (AR) - Scan d'Image

Ce guide vous explique comment créer une application AR où vous scannez une **image cible** avec votre téléphone, et Unity affiche le jumeau numérique 3D + toutes les données IoT en temps réel.

---

## 🎯 Résultat Final

Lorsque vous pointez votre téléphone vers **l'image cible** :
1. ✅ Un moteur 3D apparaît au-dessus de l'image
2. ✅ Des panneaux UI flottants affichent :
   - Vibration en temps réel
   - Température
   - Courant électrique
   - Statut (NORMAL / ANOMALIE)
   - Recommandation de l'IA
3. ✅ Le moteur change de couleur (Vert/Rouge) en temps réel
4. ✅ Le moteur vibre si anomalie détectée

---

## 📋 Prérequis

### Packages Unity à installer :

1. **AR Foundation** (Unity's official AR framework)
   - Window → Package Manager → Unity Registry
   - Cherchez "AR Foundation" → Install

2. **ARCore XR Plugin** (pour Android) OU **ARKit XR Plugin** (pour iOS)
   - Dans le même Package Manager
   - Installez celui correspondant à votre plateforme cible

3. **M2Mqtt** (déjà installé normalement)

---

## 🖼️ Étape 1 : Créer votre Image Cible

### A. Préparer l'Image

Vous avez besoin d'une image à scanner. Voici les recommandations :

**Caractéristiques idéales :**
- ✅ Contraste élevé (pas d'image floue ou fade)
- ✅ Pas trop de zones uniformes
- ✅ Idéalement 1024x1024 pixels minimum
- ✅ Format : PNG ou JPG

**Exemples d'images qui fonctionnent bien :**
- Logo de votre université
- QR Code personnalisé
- Affiche avec des motifs distincts
- Carte de visite

**Pour ce projet, je vous suggère :**

Créez une image simple avec Canva ou PowerPoint :
- Fond : Dégradé bleu
- Texte central : "IoT Maintenance 4.0"
- Logo/Icône : Engrenage ou machine
- Taille : 1024x1024px

Sauvegardez-la sous le nom : `target_image.png`

### B. Importer l'Image dans Unity

1. Glissez `target_image.png` dans `Assets/`
2. Sélectionnez l'image dans Unity
3. Dans l'Inspector :
   - **Texture Type** : `Sprite (2D and UI)`
   - **Read/Write Enabled** : ✅ Activé
   - Cliquez sur **Apply**

---

## ⚙️ Étape 2 : Créer la Reference Image Library

1. Dans Assets, faites un clic droit : `Create → XR → Reference Image Library`
2. Nommez-la : `IoT_ImageLibrary`
3. Sélectionnez cette librairie
4. Dans l'Inspector, cliquez sur **Add Image**
5. Configurez :
   - **Name** : `target_iot`
   - **Texture** : Glissez votre `target_image.png`
   - **Specify Size** : ✅ Activé
   - **Physical Size** : 0.2 (20 cm, taille réelle de l'image imprimée)

---

## 🎨 Étape 3 : Créer le Prefab AR (Modèle 3D + UI)

### A. Créer le GameObject parent

```
Hierarchy → Create Empty → Nommer "AR_MachineContent"
```

### B. Ajouter le Modèle 3D

Sous `AR_MachineContent` :
```
Create → 3D Object → Cylinder
```

Configuration :
- **Nom** : `MotorModel`
- **Position** : (0, 0.1, 0)
- **Rotation** : (0, 0, 90)
- **Scale** : (0.05, 0.15, 0.05)
- **Matériau** : Vert (#4CAF50)

### C. Créer l'UI World Space

Sous `AR_MachineContent` :
```
UI → Canvas
```

Configuration du Canvas :
- **Render Mode** : World Space
- **Position** : (0, 0.15, 0)
- **Scale** : (0.001, 0.001, 0.001)
- **Width** : 400
- **Height** : 600

### D. Ajouter les éléments UI

Sous le Canvas, créez (UI → Text) :

1. **VibrationText**
   - Position : (0, 100, 0)
   - Text : "Vibration: 0.00 mm/s"
   - Font Size : 24
   - Color : White
   - Anchor : Middle Center

2. **TemperatureText**
   - Position : (0, 50, 0)
   - Text : "Température: 0.0 °C"
   - Font Size : 24

3. **CurrentText**
   - Position : (0, 0, 0)
   - Text : "Courant: 0.0 A"
   - Font Size : 24

4. **StatusText**
   - Position : (0, -50, 0)
   - Text : "✅ NORMAL"
   - Font Size : 28
   - Font Style : Bold
   - Color : Green

5. **RecommendationText**
   - Position : (0, -120, 0)
   - Text : "Recommandation IA..."
   - Font Size : 20
   - Color : Yellow
   - Active : Désactivé par défaut

### E. Créer le Prefab

1. Glissez `AR_MachineContent` de la Hierarchy vers le dossier `Assets/`
2. Cela crée un Prefab
3. Supprimez `AR_MachineContent` de la Hierarchy (il sera spawné dynamiquement)

---

## 🎮 Étape 4 : Configurer la Scène AR

### A. Supprimer la Main Camera classique

Sélectionnez la `Main Camera` → Delete

### B. Créer le Setup AR

1. Hierarchy → Create Empty → Nommer "AR Session Origin"
2. Add Component → **AR Session Origin**
3. Add Component → **AR Tracked Image Manager**

Configuration de **AR Tracked Image Manager** :
- **Serialized Library** : Glissez `IoT_ImageLibrary`
- **Max Number of Moving Images** : 1

### C. Ajouter la caméra AR

Sous `AR Session Origin` :
```
Hierarchy → Create → Camera
```

Configuration :
- **Nom** : `AR Camera`
- **Tag** : MainCamera
- **Position** : (0, 0, 0)
- Add Component → **AR Camera Manager**
- Add Component → **AR Camera Background**

### D. Créer l'AR Session

```
Hierarchy → Create Empty → Nommer "AR Session"
```

- Add Component → **AR Session**

---

## 🔌 Étape 5 : Attacher le Script

1. Sélectionnez `AR Session Origin`
2. Add Component → Cherchez `ARPredictiveMaintenanceController`
3. Configurez dans l'Inspector :

**AR Configuration**
- **Tracked Image Manager** : Glissez le composant `AR Tracked Image Manager` (même objet)
- **AR Content Prefab** : Glissez le prefab `AR_MachineContent` depuis Assets

**MQTT Configuration**
- **Broker Address** : `broker.hivemq.com`
- **Broker Port** : `1883`
- **Topic** : `pfe/machine01/analysis`

**3D Model**
- (Sera rempli automatiquement au runtime)

**UI Elements**
- (Sera rempli automatiquement au runtime)

---

## 📱 Étape 6 : Build et Test sur Mobile

### Configuration du Build

1. File → Build Settings
2. Sélectionnez **Android** (ou iOS)
3. Switch Platform
4. Player Settings :
   - **Company Name** : Votre nom
   - **Product Name** : IoT Predictive AR
   - **Minimum API Level** : Android 7.0 (API 24)
   - **Graphics APIs** : Enlevez Vulkan, gardez OpenGLES3

5. XR Plug-in Management :
   - ✅ ARCore (Android) OU ARKit (iOS)

6. Cliquez sur **Build** → Créez un dossier `Builds/` → Générez l'APK

### Installation

1. Connectez votre téléphone Android en USB
2. Activez le **Mode Développeur** sur le téléphone
3. Installez l'APK : `adb install IoT_Predictive_AR.apk`

---

## 🎬 Test de l'Application

### 1. Préparer l'image cible

- Imprimez `target_image.png` sur papier A4 **OU**
- Affichez l'image sur un écran d'ordinateur/tablette

### 2. Lancer l'application sur le téléphone

### 3. Démarrer le système IoT

Sur votre PC :
```bash
# Terminal 1
cd backend_node && npm start

# Terminal 2
cd edge_computing/inference_service && python main.py

# Terminal 3
python simulate_device.py
```

### 4. Scanner l'image

- Pointez la caméra du téléphone vers l'image imprimée
- **Le moteur 3D devrait apparaître instantanément** au-dessus de l'image !
- Les données s'affichent en temps réel
- Si une anomalie est détectée, le moteur devient rouge et vibre

---

## 🎥 Captures d'Écran Attendues

### Vue 1 : Scan Initial
```
┌─────────────────────────────────┐
│   [Caméra du téléphone active]  │
│                                 │
│   Pointez vers l'image cible    │
│                                 │
│   [Image cible visible]         │
│                                 │
└─────────────────────────────────┘
```

### Vue 2 : AR Activée (État Normal)
```
┌─────────────────────────────────┐
│   [Moteur 3D VERT flottant]    │
│                                 │
│   ┌─────────────────────────┐  │
│   │ Vibration: 1.2 mm/s     │  │
│   │ Température: 50.5 °C    │  │
│   │ Courant: 7.0 A          │  │
│   │ ✅ NORMAL               │  │
│   └─────────────────────────┘  │
│                                 │
└─────────────────────────────────┘
```

### Vue 3 : Anomalie Détectée
```
┌─────────────────────────────────┐
│   [Moteur 3D ROUGE qui vibre]  │
│                                 │
│   ┌─────────────────────────┐  │
│   │ Vibration: 6.8 mm/s     │  │
│   │ Température: 52.0 °C    │  │
│   │ Courant: 7.5 A          │  │
│   │ ⚠️ ANOMALIE             │  │
│   │                         │  │
│   │ 💡 Vérifier alignement  │  │
│   │    de l'arbre           │  │
│   └─────────────────────────┘  │
└─────────────────────────────────┘
```

---

## 🐛 Dépannage

| Problème | Solution |
|----------|----------|
| L'image n'est pas détectée | Vérifiez que l'image imprimée fait bien 20cm de largeur |
| Pas de connexion MQTT | Vérifiez que le téléphone est sur le même réseau WiFi |
| Le modèle 3D ne s'affiche pas | Vérifiez que le prefab est bien assigné |
| UI invisible | Vérifiez l'échelle du Canvas (0.001) |
| Application crash au démarrage | Vérifiez que ARCore est bien activé dans XR Settings |

---

## 📹 Démonstration pour la Soutenance

### Script de démo (30 secondes) :

1. **"Voici une application mobile de Réalité Augmentée que j'ai développée"**
2. *Montrer le téléphone avec l'app ouverte*
3. **"Je scanne cette image..."** *Pointer vers l'image imprimée*
4. **"Et le jumeau numérique apparaît avec toutes les données en temps réel"**
5. *Montrer les valeurs qui changent*
6. **"Lorsque l'IA détecte une anomalie, le système m'alerte instantanément"**
7. *Montrer le changement de couleur et la recommandation*

---

## 🎯 Pour aller encore plus loin

- Ajouter des **animations** au modèle 3D (rotation des pièces)
- Implémenter un **bouton AR** pour déclencher une maintenance
- Enregistrer une **vidéo AR** pour documentation
- Multi-images : Scanner différentes machines (chacune avec sa propre image cible)

Vous avez maintenant une application AR complète de niveau industriel ! 🚀
