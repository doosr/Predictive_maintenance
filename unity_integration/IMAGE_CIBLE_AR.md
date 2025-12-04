# 🎯 Image Cible pour AR - Instructions

Cette image est conçue pour être scannée par l'application AR de maintenance prédictive.

## 📋 Comment l'utiliser

### Option 1 : Impression (Recommandé)

1. **Téléchargez** cette image : `AR_Target_Image.png`
2. **Imprimez** sur papier blanc A4
3. **Mesures importantes** :
   - Largeur de l'image : **20 cm** exactement
   - Ajustez lors de l'impression si nécessaire
4. **Qualité** : Mode "Haute qualité" ou "Photo"

### Option 2 : Affichage sur écran

1. Ouvrez `AR_Target_Image.png` sur un écran (PC, tablette)
2. Mode **plein écran**
3. **Luminosité maximale**
4. Taille à l'écran : environ 20 cm

---

## ✅ Vérification

Une fois imprimée/affichée, l'image devrait :
- Avoir un **contraste élevé**
- Être **nette** (pas floue)
- Avoir des **motifs distincts** reconnaissables
- Mesurer environ **20 cm de largeur**

---

## 🎨 Créer votre propre image cible

Si vous voulez personnaliser l'image :

### Recommandations :

✅ **À FAIRE** :
- Utiliser des couleurs contrastées
- Inclure du texte ou des icônes
- Format : 1024x1024px minimum
- Éviter les zones uniformes

❌ **À ÉVITER** :
- Images floues ou basse résolution
- Couleurs trop similaires
- Zones entièrement blanches ou noires
- Motifs trop répétitifs

### Outils suggérés :

- **Canva** (gratuit, en ligne)
- **PowerPoint** (avec export PNG haute qualité)
- **Photoshop** / **GIMP**

### Template suggéré :

```
┌─────────────────────────────────┐
│                                 │
│    [Logo / Icône Engrenage]    │
│                                 │
│   IoT MAINTENANCE PRÉDICTIVE    │
│                                 │
│    [Code QR ou Pattern]        │
│                                 │
│         INDUSTRIE 4.0          │
│                                 │
└─────────────────────────────────┘
```

Fond : Dégradé bleu (#0066CC → #00CCFF)  
Texte : Blanc et gras  
Icônes : Jaune/Orange pour contraste

---

## 🔧 Configuration dans Unity

Après avoir créé/choisi votre image :

1. Importez l'image dans Unity (`Assets/`)
2. Inspector → **Texture Type** : `Sprite (2D and UI)`
3. ✅ Cochez **Read/Write Enabled**
4. Cliquez sur **Apply**
5. Ajoutez-la à la **Reference Image Library**
6. **Physical Size** : `0.2` (= 20 cm)

---

## 🎬 Test de l'Image

Pour vérifier que votre image fonctionne bien :

1. Lancez l'app AR sur téléphone
2. Pointez vers l'image
3. **Résultat attendu** :
   - ✅ Détection en < 2 secondes
   - ✅ Modèle 3D stable et bien positionné
   - ✅ Pas de "jitter" (tremblements)

Si la détection est lente ou instable :
- Améliorez l'éclairage
- Augmentez le contraste de l'image
- Imprimez en meilleure qualité

---

## 📸 Exemples de bonnes images cibles

### Type 1 : Logo Entreprise
- Logo coloré sur fond uni
- Texte "Maintenance IoT"
- QR Code en coin

### Type 2 : Motif Technique
- Schéma de machine
- Grille + texte
- Icônes industrielles

### Type 3 : Badge / Carte
- Design type carte de visite
- Informations projet
- Pattern géométrique

---

## 💡 Astuce Pro

Pour une présentation impressionnante :

1. Créez un **poster A3** avec :
   - L'image cible au centre
   - Informations sur le projet autour
   - Logo de l'université
   
2. Lors de la démo :
   - Montrez le poster accroché au mur
   - Scannez avec le téléphone
   - Le modèle 3D apparaît !

Cela donne un effet très professionnel ! 🎓

---

## 📞 Support

Si l'image ne fonctionne pas :
- Vérifiez les dimensions (20 cm)
- Testez l'éclairage
- Essayez avec une autre imprimante
- Utilisez du papier mat (pas brillant)

Bonne chance pour votre démonstration ! 🚀
