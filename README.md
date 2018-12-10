FrOG: Framework for Optimization in Grasshopper
<br>Branch now needs C#6
<br><br>Developed by Thomas Wortmann and Akbar Zuardin with contributions by Dimitry Demin and Christoph Waibel.
<br><br>
This project intends to make it easier to link optimization algorithms to Grasshopper (http://www.grasshopper3d.com/).
FrOG handles all interactions with Grasshopper and provides a GUI. 
<br><br><br>
How to use the component: 
<br>
- Copy the [FrOG.gha](https://github.com/christophwaibel/FrOG/blob/VS2012/FrOG/bin/FrOG.gha) and the [MetaheuristicsLibrary.dll](https://github.com/christophwaibel/FrOG/blob/VS2012/FrOG/bin/MetaheuristicsLibrary.dll) into your Grasshopper components folder (open Rhino, enter "GrasshopperFolders", "Components" into your console).
- Start Rhino Grasshopper, find the FrOG component under Params, Util 

<br><br>


How to contribute:


To add a new solver, implement the ISolver interface and add the resulting solver class to GetSolverList, both in SolverInterface.cs.
The solver should appear in the GUI, with different presets of settings listed seperatly.
HillclimberInterface.cs is provided as an example that links Hillclimber.cs to FrOG.

Please feel free to use this code (which comes without any warranties) and to contribute improvements to the Framework.
When using FrOG for academic research, please cite this repository.

Happy optimizing!
