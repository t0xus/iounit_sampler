import time
import board
import adafruit_dht
import psycopg2
from psycopg2 import sql
from multiprocessing import Process, Queue
import time

#def DHT22_1(id_queue1, value_queue, return_queue):
def DHT22_1(id_queue, return_queue):
    while True:
        message = id_queue.get()
        if message == 0:
            break
        
        result = 0
        
        dhtDevice = adafruit_dht.DHT22(board.D4, use_pulseio=False)
        
        cal_run = True
        while cal_run:
            try:
                #if message == <iounit_data_masterdata.id>
                if message == 1:
                    result = dhtDevice.temperature
                    cal_run = False
                elif message == 2: 
                    result = dhtDevice.humidity
                    cal_run = False
            except RuntimeError as error:
                # Errors happen fairly often, DHT's are hard to read, just keep going
                print(error.args[0])
                time.sleep(2)
                continue
            except Exception as error:
                dhtDevice.exit()
                raise error

            time.sleep(2)
        
        
        
        return_queue.put(result)
        time.sleep(1)





if __name__ == "__main__":
    id_queue = Queue()
    return_queue = Queue()

    #Hier muss noch ein Listendatentyp hin
    process1 = Process(target=DHT22_1, args=(id_queue, return_queue))
    process1.start()


    db_config = {
        'dbname': 'postgres',
        'user': 'postgres',
        'password': 'xxx',
        'host': '172.17.0.1',
        'port': 5432
    }
    
    conn = psycopg2.connect(**db_config)

    while True:
        

        
        
        
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
            
            
            #Bestätige der API, das die Anfrage zu diesem Zeitpunkt registriert wurde
            update_query_b = "UPDATE iounit_data_currently set direction_stamp_b = CURRENT_TIMESTAMP WHERE id_sdm = {temp_id_sdm}"
            cursor_update_b = conn.cursor()
            cursor_update_b.execute(update_query_b.format(temp_id_sdm = temp_id_sdm))
            conn.commit()
            cursor_update_b.close()
            
            
            #result = 3000
            
            #Kommunikation mit dem IO Thread
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

