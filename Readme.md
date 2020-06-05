# Plague Simulator

Learning project which aims to simulate the evolution of a disease inside 
a defined population using Multi Agent system in Unity.

## Features

| Feature                    | Progress                                                       |
|----------------------------|----------------------------------------------------------------|
| Citizen Agent              | Done                                                           |
| Mayor Agent                | Done                                                           |
| Environment                | Done                                                           |
| Simulation settings        | Done                                                           |
| Use of real disease models | Done (Gaussian disease duration)                               |

## Story

The simulation take place in a fictive city where a disease is spreading. 

Citizens are getting sick by getting close to another agent that is contagious.

Some rules lead the simulation :

- Citizens want to get outside or want to see people, if they can't their 
stress rise until they die if they can't satisfy their needs before the maximum
time.
- Citizens follow rules that are set by the Mayor agent that can influence
the ability of citizens to get outside or getting close of others.

## User interface and controls

### Setup scene

The Setup scene provide some input that can be modified by the user to setup the 
simulation information.

![Setup Ui](readmefiles/SetupUi.png)

Here are the meanings of the inputs:

- __Population density__: Number of citizens at launch time.
- __Infectivity__: Probability for a citizen to become sick when being close to another agent.
- __Launch sick number__: Percentage of sick citizen at launch time.
- __Launch immuned number__: Percentage of immuned citizen at launch time.
- __Disease Duration__: Disease duration in second.
- __Death statistics__: Death probability when being sick.

### Simulation

![Setup Ui](readmefiles/SimulationScene.png)

Some inputs allow the user to move the camera :

- __Z__: Move up
- __S__: Move down
- __Q__: Move left
- __F__: Move Right
- __Space__: Move Forward
- __Left Ctrl__: Move backward

## Based on

[Gaussian function](https://gist.github.com/tansey/1444070) 

## Contributors

[Simon Perche](https://github.com/Solidras)

[Cyprien Plateau--Holleville](https://github.com/PlathC)