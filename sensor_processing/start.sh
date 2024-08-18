#!/bin/bash

# Starten Sie den Python-Prozess im Vordergrund
python iounit_processing.py &

# Warten Sie auf SIGTERM oder SIGINT und beenden Sie den Prozess korrekt
trap "kill 0" SIGTERM SIGINT

# Warten Sie auf alle Hintergrundprozesse
wait