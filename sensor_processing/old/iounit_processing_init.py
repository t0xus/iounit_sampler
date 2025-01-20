import time
import board
import adafruit_dht
import psycopg2
from psycopg2 import sql
from multiprocessing import Process, Queue
import time

from jinja2 import Template






if __name__ == "__main__":
    id_queue = Queue()
    return_queue = Queue()
    
    #Neuer Code
    
    with open("iounit_processing.j2", "r") as tpl_file:
        template_str = tpl_file.read()

    # 2. Template parsen
    template = Template(template_str)

    # 3. Platzhalter füllen
    rendered_content = template.render(
        drv_pckges="from sensor_type_drvs.DHT22 import DHT22_222"
    )

    # 4. In eine neue .py-Datei schreiben
    with open("iounit_processing.py", "w") as out_file:
        out_file.write(rendered_content)

    print(" wurde erstellt.")
    
    