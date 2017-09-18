using System;
using System.Collections.Generic;

using System.Linq;

using MetaheuristicsLibrary;

namespace FrOG
{
    public class Solvers : ISolver
    {
        /// <summary>
        /// Variable vector of final solution.
        /// </summary>
        public double[] Xopt { get; private set; }
        /// <summary>
        /// Cost of final solution.
        /// </summary>
        public double Fxopt { get; private set; }

        //public Dictionary<string, string> settings = new Dictionary<string, string>();

        private readonly Dictionary<string, Dictionary<string, double>> _presets = new Dictionary<string, Dictionary<string, double>>();

        public Solvers()
        {
            //Prepare settings
            var standardSettings = new Dictionary<string, double>
            {
                { "seed", 1},
                { "stepsize", 0.1},
                //The Froginterface is in charge of the number of iterations.
                //If the solvers requires this parameters, I would use a high number.
                { "itermax", 1000} 
            };

            var SGA_Settings = new Dictionary<string, double>{
                {"maxgen", 200},
                {"popsize", 30},
                { "itermax", 1000},
                { "seed", 1}
            };

            _presets.Add("Hillclimber", standardSettings);
            _presets.Add("SimpleGA", SGA_Settings);
        }

        public bool RunSolver(List<Variable> variables, Func<IList<decimal>, double> evaluate, string preset, string expertsettings, string installFolder, string documentPath)
        {
            var settings = _presets[preset];

            //System.Windows.Forms.MessageBox.Show(expertsettings);     //use expertsettings to input custom solver parameters

            var dvar = variables.Count;
            var lb = new double[dvar];
            var ub = new double[dvar];
            var integer = new bool[dvar];

            for (var i = 0; i < dvar; i++)
            {
                lb[i] = Convert.ToDouble(variables[i].LowerB);
                ub[i] = Convert.ToDouble(variables[i].UpperB);
                integer[i] = variables[i].Integer;
            }

            Func<double[], double> eval = x =>
            {
                var decis = x.Select(Convert.ToDecimal).ToList();
                return evaluate(decis);
            };

            try
            {
                if (preset.Equals("SimpleGA"))
                {
                    Dictionary<string, object> GAsettings = new Dictionary<string, object>();
                    GAsettings.Add("maxgen", (int)settings["maxgen"]);
                    GAsettings.Add("popsize", (int)settings["popsize"]);
                    int seed = (int)settings["seed"];
                    int itermax = (int)settings["itermax"];




                    var ga = new MetaheuristicsLibrary.SolversSO.SimpleGA(lb, ub, integer, itermax, eval, seed, GAsettings);
                    ga.solve();
                    Xopt = ga.get_Xoptimum();
                    Fxopt = ga.get_fxoptimum();
                }
                else
                {
                    var seed = (int)settings["seed"];
                    var stepsize = settings["stepsize"];
                    var itermax = (int)settings["itermax"];
                    var hc = new Hillclimber(lb, ub, stepsize, itermax, eval, seed);
                    hc.Solve();
                    Xopt = hc.get_Xoptimum();
                    Fxopt = hc.get_fxoptimum();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        public string GetErrorMessage()
        {
            return "";
        }

        /// <summary>
        /// Get the variable vector of the final solution.
        /// </summary>
        /// <returns>Variable vector.</returns>
        public double[] get_Xoptimum()
        {
            return Xopt;
        }

        public IEnumerable<string> GetPresetNames()
        {
            return _presets.Keys;
        }
    }
}
