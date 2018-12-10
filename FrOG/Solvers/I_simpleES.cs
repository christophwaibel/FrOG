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

namespace FrOG.Solvers
{
    public class I_simpleES : ISolver
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

        public I_simpleES()
        {

            //Prepare settings
            var ES_Settings_untuned = new Dictionary<string, double>{
                { "itermax", 10000},
                { "seed", 1},
                {"popsize", 20},
                {"lambda", 5},          //offspring
                {"roh", 5},             //mixing nr.
                {"x0sampling", 0},      // 0 = uniform, 1 = gaussian
                {"stepsize0",  0.1}, //initial stepsize s0
                {"stepsize",  0.1},   //stepsize s
                {"tauc", 1.5},      //learning rate tau
                {"selmode", 1},         //mating selection mode. 0 = random, 1 = tournament
                {"pmut_int", 0.5}       //mutation probability, only for integer
            };


            var ES_Settings_n4_A = new Dictionary<string, double>{
                { "itermax", 10000},
                { "seed", 1},
                {"popsize", 25},
                {"lambda", 5},          //offspring
                {"roh", 4},             //mixing nr.
                {"x0sampling", 1},      // 0 = uniform, 1 = gaussian
                {"stepsize0",  9.82254}, //initial stepsize s0
                {"stepsize",  0.0946},   //stepsize s
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

            var ES_Settings_n10_A = new Dictionary<string, double>{
                { "itermax", 10000},
                { "seed", 1},
                {"popsize", 13},
                {"lambda", 7},          //offspring
                {"roh", 8},             //mixing nr.
                {"x0sampling", 0},      // 0 = uniform, 1 = gaussian
                {"stepsize0", 9.78386		}, //initial stepsize s0
                {"stepsize", 0.13504	},   //stepsize s
                {"tauc", 1.81098},      //learning rate tau
                {"selmode", 1},         //mating selection mode. 0 = random, 1 = tournament
                {"pmut_int", 0.5}       //mutation probability, only for integer
            };

            var ES_Settings_n10_B = new Dictionary<string, double>{
                { "itermax", 10000},
                { "seed", 1},
                {"popsize", 20},
                {"lambda", 6},          //offspring
                {"roh", 6},             //mixing nr.
                {"x0sampling", 0},      // 0 = uniform, 1 = gaussian
                {"stepsize0", 0.01		}, //initial stepsize s0
                {"stepsize", 0.97151},   //stepsize s
                {"tauc", 1.84719},      //learning rate tau
                {"selmode", 1},         //mating selection mode. 0 = random, 1 = tournament
                {"pmut_int", 0.5}       //mutation probability, only for integer
            };

            var ES_Settings_n11_A = new Dictionary<string, double>{
                { "itermax", 10000},
                { "seed", 1},
                {"popsize", 11},
                {"lambda", 6},          //offspring
                {"roh", 7},             //mixing nr.
                {"x0sampling", 0},      // 0 = uniform, 1 = gaussian
                {"stepsize0", 10	}, //initial stepsize s0
                {"stepsize", 0.58445	},   //stepsize s
                {"tauc", 1.75398},      //learning rate tau
                {"selmode", 1},         //mating selection mode. 0 = random, 1 = tournament
                {"pmut_int", 0.5}       //mutation probability, only for integer
            };

            var ES_Settings_n11_B = new Dictionary<string, double>{
                { "itermax", 10000},
                { "seed", 1},
                {"popsize", 2},
                {"lambda", 1},          //offspring
                {"roh", 1},             //mixing nr.
                {"x0sampling", 0},      // 0 = uniform, 1 = gaussian
                {"stepsize0", 0.01	}, //initial stepsize s0
                {"stepsize", 0.09993	},   //stepsize s
                {"tauc", 0.01},      //learning rate tau
                {"selmode", 1},         //mating selection mode. 0 = random, 1 = tournament
                {"pmut_int", 0.5}       //mutation probability, only for integer
            };

            var ES_Settings_n13_A = new Dictionary<string, double>{
                { "itermax", 10000},
                { "seed", 1},
                {"popsize", 3},
                {"lambda", 1},          //offspring
                {"roh", 2},             //mixing nr.
                {"x0sampling", 0},      // 0 = uniform, 1 = gaussian
                {"stepsize0", 10}, //initial stepsize s0
                {"stepsize", 1.83897},   //stepsize s
                {"tauc", 0.01},      //learning rate tau
                {"selmode", 1},         //mating selection mode. 0 = random, 1 = tournament
                {"pmut_int", 0.5}       //mutation probability, only for integer
            };

            var ES_Settings_n13_B = new Dictionary<string, double>{
                { "itermax", 10000},
                { "seed", 1},
                {"popsize", 16},
                {"lambda", 6},          //offspring
                {"roh", 7},             //mixing nr.
                {"x0sampling", 0},      // 0 = uniform, 1 = gaussian
                {"stepsize0", 0.01}, //initial stepsize s0
                {"stepsize", 0.41785},   //stepsize s
                {"tauc", 1.53637},      //learning rate tau
                {"selmode", 1},         //mating selection mode. 0 = random, 1 = tournament
                {"pmut_int", 0.5}       //mutation probability, only for integer
            };

            var ES_Settings_n18_A = new Dictionary<string, double>{
                { "itermax", 10000},
                { "seed", 1},
                {"popsize", 14},
                {"lambda", 11},          //offspring
                {"roh", 12},             //mixing nr.
                {"x0sampling", 0},      // 0 = uniform, 1 = gaussian
                {"stepsize0", 9.96262		}, //initial stepsize s0
                {"stepsize", 0.64298	},   //stepsize s
                {"tauc", 1.72539},      //learning rate tau
                {"selmode", 1},         //mating selection mode. 0 = random, 1 = tournament
                {"pmut_int", 0.5}       //mutation probability, only for integer
            };

            var ES_Settings_n18_B = new Dictionary<string, double>{
                { "itermax", 10000},
                { "seed", 1},
                {"popsize", 2},
                {"lambda", 1},          //offspring
                {"roh", 1},             //mixing nr.
                {"x0sampling", 1},      // 0 = uniform, 1 = gaussian
                {"stepsize0", 0.08942		}, //initial stepsize s0
                {"stepsize", 2.49373	},   //stepsize s
                {"tauc", 0.01},      //learning rate tau
                {"selmode", 1},         //mating selection mode. 0 = random, 1 = tournament
                {"pmut_int", 0.5}       //mutation probability, only for integer
            };

            var ES_Settings_n20_A = new Dictionary<string, double>{
                { "itermax", 10000},
                { "seed", 1},
                {"popsize", 23},
                {"lambda", 12},          //offspring
                {"roh", 13},             //mixing nr.
                {"x0sampling", 0},      // 0 = uniform, 1 = gaussian
                {"stepsize0", 0.01	}, //initial stepsize s0
                {"stepsize", 0.24521	},   //stepsize s
                {"tauc", 1.23719},      //learning rate tau
                {"selmode", 1},         //mating selection mode. 0 = random, 1 = tournament
                {"pmut_int", 0.5}       //mutation probability, only for integer
            };

            var ES_Settings_n20_B = new Dictionary<string, double>{
                { "itermax", 10000},
                { "seed", 1},
                {"popsize", 2},
                {"lambda", 1},          //offspring
                {"roh", 1},             //mixing nr.
                {"x0sampling", 0},      // 0 = uniform, 1 = gaussian
                {"stepsize0", 0.13794		}, //initial stepsize s0
                {"stepsize", 0.06444	},   //stepsize s
                {"tauc", 0.01},      //learning rate tau
                {"selmode", 1},         //mating selection mode. 0 = random, 1 = tournament
                {"pmut_int", 0.5}       //mutation probability, only for integer
            };


            var ES_Settings_n35_A = new Dictionary<string, double>{
                { "itermax", 10000},
                { "seed", 1},
                {"popsize", 22},
                {"lambda", 4},          //offspring
                {"roh", 5},             //mixing nr.
                {"x0sampling", 0},      // 0 = uniform, 1 = gaussian
                {"stepsize0", 0.01}, //initial stepsize s0
                {"stepsize", 0.04086	},   //stepsize s
                {"tauc", 0.11277},      //learning rate tau
                {"selmode", 1},         //mating selection mode. 0 = random, 1 = tournament
                {"pmut_int", 0.5}       //mutation probability, only for integer
            };

            var ES_Settings_n35_B = new Dictionary<string, double>{
                { "itermax", 10000},
                { "seed", 1},
                {"popsize", 4},
                {"lambda", 1},          //offspring
                {"roh", 2},             //mixing nr.
                {"x0sampling", 0},      // 0 = uniform, 1 = gaussian
                {"stepsize0", 9.45934	}, //initial stepsize s0
                {"stepsize", 0.15398	},   //stepsize s
                {"tauc", 0.01},      //learning rate tau
                {"selmode", 1},         //mating selection mode. 0 = random, 1 = tournament
                {"pmut_int", 0.5}       //mutation probability, only for integer
            };

            _presets.Add("ES_unt", ES_Settings_untuned);
            _presets.Add("ES_n4_A", ES_Settings_n4_A);
            _presets.Add("ES_n4_B", ES_Settings_n4_B);
            _presets.Add("ES_n10_A", ES_Settings_n10_A);
            _presets.Add("ES_n10_B", ES_Settings_n10_B);
            _presets.Add("ES_n11_A", ES_Settings_n11_A);
            _presets.Add("ES_n11_B", ES_Settings_n11_B);
            _presets.Add("ES_n13_A", ES_Settings_n13_A);
            _presets.Add("ES_n13_B", ES_Settings_n13_B);
            _presets.Add("ES_n18_A", ES_Settings_n18_A);
            _presets.Add("ES_n18_B", ES_Settings_n18_B);
            _presets.Add("ES_n20_A", ES_Settings_n20_A);
            _presets.Add("ES_n20_B", ES_Settings_n20_B);
            _presets.Add("ES_n35_A", ES_Settings_n35_A);
            _presets.Add("ES_n35_B", ES_Settings_n35_B);
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
                if (preset.Equals("ES_n4_A") || preset.Equals("ES_n4_B") ||
                    preset.Equals("ES_n10_A") || preset.Equals("ES_n10_B") ||
                    preset.Equals("ES_n11_A") || preset.Equals("ES_n11_B") ||
                    preset.Equals("ES_n13_A") || preset.Equals("ES_n13_B") ||
                    preset.Equals("ES_n18_A") || preset.Equals("ES_n18_B") ||
                    preset.Equals("ES_n20_A") || preset.Equals("ES_n20_B") ||
                    preset.Equals("ES_n35_A") || preset.Equals("ES_n35_B") ||
                    preset.Equals("ES_unt"))
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
                        Random rnd = new Random();
                        seed = rnd.Next();
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
