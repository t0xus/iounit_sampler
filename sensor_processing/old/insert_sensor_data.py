import psycopg2
from psycopg2 import sql

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
    'value_numeric': 999,
    'value_alphanumeric': None,
    'id_mu': None,
    'id_sc': None
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
