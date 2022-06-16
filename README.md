# Vimal
Vimal is MVP UWP application for writing scripts, executing them and providing syntax simple highlighting for specified languages.
> Vimal stands for Vim's Rival

## Content
- [Installation](#instalation)
- [How To Use](#how-to-use)
- [What has been done](#)

## Instalation
> App is supported only on devices with Windows of version at least 10.0.17763.0

Installation package can be downloaded from [here](https://uloz.to/tam/32374d05-410b-41bc-b244-21a8ea11382c).

If you want to build the project yourself in VisualStudio make sure you have **Universal Windows Platform development** installed. More info [here](https://docs.microsoft.com/en-us/windows/apps/windows-app-sdk/set-up-your-development-environment?tabs=vs-2022-17-1-a%2Cvs-2022-17-1-b).

All other packages and dependencies should be already setup for you.

## How To Use

Currently the only fully supported language is *Kotlin*. On startup and later in settings you can provide path to Kotlin compiler. Please provide an absolute path to the compiler otherwise the script will not be executed. The folder should contain `kotlinc.bat` file and `kotlinc` file.

> Example path could be *C:\Program Files\JetBrains\Kotlin\bin*

The GUI is pretty straight forward. At the top we have two tabs. First is for *Kotlin* development and the other for *Swift* development.
> Note that only only *Kotlin* development is fully supported right now.
The tab contains few basic controls for running the script, uploading script from file and saving current script into file. At the right we can see settings button where we can modify the path to the script compiler.

Below there is pane to write your script into. Below that there is an output window.

## What has been done
List of required functionality: 
- [x] Should have an editor pane and an output pane.
- [x] Write the script to a file and run it using `/usr/bin/env swift foo.swift`, or `kotlinc -script foo.kts` respectively.
- [x] Assume the script might run for a long time.
- [x] Show live output of the script as it executes.


## What could be improved

## What would I do differently