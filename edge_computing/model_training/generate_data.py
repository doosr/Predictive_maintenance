import pandas as pd
import numpy as np

# Configuration
NUM_SAMPLES = 1000

def generate_data():
    data = []
    for _ in range(NUM_SAMPLES):
        # Normal state
        if np.random.random() > 0.1: 
            vibration = np.random.normal(1.5, 0.5)
            temperature = np.random.normal(50, 5)
            current = np.random.normal(7, 1)
            label = 0 # Normal
        # Anomaly state
        else:
            vibration = np.random.normal(6.0, 1.5) # High vibration
            temperature = np.random.normal(70, 10) # Overheating
            current = np.random.normal(12, 2) # High current
            label = 1 # Anomaly
        
        data.append([vibration, temperature, current, label])

    df = pd.DataFrame(data, columns=['vibration', 'temperature', 'current', 'label'])
    df.to_csv('sensor_data.csv', index=False)
    print("Dataset generated: sensor_data.csv")

if __name__ == "__main__":
    generate_data()
