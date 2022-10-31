import json
import time
from threading import Timer
import threading
import RPi.GPIO as GPIO
import requests

#Lights pins
GREEN_EW, YELLOW_EW, RED_EW = 2, 3, 4
GREEN_WE, YELLOW_WE, RED_WE = 0, 0, 0
GREEN_NS, YELLOW_NS, RED_NS = 17, 27, 22
GREEN_SN, YELLOW_SN, RED_SN = 0, 0, 0

#Sensors pins
TRIGGER_EW, ECHO_EW = 14, 15 
TRIGGER_WE, ECHO_WE = 0, 0
TRIGGER_NS, ECHO_NS = 23, 24 
TRIGGER_SN, ECHO_SN = 0, 0 

#Global variables
IOTHUB_URL = 'https://smartlightsapi.azurewebsites.net/api/TrafficData'
lightsDurationEW = 6
lightsDurationNS = 3

carCountEW, carCountWE, carCountNS, carCountSN = 0, 0, 0, 0
carPassingTimeEW, carPassingTimeWE, carPassingTimeNS, carPassingTimeSN = 0, 0, 0, 0


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
    global carCountEW
    global carCountWE
    global carCountNS
    global carCountSN

    data = {
        "TrafficLightID": "62f874e8d55125227da25a0f",
        "TrafficLightName": "Soldado y Olleros",
        "Latitude": -34.565221,
        "Longitude": -58.438399,
        "CarCountEW": carCountEW,
        "CarPassingTimeEW": carPassingTimeEW,
        "CarCountWE": carCountWE,
        "CarPassingTimeWE": carPassingTimeWE,
        "CarCountNS": carCountNS,
        "CarPassingTimeNS": carPassingTimeNS,
        "CarCountSN": carCountSN,
        "CarPassingTimeSN": carPassingTimeSN
    }
    print(data)
    response = requests.post(IOTHUB_URL, json = data)
    print(response.status_code)
    clearCounters()
    

def lights():
    global lightsDurationEW
    global lightsDurationNS

    while True:
        print('Green 1')
        GPIO.output(GREEN_EW,GPIO.HIGH)
        print('Red 2')
        GPIO.output(RED_NS,GPIO.HIGH)
        time.sleep(lightsDurationEW)
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
        time.sleep(lightsDurationNS)
        GPIO.output(RED_EW,GPIO.LOW)
        GPIO.output(GREEN_NS,GPIO.LOW)
        print('Yellow 1')
        GPIO.output(YELLOW_EW,GPIO.HIGH)
        GPIO.output(YELLOW_NS,GPIO.HIGH)
        time.sleep(1)
        GPIO.output(YELLOW_EW,GPIO.LOW)
        GPIO.output(YELLOW_NS,GPIO.LOW)      


def EWSensor():
    global carCountEW
    global carPassingEW 
    global carPassingTimeEW
    carPassingEW = False

    while True:

        GPIO.output(TRIGGER_EW, False)
        time.sleep(0.05)
    
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
        
    #measure how long it takes a car to pass to get an approximate idea of the speed 
        if(distance < 20 and carPassingEW == False):
            print("EW: Car passing")
            carPassingEW = True
            start = time.time()
        else:
            if(distance > 20 and carPassingEW == True):
                carPassingEW = False
                end = time.time()
                timeTaken = end - start
                print("EW: Car passed. Time taken: " + str(timeTaken))
                carCountEW = carCountEW + 1
                carPassingTimeEW = carPassingTimeEW + timeTaken

def WESensor():
    global carCountWE
    global carPassingWE 
    global carPassingTimeWE
    carPassingWE = False

    while True:

        GPIO.output(TRIGGER_WE, False)
        time.sleep(0.05)
    
    #generate 10us pulse
        GPIO.output(TRIGGER_WE, True)
        time.sleep(0.00001)
        GPIO.output(TRIGGER_WE, False)
    
    #measure how long it takes to read it in echo pin
        while GPIO.input(ECHO_WE)==0:
            pulseStart = time.time()
    
        while GPIO.input(ECHO_WE)==1:
            pulseEnd = time.time()
    
        pulseDuration = pulseEnd - pulseStart
        distance = pulseDuration * 17150
        distance = round(distance, 2)
        
    #measure how long it takes a car to pass to get an approximate idea of the speed 
        if(distance < 20 and carPassingWE == False):
            print("EW: Car passing")
            carPassingWE = True
            start = time.time()
        else:
            if(distance > 20 and carPassingWE == True):
                carPassingWE = False
                end = time.time()
                timeTaken = end - start
                print("EW: Car passed. Time taken: " + str(timeTaken))
                carCountWE = carCountWE + 1
                carPassingTimeWE = carPassingTimeWE + timeTaken

def NSSensor():
    global carCountNS
    global carPassingNS
    global carPassingTimeNS
    carPassingNS = False

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
        
        #measure how long it takes a car to pass to get an approximate idea of the speed 
        if(distance < 20 and carPassingNS == False):
            print("NS: Car passing")
            carPassingNS = True
            start = time.time()
        else:
            if(distance > 20 and carPassingNS == True):
                carPassingNS = False
                end = time.time()
                timeTaken = end - start
                print("NS: Car passed. Time taken: " + str(timeTaken))
                carCountNS = carCountNS + 1
                carPassingTimeNS = carPassingTimeNS + timeTaken

def SNSensor():
    global carCountSN
    global carPassingSN
    global carPassingTimeSN
    carPassingSN = False

    while True:

        GPIO.output(TRIGGER_SN, False)
        time.sleep(0.1)
    
    #generate 10us pulse
        GPIO.output(TRIGGER_SN, True)
        time.sleep(0.00001)
        GPIO.output(TRIGGER_SN, False)
    
    #measure how long it takes to read it in echo pin
        while GPIO.input(ECHO_SN)==0:
            pulseStart = time.time()
    
        while GPIO.input(ECHO_SN)==1:
            pulseEnd = time.time()
    
        pulseDuration = pulseEnd - pulseStart
        distance = pulseDuration * 17150
        distance = round(distance, 2)
        
        #measure how long it takes a car to pass to get an approximate idea of the speed 
        if(distance < 20 and carPassingSN == False):
            print("NS: Car passing")
            carPassingSN = True
            start = time.time()
        else:
            if(distance > 20 and carPassingSN == True):
                carPassingSN = False
                end = time.time()
                timeTaken = end - start
                print("NS: Car passed. Time taken: " + str(timeTaken))
                carCountSN = carCountSN + 1
                carPassingTimeSN = carPassingTimeSN + timeTaken


def clearCounters():
    global carCountEW
    global carCountWE
    global carCountNS
    global carCountSN
    carCountEW = 0
    carCountWE = 0
    carCountNS = 0
    carCountSN = 0


class RepeatTimer(Timer):  
    def run(self):  
        while not self.finished.wait(self.interval):   
            postTrafficData()            



lights_thread = threading.Thread(target=lights)
lights_thread.start()

sensor_thread = threading.Thread(target=EWSensor)
sensor_thread.start()

# sensor_thread = threading.Thread(target=WESensor)
# sensor_thread.start()

sensor_thread = threading.Thread(target=NSSensor)
sensor_thread.start()

# sensor_thread = threading.Thread(target=SNSensor)
# sensor_thread.start()

postData_timer = RepeatTimer(10,postTrafficData)  
postData_timer.start()

while True:
    time.sleep(1)
