import RPi.GPIO as GPIO
import adafruit_dht
import time

# Definiere den GPIO-Pin, an den der Sensor angeschlossen ist


# Definiere den Sensortyp (DHT11 oder DHT22)
#sensor = dht.DHT22(pin)

dhtDevice = adafruit_dht.DHT22(D4)

GPIO.setwarnings(False)
GPIO.setmode(GPIO.BCM)

try:
    # Print the values to the serial port
    temperature_c = dhtDevice.temperature
    temperature_f = temperature_c * (9 / 5) + 32
    humidity = dhtDevice.humidity
    print(
        "Temp: {:.1f} F / {:.1f} C    Humidity: {}% ".format(
            temperature_f, temperature_c, humidity
        )
    )

except RuntimeError as error:
    # Errors happen fairly often, DHT's are hard to read, just keep going
    print(error.args[0])
    time.sleep(2)
except Exception as error:
    dhtDevice.exit()
    raise error