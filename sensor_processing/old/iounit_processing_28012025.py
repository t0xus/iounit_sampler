import time
import board
import adafruit_dht
import psycopg2
from psycopg2 import sql
from multiprocessing import Process, Queue
import time

from jinja2 import Template

from sensor_type_drvs.DHT22 import DHT22




if __name__ == "__main__":
    id_queue = Queue()
    return_queue = Queue()
    board_pin = Queue()
    
    #Hier muss noch ein Listendatentyp hin
    process1 = Process(target=DHT22, args=(id_queue, return_queue, board_pin))
    process1.start()
    

    db_config = {
        'dbname': 'postgres',
        'user': 'postgres',
        'password': 'FlyHigh69#2024',
        'host': '172.17.0.1',
        'port': 5432
    }
    
    conn = psycopg2.connect(**db_config)

    while True:
        

        
        print("Hallo1")
        
        cursor_read = conn.cursor()
        # SQL SELECT Befehl
        select_query = "SELECT id_sdm, value_numerical, value_alphanumerical FROM iounit_data_currently WHERE direction_stamp_a IS NOT NULL ORDER BY direction_stamp_a ASC LIMIT 1"
        
        cursor_read.execute(select_query)
        records = cursor_read.fetchall()

        
        
        
        for row in records:
            temp_id_sdm = row[0]
            #if row.count() != 0:
            #temp_id_sdm = None
            
            #for row in records:
            #temp_id_sdm = row.id_sdm
            print("Hallo2")
            
            #Bestätige der API, das die Anfrage zu diesem Zeitpunkt registriert wurde
            update_query_b = "UPDATE iounit_data_currently set direction_stamp_b = CURRENT_TIMESTAMP WHERE id_sdm = {temp_id_sdm}"
            cursor_update_b = conn.cursor()
            cursor_update_b.execute(update_query_b.format(temp_id_sdm = temp_id_sdm))
            conn.commit()
            cursor_update_b.close()
            
            cursor_read2 = conn.cursor()
            # SQL SELECT Befehl
            select_query = "SELECT d1, d2, d3, d4, d5, d6, d7, d8 from iounit_configuration WHERE id = " + str(temp_id_sdm)
        
            cursor_read2.execute(select_query)
            records2 = cursor_read2.fetchall()
            board_pin_enum = None
            for row2 in records2:
            
            
                #Auflösen der PIN Datenbankeinträge
                if row2[0] == "x":
                    board_pin_enum = board.D1
                elif row2[1] == "x": 
                    board_pin_enum = board.D2
                elif row2[2] == "x": 
                    board_pin_enum = board.D3
                elif row2[3] == "x":
                    board_pin_enum = board.D4
                elif row2[4] == "x":
                    board_pin_enum = board.D5
                elif row2[5] == "x":
                    board_pin_enum = board.D6
                elif row2[6] == "x":
                    board_pin_enum = board.D7
                elif row2[7] == "x":
                    board_pin_enum = board.D8
                    
            cursor_read2.close
            
            #Kommunikation mit dem IO Thread
            board_pin.put(board_pin_enum)
            id_queue.put(temp_id_sdm)
            
            while return_queue.qsize() == 0:
                time.sleep(0.1)
            
            result = return_queue.get()
            
            #Eintrag in die Laufdatentabelle iounit_data_chronology
            cursor_insert = conn.cursor()
            insert_query = sql.SQL("""
                INSERT INTO iounit_data_chronology (datetime, value_numeric, value_alphanumeric, id_mu, id_sc)
                VALUES (CURRENT_TIMESTAMP, %s, %s, %s, %s)
            """)
            
            data = {
                'value_numeric': result,
                'value_alphanumeric': None,
                'id_mu': None,
                'id_sc': 1
            }
            
            cursor_insert.execute(insert_query, (
                data['value_numeric'],
                data['value_alphanumeric'],
                data['id_mu'],
                data['id_sc']
            ))

            conn.commit()
            cursor_insert.close()
            
            #Update current Wert in iounit_data_currently
            update_query_value_numerical = "UPDATE iounit_data_currently set value_numerical = {result} WHERE id_sdm = {temp_id_sdm}"
            cursor_update_value_numerical = conn.cursor()
            cursor_update_value_numerical.execute(update_query_value_numerical.format(temp_id_sdm = temp_id_sdm, result = result))
            conn.commit()
            cursor_update_value_numerical.close()
            
            #Update an web api das Daten abgeholt werden können
            update_query_a = "UPDATE iounit_data_currently set direction_stamp_a = NULL WHERE id_sdm = {temp_id_sdm}"
            cursor_update_a = conn.cursor()
            cursor_update_a.execute(update_query_a.format(temp_id_sdm = temp_id_sdm))
            conn.commit()
            cursor_update_a.close()

        cursor_read.close()
        
        time.sleep(1)
        
        #Unten das zum testen bleibt erst mal drin
        #break
    
    #Das auch zum testen
    process1.terminate()
    
    conn.close()
