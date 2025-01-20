import board
import adafruit_dht
import time

def DHT22_222(id_queue, return_queue):
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