# Guide pour Générer les Images des Diagrammes UML

Ce guide vous explique comment convertir les fichiers `.puml` en images PNG pour le README.

## Option 1 : En Ligne (Le Plus Rapide)

1. Allez sur **https://www.plantuml.com/plantuml/uml/**
2. Pour chaque fichier `.puml` dans `documentation/uml_diagrams/` :
   - Ouvrez le fichier
   - Copiez tout le contenu (Ctrl+A, Ctrl+C)
   - Collez dans l'éditeur en ligne
   - Le diagramme s'affiche automatiquement
   - Clic droit sur l'image → **Enregistrer l'image sous...**
   - Sauvegardez dans le dossier `images/` avec le nom correspondant :
     - `uml_use_case.png`
     - `uml_sequence.png`
     - `uml_deployment.png`
     - `uml_component.png`
     - `uml_activity.png`
     - `uml_class.png`

## Option 2 : VS Code (Automatique)

1. Installez l'extension **PlantUML** dans VS Code
2. Ouvrez chaque fichier `.puml`
3. Appuyez sur **Alt+D** pour voir l'aperçu
4. Clic droit sur l'aperçu → **Export Current Diagram**
5. Choisissez **PNG**
6. Sauvegardez dans le dossier `images/`

## Option 3 : Ligne de Commande

```bash
# Installer PlantUML
choco install plantuml

# Générer toutes les images
cd documentation/uml_diagrams
plantuml *.puml -o ../../images/
```

## Fichiers à Générer

| Fichier Source | Image de Sortie |
|----------------|------------------|
| `01_use_case_diagram.puml` | `images/uml_use_case.png` |
| `02_sequence_diagram_anomaly.puml` | `images/uml_sequence.png` |
| `03_class_diagram.puml` | `images/uml_class.png` |
| `04_deployment_diagram.puml` | `images/uml_deployment.png` |
| `05_activity_diagram_training.puml` | `images/uml_activity.png` |
| `06_component_diagram.puml` | `images/uml_component.png` |

Une fois générées, ces images seront automatiquement affichées dans le README GitHub !
