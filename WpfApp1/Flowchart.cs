﻿using System;
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
    //***************** Flow Chart Class*************
    // The chart is a List<List<List<T>>>.
    // The First List is for columns which contains objects for each column in the diagram
    // The Second inside list is the list that contains Objects for each column.
    // The last list contains object name as its first element and all objects are connect to 
    // the first element.
    // for better picture here is the example of the chart is look like (Same example as the Home Project)


    // A,F1   F1,C   C,F4     F4,W   W
    // B,F1   F2,D   D,F4,F3  F3,H   H
    // E,F2          R,F3
    // G,F2
    // T,F2

    //*************************************************
    public class Flowchart :INotifyPropertyChanged
    {
        //We make some separate lists for keeping Inputs, OutOuts and Functions.
        //*************************************************
        List<string> functions = new List<string>();
        List<string> outputs = new List<string>();
        List<List<string>> inputs = new List<List<string>>();
        //*************************************************

        //This is the list we keep all our Entries into it. 
        public List<List<string>> allFuncs = new List<List<string>>();
        // Class Constructor.
        public Flowchart() {
            _chart = new List<List<List<string>>>();
        }

        // Finding if the Flowchart has Cycle in it or not.
        // This function is using this fact that the current chart we have does not have
        // Cycle so we just need to check the output that user just entered.
        // so give the entry to the chart and we got some outputs
        // then again we give these new outputs as new inputs to the chart
        // we continue this process recursively unless we find  
        // any of our outputs is the same with the current user output.
        // which means we start from one object and after some steps we got back to the same point.
        private bool isCycle(List<string> inputs,string firstInput)
        {
            // if the chart does not have any cycle. Eventually we will get to the point that our new input list does not
            // have any output which means we get to the end of flowchart. So if we get to this point we can say 
            // the flowchart will not have cycle if we add the user entries.
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
                // check if any of our outputs are the same with our first entry.
                if (vs.Contains(firstInput))
                {
                    return true;
                }
                // if the code get here it returns itself to repeat the process.
                return isCycle(vs, firstInput);
            }
        }

        int serialCounter = 1;
        public void Add(List<string> input, string output, string fName)
        {
            string serialNumber = fName + "-" +Convert.ToString(serialCounter);
            serialCounter++;
            // check inputs and output have duplicate(s).
            for (int r = 0; r <= input.Count - 1; r++)
            {
                if (input[r].Equals(output))
                {
                    Warning = "Inputs and output have duplicate(s)!";
                    return;
                }
            }

            //Check if inputs have duplicates.
            for (int r = 0; r <= input.Count - 1; r++)
            {
                for (int s = 0; s <= input.Count - 1; s++)
                {
                    if (input[r].Equals(input[s]) && r!=s)
                    {
                        Warning = "Inputs have duplicate(s)!";
                        return;
                    }
                }
            }
            // we add Inputs, Function Name and output to the list to keep it as a object in allFuncs list.
            // Example: for inputs: A,B,C,D output: E and function: F1, the f is like this:
            // A B C D F1 E
            List<string> f = new List<string>(input)
                {
                    fName,
                    output,
                    serialNumber
                };
            // 
            allFuncs.Add(f);
            List<string> a = new List<string>();

            // We add the "user entry for output" to the list and then we call the cycle function
            // with that list and also "user entry for output" as a second paratmeter.
            a.Add(output);
            if (isCycle(a, output))
            {
                allFuncs.Remove(allFuncs.Last());
                Warning = "There is a Loop!";
                return;
            }
            //adjustment is the list that has the index adjustments in it.
            // It helps us in inserting new user inputs to our current chart.
            // we have the number of inputs of 0's and then 2 and 1 for output and function name
            //Because in the drawing their order is INPUTS ----> Function Name -----> OOUTPUT
            var adjustements = new List<double>(new double[input.Count]);
            adjustements.Add(2);
            adjustements.Add(1);
            //Stack number that we find the inserting object in it.
            int currentStackNumber = 0;
            // Maximum of stack numbers we have found by now.
            int maxStackNumber = 0;
            //int cntr = 0;
            bool firstTimeFound = false;
            Warning = "";
            //**********Making Functions, Inputs and output.*************************
            functions.Add(fName);
            if (!outputs.Contains(output)) outputs.Add(output);
            inputs.Add(input);
            //**************************************************
     
            // we make a list of current entry in orderof Inputs, Output, Function Name
            // we actually make a loop into this list for our calculation.
            List<string> function = new List<string>(input)
            {
                    output,
                    fName,
                    serialNumber
            };
            //Starting of Insertation into the chart.
            for (int i = 0; i <= function.Count-2;i++)
                {
                    chart.Add(new List<List<string>>());
                    bool isFound = false;
                // Inserting of Function Name
                if (i == function.Count - 2)
                {
                    //The index of Function number column is maxstacknumber + 1;
                    chart[maxStackNumber + 1].Add(new List<string>() { function[function.Count -1], function[function.Count - 3], });
                    //after inserting function name, we check if previous Entries(inputs and output) 
                    // has been found in the chart or not. If answer is false so we will add them all.
                    if (!firstTimeFound)
                    {
                        for (int l = 0; l < i; l++)
                        {
                            // Output
                            if (l == i - 1) chart[maxStackNumber + Convert.ToInt32(adjustements[l])].Add(new List<string>() { function[l] });
                            //input
                            else chart[currentStackNumber + Convert.ToInt32(adjustements[l])].Add(new List<string>() { function[l], function.Last() });
                        }
                    }
                }
                    
                    for (int j = 0; j < chart.Count; j+=2)
                    { 
                        for(int k=0; k <= chart[j].Count -1; k++)
                        {
                            // When we find the entries in out current chart.
                            if (chart[j][k][0].Equals(function[i])){
                               //if the object we have found is not the output, we just add its correspandant
                               // function name into its list. for example user has A in his new Entry
                               // and have found A in 2nd column. A,F1,F2. It means A is connected to F1 and
                               // F2 by now. Now we want to insert our new A and its function name is F5 so the
                               // List is like A F1 F2 F5
                                if(i!=function.Count-3 ) chart[j][k].Add(function.Last());
                                // if it is output we check if it is in the first column, if yes 
                                // we need to insert to new columns in index 0 of our chart to insert
                                // our new entries.
                            else
                                {
                                    // If output is found in 0 index and we have not found any 
                                    // input entry in our chart by now.
                                    if (j == 0 && !firstTimeFound)
                                    {
                                        chart.Insert(0, new List<List<string>>());
                                        chart.Insert(0, new List<List<string>>());
                                    }
                                }
                                // after adding the current item, we check if previous items has been found or not
                                // if the answer is false so we have to insert them. The reason for doing it now is
                                // we have not known the stacknumber by now. 
                                // The value of !firstTimeFound is false unless we found any of our entries in our current
                                // chart. As soon As we find the first one we have Stacknumber and actually we know where should
                                // we insert our graph.
                                if (!firstTimeFound)
                                {
                                    for(int l = 0; l < i; l++)
                                    {
                                        chart[j - Convert.ToInt32(adjustements[i])].Add(new List<string>() { function[l], function.Last() });
                                    }
                                }
                                // Giving value to stacknumber, we have consider adjusments here.
                                // becuase based on the found entry is input, output or function name
                                // Inserting of others are different.
                                currentStackNumber = j - Convert.ToInt32(adjustements[i]);
                                // Making maxStackNumber
                                if (currentStackNumber > maxStackNumber) maxStackNumber = currentStackNumber;
                                //we have found an object so it is time to make firstTimeFound true.
                                firstTimeFound = true;
                                isFound = true;
                                break;
                            }
                        }
                        // after finding the element in the current chart we can just go for next one.
                        if (isFound) break;
                    }
                // If we did not find the entry in the current chart and also it is the same situation
                // with all of our previous entries. We just continue for next items.
                if (!isFound && !firstTimeFound) continue;
                // If we did not find the entry but one of the previous entries is found so now we have the Currentstacknumber or
                // maxstacknumber so we insert the new object based on that index.
                else if (!isFound && firstTimeFound)
                {
                    /// if entry is input
                    if (i <= function.Count - 4) chart[maxStackNumber].Add(new List<string>() { function[i], function.Last() });
                    /// if entry is output
                    else if (i == function.Count - 3) chart[maxStackNumber + 2].Add(new List<string>() { function[i] });
                }
                }
            
        }
        // This is the Warning Property
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
        //This is our chart.
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
