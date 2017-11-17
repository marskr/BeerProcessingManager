# BeerProcessingManager

Application implemented in C# language, .Net platform, Xamarin cross-platform development software. 
The app, so-called "Beer Processing Manager", was created for automation of beer production processes: their control by viewing results and reaction in case of any danger event.

The BPM was encapsulated and devided for multiple parts:
1. The Thingspeak Manager - part that allows user establish connection between Thingspeak web API and Xamarin mobile application,
2. The Plot Manager - part that enable application to drow basic plots and charts using data received from Thing Speak,
3. The Serialization Manager - this part provides tool to serialize and deserialize application settings,
4. The Log Manager - all error and warning logs will be set in special text file,
5. The Main Activity .cs file integrates work of an app and all modules.

This mobile app was designed in case of realization a simple-use and complete project, that would make the monitoring of beer production process easy. 

The project schema (main code files and directiories):
-> LogManagement
  - LogManager.cs
-> MainActivityResources
  - GenericFragmentPagerAdaptor.cs
  - GenericViewPagerFragment.cs
-> PlotManagement
  - PlotManager.cs
-> Resources
  -> Layout
    - Basic.axml
    - ListVwShowData.axml
    - Main.axml
    - ModifyProcessing.axml
    - ModifyValve.axml
    - ShowCharts.axml
    - ShowData.axml
-> ThingspeakManagement
  - ThingspeakManager.cs
  - VwAdapter.cs
