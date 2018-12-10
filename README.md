**FrOG: Framework for Optimization in Grasshopper**
<br><br>What's different in this branch as compared to the main? Nothing much yet! I'm using it to link [this library](https://github.com/christophwaibel/MetaheuristicsLibrary) to Grasshopper.
<br>Solvers implemented:
* Sampler (needs pre-defined input sequence. E.g. useful for sensitivity analysis)
* Simple Genetic Algorithm (SGA)
* Simple Particle Swarm Optimization (PSO)
* Fully Informed PSO (FIPS)
* Simple Evolution Strategy
<br><br>For details regarding the solvers, please have a look at [this thesis](https://www.research-collection.ethz.ch/handle/20.500.11850/307674)
<br><br>
How to use the component: 
<br>
- Copy the [FrOG.gha](https://github.com/christophwaibel/FrOG/blob/VS2012/FrOG/bin/FrOG.gha) and the [MetaheuristicsLibrary.dll](https://github.com/christophwaibel/FrOG/blob/VS2012/FrOG/bin/MetaheuristicsLibrary.dll) into your Grasshopper components folder (open Rhino, enter "GrasshopperFolders", "Components" into your console).
- Start Rhino Grasshopper, find the FrOG component under Params, Util 

<br><br>Updates
* (10.Dec.'18) Branch now needs C#6
* (before) Branch for pre-C#6

<br><br>Developed by Thomas Wortmann and Akbar Zuardin with contributions by Dimitry Demin and Christoph Waibel.
<br><br>
This project intends to make it easier to link optimization algorithms to Grasshopper (http://www.grasshopper3d.com/).
FrOG handles all interactions with Grasshopper and provides a GUI. 
<br><br>


How to contribute:


To add a new solver, implement the ISolver interface and add the resulting solver class to GetSolverList, both in SolverInterface.cs.
The solver should appear in the GUI, with different presets of settings listed seperatly.
HillclimberInterface.cs is provided as an example that links Hillclimber.cs to FrOG.

Please feel free to use this code (which comes without any warranties) and to contribute improvements to the Framework.
When using FrOG for academic research, please cite this repository.

Happy optimizing!
