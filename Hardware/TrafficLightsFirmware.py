import json
import time
from threading import Timer
import threading
import RPi.GPIO as GPIO
import requests

#Lights pins
GREEN_EW, YELLOW_EW, RED_EW = 2, 3, 4
GREEN_NS, YELLOW_NS, RED_NS = 17, 27, 22

#Sensors pins
TRIGGER_EW, ECHO_EW = 14, 15 
TRIGGER_NS, ECHO_NS = 23, 24 

#Global variables
IOTHUB_URL = 'https://smartlightsapi.azurewebsites.net/api/TrafficData'
EWDuration, NSDuration = 6,3

trafficEWCounter = 0
trafficWECounter = 0
trafficNSCounter = 0
trafficSNCounter = 0

GPIO.setmode(GPIO.BCM)
GPIO.setwarnings(False)

#Lights setup
GPIO.setup(GREEN_EW, GPIO.OUT, initial=1)
GPIO.setup(YELLOW_EW, GPIO.OUT, initial=1)
GPIO.setup(RED_EW, GPIO.OUT, initial=1)
GPIO.setup(GREEN_NS, GPIO.OUT, initial=1)
GPIO.setup(YELLOW_NS, GPIO.OUT, initial=1)
GPIO.setup(RED_NS, GPIO.OUT, initial=1)
time.sleep(2)
GPIO.output(GREEN_EW, GPIO.LOW)
GPIO.output(GREEN_NS, GPIO.LOW)
GPIO.output(YELLOW_EW, GPIO.LOW)
GPIO.output(YELLOW_NS, GPIO.LOW)
GPIO.output(RED_EW, GPIO.LOW)
GPIO.output(RED_NS, GPIO.LOW)

#Sensors setup
GPIO.setup(TRIGGER_EW,GPIO.OUT, initial=0)
GPIO.setup(ECHO_EW,GPIO.IN)
GPIO.setup(TRIGGER_NS,GPIO.OUT, initial=0)
GPIO.setup(ECHO_NS,GPIO.IN)




def postTrafficData():
    global trafficEWCounter
    global trafficWECounter
    global trafficNSCounter
    global trafficSNCounter

    data = {
        "TrafficLightID": "62f874e8d55125227da25a0f",
        "TrafficLightName": "Soldado y Olleros",
        "Latitude": -34.565221,
        "Longitude": -58.438399,
        "RoadA_EW": trafficEWCounter,
        "RoadA_WE": trafficWECounter,
        "RoadB_NS": trafficNSCounter,
        "RoadB_SN": trafficSNCounter
    }
    print(data)
    response = requests.post(IOTHUB_URL, json = data)
    print(response.status_code)
    clearCounters()
    

def lights():
    global EWDuration
    global NSDuration

    while True:
        print('Green 1')
        GPIO.output(GREEN_EW,GPIO.HIGH)
        print('Red 2')
        GPIO.output(RED_NS,GPIO.HIGH)
        time.sleep(EWDuration)
        GPIO.output(GREEN_EW,GPIO.LOW)
        GPIO.output(RED_NS,GPIO.LOW)
        print('Yellow 1')
        GPIO.output(YELLOW_EW,GPIO.HIGH)
        print('Yellow 2')
        GPIO.output(YELLOW_NS,GPIO.HIGH)
        time.sleep(1)
        GPIO.output(YELLOW_EW,GPIO.LOW)
        GPIO.output(YELLOW_NS,GPIO.LOW)
        print('Red 1')
        GPIO.output(RED_EW,GPIO.HIGH)
        print('Green 2')
        GPIO.output(GREEN_NS,GPIO.HIGH)
        time.sleep(NSDuration)
        GPIO.output(RED_EW,GPIO.LOW)
        GPIO.output(GREEN_NS,GPIO.LOW)
        print('Yellow 1')
        GPIO.output(YELLOW_EW,GPIO.HIGH)
        GPIO.output(YELLOW_NS,GPIO.HIGH)
        time.sleep(1)
        GPIO.output(YELLOW_EW,GPIO.LOW)
        GPIO.output(YELLOW_NS,GPIO.LOW)      


def EWSensor():
    global trafficEWCounter
    global EWCarPassing 
    EWCarPassing = False

    while True:

        GPIO.output(TRIGGER_EW, False)
        time.sleep(0.1)
    
    #generate 10us pulse
        GPIO.output(TRIGGER_EW, True)
        time.sleep(0.00001)
        GPIO.output(TRIGGER_EW, False)
    
    #measure how long it takes to read it in echo pin
        while GPIO.input(ECHO_EW)==0:
            pulseStart = time.time()
    
        while GPIO.input(ECHO_EW)==1:
            pulseEnd = time.time()
    
        pulseDuration = pulseEnd - pulseStart
        distance = pulseDuration * 17150
        distance = round(distance, 2)
        
        if(distance < 20):
            print("EW: Car passing")
            EWCarPassing = True
        else:
            if(EWCarPassing == True):
                print("EW: Car passed")
                EWCarPassing = False
                trafficEWCounter = trafficEWCounter + 1


def NSSensor():
    global trafficNSCounter
    global NSCarPassing 
    NSCarPassing = False

    while True:

        GPIO.output(TRIGGER_NS, False)
        time.sleep(0.1)
    
    #generate 10us pulse
        GPIO.output(TRIGGER_NS, True)
        time.sleep(0.00001)
        GPIO.output(TRIGGER_NS, False)
    
    #measure how long it takes to read it in echo pin
        while GPIO.input(ECHO_NS)==0:
            pulseStart = time.time()
    
        while GPIO.input(ECHO_NS)==1:
            pulseEnd = time.time()
    
        pulseDuration = pulseEnd - pulseStart
        distance = pulseDuration * 17150
        distance = round(distance, 2)
        
        if(distance < 20):
            print("EW: Car passing")
            NSCarPassing = True
        else:
            if(NSCarPassing == True):
                print("EW: Car passed")
                NSCarPassing = False
                trafficNSCounter = trafficNSCounter + 1


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



lights_thread = threading.Thread(target=lights)
lights_thread.start()

sensor_thread = threading.Thread(target=EWSensor)
sensor_thread.start()

postData_timer = RepeatTimer(10,postTrafficData)  
postData_timer.start()

while True:
    time.sleep(1)
