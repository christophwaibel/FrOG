using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Opt_ES.cs
 * Copyright 2017 Christoph Waibel <chwaibel@student.ethz.ch>
 * 
 * This work is licensed under the GNU GPL license version 3 or later.
*/

namespace FrOG
{
    public class Opt_ES : ISolver
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

        public Opt_ES()
        {
            //Prepare settings
            var ES_Settings_n4_A = new Dictionary<string, double>{
                { "itermax", 10000},
                { "seed", 1},
                {"popsize", 25},
                {"lambda", 5},          //offspring
                {"roh", 4},             //mixing nr.
                {"x0sampling", 1},      // 0 = uniform, 1 = gaussian
                {"stepsize0", 9.82254}, //initial stepsize s0
                {"stepsize", 0.0946},   //stepsize s
                {"tauc", 1.91772},      //learning rate tau
                {"selmode", 1},         //mating selection mode. 0 = random, 1 = tournament
                {"pmut_int", 0.5}       //mutation probability, only for integer
            };

            var ES_Settings_n4_B = new Dictionary<string, double>{
                { "itermax", 10000},
                { "seed", 1},
                {"popsize", 6},
                {"lambda", 4},          //offspring
                {"roh", 4},             //mixing nr.
                {"x0sampling", 0},      // 0 = uniform, 1 = gaussian
                {"stepsize0", 0.40684}, //initial stepsize s0
                {"stepsize", 1.65921},   //stepsize s
                {"tauc", 1.94483},      //learning rate tau
                {"selmode", 1},         //mating selection mode. 0 = random, 1 = tournament
                {"pmut_int", 0.5}       //mutation probability, only for integer
            };

            var ES_Settings_n4_C = new Dictionary<string, double>{
                { "itermax", 10000},
                { "seed", 1},
                {"popsize", 200},
                {"lambda", 194},        //offspring
                {"roh", 1},             //mixing nr.
                {"x0sampling", 1},      // 0 = uniform, 1 = gaussian
                {"stepsize0", 9.57477}, //initial stepsize s0
                {"stepsize", 0.01},   //stepsize s
                {"tauc", 2.92927},      //learning rate tau
                {"selmode", 0},         //mating selection mode. 0 = random, 1 = tournament
                {"pmut_int", 0.5}       //mutation probability, only for integer
            };

            _presets.Add("ES_n4_A", ES_Settings_n4_A);
            _presets.Add("ES_n4_B", ES_Settings_n4_B);
            _presets.Add("ES_n4_C", ES_Settings_n4_C);
        }

        public bool RunSolver(List<Variable> variables, Func<IList<decimal>, double> evaluate, string preset, string expertsettings, string installFolder, string documentPath)
        {
            var settings = _presets[preset];

            //System.Windows.Forms.MessageBox.Show(expertsettings);     //use expertsettings to input custom solver parameters

            int? seedin = null;
            string[] expsets = expertsettings.Split(';');
            foreach (string strexp in expsets)
            {
                string[] stre = strexp.Split('=');
                if (string.Equals(stre[0], "seed"))
                {
                    seedin = Convert.ToInt16(stre[1]);
                }
            }

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
                if (preset.Equals("ES_n4_A") || preset.Equals("ES_n4_B") || preset.Equals("ES_n4_C"))
                {
                    Dictionary<string, object> ESsettings = new Dictionary<string, object>();
                    ESsettings.Add("popsize", (int)settings["popsize"]);
                    ESsettings.Add("lambda", (int)settings["lambda"]);
                    ESsettings.Add("roh", (int)settings["roh"]);
                    ESsettings.Add("x0sampling", (int)settings["x0sampling"]); 
                    ESsettings.Add("stepsize0", settings["stepsize0"]);
                    ESsettings.Add("stepsize", settings["stepsize"]);
                    ESsettings.Add("tauc", settings["tauc"]);
                    ESsettings.Add("selmode", (int)settings["selmode"]);
                    ESsettings.Add("pmut_int", settings["pmut_int"]);
 
                    int seed;
                    if (seedin != null)
                    {
                        seed = Convert.ToInt16(seedin);
                    }
                    else
                    {
                        seed = (int)settings["seed"];
                    }
                    int itermax = (int)settings["itermax"];

                    var es = new MetaheuristicsLibrary.SolversSO.SimpleES(lb, ub, integer, itermax, eval, seed, ESsettings);
                    es.solve();
                    Xopt = es.get_Xoptimum();
                    Fxopt = es.get_fxoptimum();
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
