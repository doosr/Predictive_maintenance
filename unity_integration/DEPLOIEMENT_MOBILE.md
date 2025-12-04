# 📱 Guide de Déploiement Mobile - Application AR

Ce guide vous accompagne pour installer l'application de Réalité Augmentée sur votre **téléphone Android**.

---

## 🎯 Ce que vous allez installer

Une application mobile qui :
- Scanne une image cible avec la caméra
- Affiche un jumeau numérique 3D en Réalité Augmentée
- Se connecte en temps réel à votre système IoT
- Affiche vibration, température, courant en direct
- Alerte si anomalie détectée

---

## 📋 Prérequis

### Sur votre PC :
- ✅ Unity Hub installé
- ✅ Unity Editor 2020.3 LTS ou supérieur
- ✅ Android SDK (s'installe avec Unity)

### Sur votre téléphone :
- ✅ Android 7.0 (API 24) ou supérieur
- ✅ Supporte ARCore (la plupart des téléphones récents)
- ✅ Mode Développeur activé
- ✅ Débogage USB activé

---

## 🔧 Étape 1 : Activer le Mode Développeur (Téléphone)

### Sur Android :

1. **Paramètres** → **À propos du téléphone**
2. Tapez **7 fois** sur "Numéro de build"
3. Un message apparaît : "Vous êtes maintenant développeur"
4. Retour → **Options pour les développeurs**
5. Activez **Débogage USB**

---

## 🖼️ Étape 2 : Créer l'Image Cible à Scanner

### A. Télécharger l'image prête à l'emploi

Je vous fournis une image cible optimisée pour AR :

**Caractéristiques :**
- Format : 1024x1024px
- Haute résolution
- Contraste élevé
- Motifs distincts

### B. Créer votre propre image (optionnel)

Si vous voulez personnaliser :

1. Ouvrez **Canva** ou **PowerPoint**
2. Créez un design carré (1024x1024px)
3. Ajoutez :
   - Texte : "IoT Maintenance Prédictive"
   - Logo : Engrenage ou machine
   - Fond : Dégradé bleu/vert
4. Exportez en PNG : `target_image.png`

### C. Imprimer l'image

**Option 1 : Impression**
- Imprimez sur papier A4
- Taille réelle : **20 cm** de largeur (important!)
- Qualité : Haute résolution

**Option 2 : Affichage sur écran**
- Affichez l'image sur un écran de PC ou tablette
- Mode plein écran
- Luminosité maximale

---

## 🎮 Étape 3 : Configurer Unity pour Android

### A. Installer les Modules Android

1. Ouvrez **Unity Hub**
2. Onglet **Installs**
3. Cliquez sur l'icône ⚙️ de votre version Unity
4. **Add Modules**
5. Cochez :
   - ✅ **Android Build Support**
   - ✅ Android SDK & NDK Tools
   - ✅ OpenJDK
6. Cliquez sur **Install**

### B. Ouvrir le Projet

1. Unity Hub → **Projects**
2. **Open** → Naviguez vers votre projet Unity AR
3. Ouvrez le projet

---

## ⚙️ Étape 4 : Configuration du Build

### A. Build Settings

1. Dans Unity : **File** → **Build Settings**
2. Sélectionnez **Android**
3. Cliquez sur **Switch Platform** (si pas déjà Android)
4. Attendez la fin du processus (peut prendre quelques minutes)

### B. Player Settings

Cliquez sur **Player Settings** :

#### 📱 Onglet "Player"

**Company Name** : Votre nom  
**Product Name** : `IoT Predictive AR`

**Other Settings :**
- **Package Name** : `com.votrenom.iotpredictivear` (tout en minuscules, pas d'espaces)
- **Minimum API Level** : **Android 7.0 'Nougat' (API level 24)**
- **Target API Level** : **Automatic (highest installed)**

**Graphics :**
- Cliquez sur le **"-"** à côté de "Vulkan" pour le retirer
- Gardez uniquement **OpenGLES3**

#### 🔌 Onglet "XR Plug-in Management"

1. Cliquez sur l'icône **Android** (petit robot)
2. Cochez : ✅ **ARCore**

---

## 🏗️ Étape 5 : Build de l'APK

### A. Vérifier la scène

Dans Build Settings, vérifiez que votre scène AR est cochée :
- ✅ Scenes/ARScene (ou le nom de votre scène)

Si elle n'est pas listée :
- Cliquez sur **Add Open Scenes**

### B. Lancer le Build

1. Cliquez sur **Build**
2. Créez un dossier : `Builds/Android/`
3. Nom du fichier : `IoT_Predictive_AR.apk`
4. Cliquez sur **Enregistrer**

⏳ **Attendez** (5-15 minutes selon votre PC)

✅ Une fois terminé, vous avez votre APK !

---

## 📲 Étape 6 : Installer sur Téléphone

### Option 1 : Installation USB (Recommandé)

1. **Connectez** votre téléphone au PC via USB
2. Sur le téléphone, acceptez le **Débogage USB**

3. **Vérifier la connexion** :
   ```powershell
   # Dans un terminal PowerShell
   cd "C:\Users\VOTRE_NOM\AppData\Local\Android\Sdk\platform-tools"
   .\adb devices
   ```
   Vous devriez voir votre téléphone listé.

4. **Installer l'APK** :
   ```powershell
   .\adb install "C:\CHEMIN_VERS\Builds\Android\IoT_Predictive_AR.apk"
   ```

### Option 2 : Installation directe

1. Copiez le fichier `IoT_Predictive_AR.apk` sur votre téléphone (via USB ou OneDrive)
2. Sur le téléphone, ouvrez l'**Explorateur de fichiers**
3. Naviguez vers l'APK
4. Tapez dessus pour l'installer
5. Acceptez l'installation depuis des sources inconnues (si demandé)

---

## 🚀 Étape 7 : Lancer l'Application

### A. Démarrer le système IoT sur PC

Dans 3 terminaux :

**Terminal 1 : Backend**
```bash
cd backend_node
npm start
```

**Terminal 2 : IA Edge**
```bash
cd edge_computing/inference_service
python main.py
```

**Terminal 3 : Simulateur**
```bash
python simulate_device.py
```

### B. Connecter le téléphone au même réseau WiFi

⚠️ **Important** : Le téléphone doit être sur le **même WiFi** que votre PC.

### C. Lancer l'Application AR

1. Sur le téléphone, cherchez l'icône **IoT Predictive AR**
2. Tapez pour ouvrir
3. Acceptez les permissions caméra (si demandé)

### D. Scanner l'Image

1. **Pointez la caméra** vers l'image imprimée
2. **Maintenez le téléphone stable** à 30-50 cm
3. Le moteur 3D devrait **apparaître** au-dessus de l'image !
4. Les données s'affichent en temps réel

---

## 🎬 Démo pour la Soutenance

### Script de présentation (1 minute) :

**"Bonjour, je vais vous présenter l'application mobile de Réalité Augmentée que j'ai développée."**

1. 📱 Montrez le téléphone avec l'app ouverte
2. 🖼️ "Voici l'image cible représentant notre machine"
3. 📸 "Je scanne cette image avec la caméra..."
4. ✨ *Le modèle 3D apparaît*
5. 📊 "Le jumeau numérique affiche toutes les données en temps réel :
   - Vibration
   - Température
   - Courant électrique"
6. 🔴 *Attendre qu'une anomalie se produise*
7. ⚠️ "Lorsque l'IA détecte une anomalie, le système change de couleur et m'alerte avec une recommandation technique précise"
8. 💡 Montrez la recommandation IA

---

## 📹 Conseils pour Filmer la Démo

Si vous voulez faire une vidéo pour le mémoire :

1. **Caméra fixe** filmant le téléphone
2. **Éclairage correct** (pas de contre-jour)
3. **Image cible bien visible** dans le cadre
4. **Mains stables** pour un rendu propre

Utilisez **AZ Screen Recorder** (app Android) pour enregistrer l'écran du téléphone directement.

---

## 🐛 Dépannage

### L'APK ne s'installe pas
- Vérifiez que le téléphone autorise les sources inconnues
- Désinstallez l'ancienne version si elle existe

### "Aucune image détectée"
- Vérifiez que l'image imprimée fait bien **20 cm**
- Améliorez l'éclairage
- Assurez-vous que l'image est bien à plat

### Pas de données affichées
- Vérifiez que le téléphone est sur le même WiFi
- Vérifiez que le Backend/IA/Simulateur tournent sur le PC
- Dans le code Unity, changez `broker.hivemq.com` par l'IP locale de votre PC

### Application crash
- Vérifiez que ARCore est supporté par votre téléphone
- Testez avec : **Google Play Services for AR** (app à installer)

---

## 📊 Checklist Finale

Avant la soutenance, vérifiez :

- [ ] APK installé sur téléphone
- [ ] Image cible imprimée (20 cm)
- [ ] Système IoT testé et fonctionnel
- [ ] Téléphone et PC sur même WiFi
- [ ] Application AR testée au moins 1 fois
- [ ] Vidéo de démo enregistrée (backup)
- [ ] Batterie téléphone chargée à 100%

---

## 🎓 Pour aller plus loin

### Multi-Machine AR
- Créez plusieurs images cibles
- Chaque image = une machine différente
- Scannez différentes images pour voir différentes machines

### Export iOS
- Mêmes étapes mais avec Xcode
- Platform : iOS au lieu d'Android
- ARKit au lieu d'ARCore

Félicitations ! Vous avez une application mobile professionnelle ! 🚀
