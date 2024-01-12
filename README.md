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

## The Project
The rocket on-board computer project is a comprehensive initiative aimed at developing a sophisticated system to manage and control various aspects of a rocket's operation. 

The project encompasses five main components:
 1. On-Board Computer: https://github.com/Filipeak/rocket-obc
 2. Telemetry: https://github.com/Filipeak/rocket-tlm
 3. Estimation & Control Library: https://github.com/Filipeak/rocket-tlm
 4. Ground Station: https://github.com/Filipeak/rocket-gcs
 5. Hardware designs: https://github.com/Filipeak/rocket-hw