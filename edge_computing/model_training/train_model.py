import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.svm import SVC
from sklearn.metrics import classification_report
import joblib

def train():
    # Load data
    df = pd.read_csv('sensor_data.csv')
    X = df[['vibration', 'temperature', 'current']]
    y = df['label']

    # Split
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

    # Train SVM
    print("Training SVM model...")
    model = SVC(kernel='rbf', probability=True)
    model.fit(X_train, y_train)

    # Evaluate
    y_pred = model.predict(X_test)
    print(classification_report(y_test, y_pred))

    # Save model
    joblib.dump(model, 'anomaly_detector.pkl')
    print("Model saved to anomaly_detector.pkl")

if __name__ == "__main__":
    train()
