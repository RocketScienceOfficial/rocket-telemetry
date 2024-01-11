# Rocket Telemetry

## Overiew
Simple application made in Unity for rocket telemetry

## Features
 - Live Telemetry
   - Speed
   - Altitude
   - Battery
   - Signal
   - Map (Google Maps)
   - Wind prediction (OpenWeatherMap)
   - Pre-flight checks
   - PDF Report
   - Flight configuration
 - Data download
 - Data replay
 - Simulation from .csv file

## Setup
Project is done on Unity 2021.3.2f1 but it should be fine to run it on other versions

Install Unity and open project

Provide Google Maps API Key and OpenWeatherMap API Key in ```Utils/config.json``` file (otherwise map and wind prediction will not work). Scheme is provided in ```Utils/config.example.json```