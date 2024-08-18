# Verwende ein leichtgewichtiges Python-Image als Basis
FROM python:3.11-slim

# Setze das Arbeitsverzeichnis im Container
WORKDIR /app

# Installiere gcc und andere benötigte Werkzeuge
RUN apt-get update && apt-get install -y gcc build-essential libpq-dev

# Kopiere die requirements.txt und installiere die Python-Pakete
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt
#RUN pip install --no-cache-dir adafruit-circuitpython-dht

# Kopiere das Python-Skript in das Arbeitsverzeichnis des Containers
COPY iounit_processing.py .
COPY start.sh .

# Standard-Befehl, der beim Start des Containers ausgeführt wird
#CMD ["python", "iounit_processing.py"]

# Geben Sie dem Skript Ausführungsrechte
RUN chmod +x start.sh

# Verwenden Sie das Wrapper-Skript als Startbefehl
CMD ["sh", "./start.sh"]