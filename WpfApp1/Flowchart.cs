using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Flowchart :INotifyPropertyChanged
    {
        List<string> functions = new List<string>();
        List<string> outputs = new List<string>();
        List<List<string>> inputs = new List<List<string>>();
        public List<List<string>> allFuncs = new List<List<string>>();
        public Flowchart() {
            _chart = new List<List<List<string>>>();
        }

        private bool isCycle(List<string> inputs,string firstInput)
        {
            if (!inputs.Any()) { return false; }
            else
            {
                List<string> vs = new List<string>();
                for (int i = 0; i <= inputs.Count - 1; i++)
                {
                    foreach (var item in allFuncs.Where(s => s.Contains(inputs[i])))
                    {
                        if (!item.Last().Equals(inputs[i]))
                             vs.Add(item.Last());
                    }
                }
                if (vs.Contains(firstInput))
                {
                    return true;
                }
                return isCycle(vs, firstInput);
            }
        }

        public void Add(List<string> input, string output, string fName)
        {
            for(int r=0; r <= input.Count -1; r++)
            {
                if(input[r].Equals(output))
                {
                    Warning = "Inputs and output have duplicates!";
                    return;
                }
            }
            for (int r = 0; r <= input.Count - 1; r++)
            {
                for (int s = 0; s <= input.Count - 1; s++)
                {
                    if (input[r].Equals(input[s]) && r!=s)
                    {
                        Warning = "Inputs have duplicates!";
                        return;
                    }
                }
            }
            List<string> function2 = new List<string>(input)
                {
                    fName,
                    output
                };
            allFuncs.Add(function2);
            List<string> a = new List<string>(); 
            a.Add(output);
            if (isCycle(a, output))
            {
                allFuncs.Remove(allFuncs.Last());
                Warning = "There is a Loop!";
                return;
            }
            var adjustements = new List<double>(new double[input.Count]);
            adjustements.Add(2);
            adjustements.Add(1);
            int currentStackNumber = 0;
            int maxStackNumber = 0;
            bool firstTimeFound = false;
            if (!functions.Contains(fName))
            {
                Warning = "";
                functions.Add(fName);
                if (!outputs.Contains(output)) outputs.Add(output);
                inputs.Add(input);
                List<string> function = new List<string>(input)
                {
                    output,
                    fName
                };
                allFuncs.Add(function2);

                for (int i = 0; i <= function.Count-1;i++)
                {
                    chart.Add(new List<List<string>>());
                    bool isFound = false;
                    if (i == function.Count - 1)
                    {
                        chart[maxStackNumber + 1].Add(new List<string>() { function.Last(), function[function.Count -2] });
                        if (!firstTimeFound)
                        {
                            for (int l = 0; l < i; l++)
                            {
                                if (l == i - 1) chart[maxStackNumber + Convert.ToInt32(adjustements[l])].Add(new List<string>() { function[l] });
                                else chart[currentStackNumber + Convert.ToInt32(adjustements[l])].Add(new List<string>() { function[l], function.Last() });
                            }
                        }
                        continue;
                    }   
                    for (int j = 0; j < chart.Count; j+=2)
                    { 
                        for(int k=0; k<= chart[j].Count -1; k++)
                        {
                            if (chart[j][k][0].Equals(function[i])){
                                if(i!=function.Count-2 ) chart[j][k].Add(function.Last());
                                else
                                {
                                    if (j == 0 && !firstTimeFound)
                                    {
                                        chart.Insert(0, new List<List<string>>());
                                        chart.Insert(0, new List<List<string>>());
                                    }
                                }
                                if (!firstTimeFound)
                                {
                                    for(int l = 0; l < i; l++)
                                    {
                                        chart[j - Convert.ToInt32(adjustements[i])].Add(new List<string>() { function[l], function.Last() });
                                    }
                                }
                                currentStackNumber = j - Convert.ToInt32(adjustements[i]);
                                if (currentStackNumber > maxStackNumber) maxStackNumber = currentStackNumber;
                                firstTimeFound = true;
                                isFound = true;
                                break;
                            }
                        }
                        if (isFound) break;
                    }
                    if (!isFound && !firstTimeFound) continue;
                    else if (!isFound && firstTimeFound)
                    {
                        if (i <= function.Count - 3) chart[currentStackNumber].Add(new List<string>() { function[i], function.Last() });
                        else if (i == function.Count - 2) chart[currentStackNumber + 2].Add(new List<string>() { function[i] });
                    }
                }
            }
            else
            {
                Warning = "This Function is Already Exist!";
            }
        }
        private string _warning;
        public string Warning
        {
            get
            {
                return _warning;
            }
            set
            {
                _warning = value;
                OnPropertyChanged("Warning");
            }
        }
        private List<List<List<string>>> _chart;
        public List<List<List<string>>> chart
        {
            get
            {
                return _chart;
            }
            set
            {
                _chart = value;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
