import time
import board
import adafruit_dht
import psycopg2
from psycopg2 import sql


# Initial the dht device, with data pin connected to:
#dhtDevice = adafruit_dht.DHT22(board.D18)

# you can pass DHT22 use_pulseio=False if you wouldn't like to use pulseio.
# This may be necessary on a Linux single board computer like the Raspberry Pi,
# but it will not work in CircuitPython.
dhtDevice = adafruit_dht.DHT22(board.D4, use_pulseio=False)


try:
    # Print the values to the serial port
    temperature_c = dhtDevice.temperature
    #temperature_f = temperature_c * (9 / 5) + 32
    humidity = dhtDevice.humidity
    
    #print(
    #    "Temp: {:.1f} F / {:.1f} C    Humidity: {}% ".format(
    #        temperature_f, temperature_c, humidity
    #    )
    #)

    # Datenbankverbindungsparameter
    
    db_config = {
        'dbname': 'postgres',
        'user': 'postgres',
        'password': 'FlyHigh69#2024',
        'host': 'localhost',
        'port': 5432
    }

    # Datenzeile, die eingefügt werden soll
    data = {
        'datetime': '2024-07-14 23:06:50',
        'value_numeric': temperature_c,
        'value_alphanumeric': None,
        'id_mu': None,
        'id_sc': 1
    }

    # Verbindung zur Datenbank herstellen
    conn = psycopg2.connect(**db_config)
    cur = conn.cursor()

    # SQL INSERT-Befehl
    insert_query = sql.SQL("""
        INSERT INTO sensor_data_chronology (datetime, value_numeric, value_alphanumeric, id_mu, id_sc)
        VALUES (%s, %s, %s, %s, %s)
    """)

    # SQL INSERT-Befehl ausführen
    cur.execute(insert_query, (
        data['datetime'],
        data['value_numeric'],
        data['value_alphanumeric'],
        data['id_mu'],
        data['id_sc']
    ))

    # Änderungen speichern
    conn.commit()

    # Verbindung schließen
    cur.close()
    conn.close()

    print("Datenzeile erfolgreich eingefügt.")

    
    
    

except RuntimeError as error:
    # Errors happen fairly often, DHT's are hard to read, just keep going
    print(error.args[0])
    print("My line")
    time.sleep(2.0)
    #continue
except Exception as error:
    dhtDevice.exit()
    raise error
