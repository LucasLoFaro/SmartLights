import json
import time
from threading import Timer
import threading
import RPi.GPIO as GPIO
import random

GREEN1, YELLOW1, RED1 = 18, 23, 24 
GREEN2, YELLOW2, RED2 = 17, 27, 22

trafficEWCounter = 0
trafficWECounter = 0
trafficNSCounter = 0
trafficSNCounter = 0

GPIO.setmode(GPIO.BCM)
GPIO.setwarnings(False)

GPIO.setup(GREEN1,GPIO.OUT)
GPIO.setup(YELLOW1,GPIO.OUT)
GPIO.setup(RED1,GPIO.OUT)
GPIO.setup(GREEN2,GPIO.OUT)
GPIO.setup(YELLOW2,GPIO.OUT)
GPIO.setup(RED2,GPIO.OUT)



def postTrafficData():
    global trafficEWCounter
    global trafficWECounter
    global trafficNSCounter
    global trafficSNCounter

    data = json.dumps({
        "TrafficLightID": "62f874e8d55125227da25a0f",
        "TrafficLightName": "Soldado y Olleros",
        "Latitude": -34.56536,
        "Longitude": -58.438618,
        "RoadA_EW": trafficEWCounter,
        "RoadA_WE": trafficWECounter,
        "RoadB_NS": trafficNSCounter,
        "RoadB_SN": trafficSNCounter
    })
    print(data)
    clearCounters()
    

def light1():
    while True:
        print('Green 1')
        GPIO.output(GREEN1,GPIO.HIGH)
        time.sleep(6)
        GPIO.output(GREEN1,GPIO.LOW)
        print('Yellow 1')
        GPIO.output(YELLOW1,GPIO.HIGH)
        time.sleep(1)
        GPIO.output(YELLOW1,GPIO.LOW)
        print('Red 1')
        GPIO.output(RED1,GPIO.HIGH)
        time.sleep(3)
        GPIO.output(RED1,GPIO.LOW)


def light2():
    while True:
        print('Red 2')
        GPIO.output(RED2,GPIO.HIGH)
        time.sleep(6)
        GPIO.output(RED2,GPIO.LOW)
        print('Yellow 2')
        GPIO.output(YELLOW2,GPIO.HIGH)
        time.sleep(1)
        GPIO.output(YELLOW2,GPIO.LOW)
        print('Green 2')
        GPIO.output(GREEN2,GPIO.HIGH)
        time.sleep(3)
        GPIO.output(GREEN2,GPIO.LOW)


def clearCounters():
    global trafficEWCounter
    global trafficWECounter
    global trafficNSCounter
    global trafficSNCounter
    trafficEWCounter = 0
    trafficWECounter = 0
    trafficNSCounter = 0
    trafficSNCounter = 0


class RepeatTimer(Timer):  
    def run(self):  
        while not self.finished.wait(self.interval):   
            postTrafficData()            



light1_thread = threading.Thread(target=light1)
light1_thread.start()

light2_thread = threading.Thread(target=light2)
light2_thread.start() 

postData_timer = RepeatTimer(10,postTrafficData)  
postData_timer.start()

while True:
    trafficEWCounter = random.randint(0, 9)
    trafficWECounter = random.randint(0, 9)
    trafficNSCounter = random.randint(0, 9)
    trafficSNCounter = random.randint(0, 9)
    time.sleep(1)