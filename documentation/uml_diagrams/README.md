# Diagrammes UML - Projet Maintenance Prédictive

Ce dossier contient tous les diagrammes UML nécessaires pour la documentation du PFE.

## 📋 Liste des Diagrammes

### 1. **Diagramme de Cas d'Utilisation** (`01_use_case_diagram.puml`)
- **Objectif** : Identifier les acteurs du système et leurs interactions
- **Acteurs** : Opérateur, Machine, Système IA, Administrateur
- **Usage** : Chapitre 2 du mémoire (Analyse et Conception)

### 2. **Diagramme de Séquence** (`02_sequence_diagram_anomaly.puml`)
- **Objectif** : Détailler le flux de données lors de la détection d'une anomalie
- **Montre** : Communication MQTT, inférence IA, alerte temps réel
- **Usage** : Chapitre 2 & 3 (Conception et Réalisation)

### 3. **Diagramme de Classes** (`03_class_diagram.puml`)
- **Objectif** : Représenter la structure objet du système
- **Contient** : Classes principales de chaque couche (IoT, Edge, Backend, Frontend)
- **Usage** : Chapitre 2 (Conception)

### 4. **Diagramme de Déploiement** (`04_deployment_diagram.puml`)
- **Objectif** : Montrer l'architecture physique (Hardware + Network)
- **Affiche** : ESP32, Raspberry Pi, Serveur Web, Cloud
- **Usage** : Chapitre 2 & Présentation (infrastructure)

### 5. **Diagramme d'Activité** (`05_activity_diagram_training.puml`)
- **Objectif** : Processus d'entraînement du modèle IA
- **Détaille** : Collecte de données → Entraînement → Validation → Déploiement
- **Usage** : Chapitre 3 (Réalisation)

### 6. **Diagramme de Composants** (`06_component_diagram.puml`)
- **Objectif** : Architecture logicielle du système
- **Montre** : Tous les modules et leurs interfaces (MQTT, WebSocket, etc.)
- **Usage** : Chapitre 2 (Architecture détaillée)

---

## 🛠️ Comment générer les images

Ces fichiers sont au format **PlantUML** (`.puml`), un langage textuel pour créer des diagrammes UML.

### Option 1 : En ligne (Rapide)
1. Ouvrez [PlantUML Online Editor](https://www.plantuml.com/plantuml/uml/)
2. Copiez-collez le contenu d'un fichier `.puml`
3. Le diagramme s'affiche automatiquement
4. Téléchargez l'image (PNG ou SVG)

### Option 2 : VS Code (Recommandé)
1. Installez l'extension **PlantUML** dans VS Code
2. Ouvrez un fichier `.puml`
3. Appuyez sur `Alt+D` pour prévisualiser
4. Clic droit → `Export Current Diagram` → Choisir PNG

### Option 3 : Ligne de commande (Java requis)
```bash
java -jar plantuml.jar *.puml
```

---

## 📖 Utilisation dans le mémoire

- **Insertion dans Word/LaTeX** : Exportez en PNG haute résolution (300 DPI minimum)
- **Légendes** : Ajoutez toujours une légende explicative sous chaque diagramme
- **Numérotation** : Figure X.Y (X = numéro chapitre, Y = numéro figure)

Exemple :
> *Figure 2.1 : Diagramme de cas d'utilisation montrant les interactions entre les acteurs du système et les principales fonctionnalités de la plateforme.*

---

## ✅ Checklist pour la soutenance

- [ ] Imprimer les diagrammes en A4 couleur
- [ ] Préparer une version simplifiée pour les slides PowerPoint
- [ ] Être capable d'expliquer chaque flèche et composant
- [ ] Relier chaque diagramme à une partie du code réel

Bon courage ! 🎓
