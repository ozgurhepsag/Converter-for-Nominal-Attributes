using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using weka.core.converters;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private static weka.core.Instances insts;

        public Form1()
        {
            InitializeComponent();
            this.browseFileBtn.Click += BrowseFileBtn_Click;
        }

        private void BrowseFileBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse Data Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "arff",
                Filter = "arff files (*.arff)|*.arff",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                browseFileName.Text = openFileDialog1.FileName;
                insts = new weka.core.Instances(new java.io.FileReader(browseFileName.Text));
                PrepareDataset();
            }
        }

        public void PrepareDataset()
        {
            List<bool> isNumeric = new List<bool>();
            List<bool> isMultiCategorigal = new List<bool>();
            List<List<string>> numericColumns = new List<List<string>>();

            for (int i = 0; i < insts.numAttributes(); i++)
            {
                bool isNum = insts.attribute(i).isNumeric();
                isNumeric.Add(isNum);

                if(isNum == true)
                {
                    numericColumns.Add(new List<string>());

                    for (int j = 0; j < insts.numInstances(); j++)
                    {
                        //Console.WriteLine(insts.instance(j).toString(i));
                        numericColumns[numericColumns.Count - 1].Add(insts.instance(j).toString(i));
                    }
                }
            }

            weka.filters.unsupervised.attribute.Discretize myDiscretize = new weka.filters.unsupervised.attribute.Discretize();
            myDiscretize.setInputFormat(insts);
            myDiscretize.setFindNumBins(true);
            insts = weka.filters.Filter.useFilter(insts, myDiscretize);

            List<List<string>> atrs = new List<List<string>>();

            for (int i = 0; i < insts.numAttributes(); i++)
            {
                atrs.Add(new List<string>());
                for (int j = 0; j < insts.attribute(i).numValues(); j++)
                {
                    string sub_category = insts.attribute(i).value(j);
                    string temp = sub_category.Replace("'", string.Empty);
                    atrs[atrs.Count - 1].Add(temp);
                    //Console.Write(sub_category);
                }

                if(atrs[atrs.Count - 1].Count == 2)
                {
                    isMultiCategorigal.Add(true);
                }
                else
                {
                    isMultiCategorigal.Add(false);
                }
                //Console.WriteLine(atrs[atrs.Count - 1].Count);
            }

            List<List<string>> lst = new List<List<string>>();

            for (int i = 0; i < insts.numInstances(); i++)
            {
                lst.Add(new List<string>());

                for (int j = 0; j < insts.instance(i).numValues(); j++)
                {
                    string temp = insts.instance(i).toString(j);
                    temp = temp.Replace("\\", string.Empty);
                    temp = temp.Replace("'", string.Empty);
                    lst[lst.Count - 1].Add(temp);
                
                    //Console.Write(temp + " ");
                }
                //Console.WriteLine();
            }

            List<List<string>> giniDataset = ConvertToNumericWithGini(lst, atrs);
            giniDataset = ChangeBackNumericalColumns(giniDataset);
            giniDataset = Arrange2CategoricalColumns(giniDataset);
            List <List<string>> twoingDataset = ConvertToNumericWithTwoing(lst, atrs);
            twoingDataset = ChangeBackNumericalColumns(twoingDataset);
            twoingDataset = Arrange2CategoricalColumns(twoingDataset);
        }

        public List<List<string>> ConvertToNumericWithGini(List<List<string>> lst, List<List<string>> atrs)
        {
            List<List<string>> numericDataset = new List<List<string>>();
            List<string> targetCol = new List<string>();
            int targetAtrNum = atrs[atrs.Count - 1].Count;
            List<string> targetAtrs = atrs[atrs.Count - 1];
            int totalInst = lst.Count;
            int columnNum = lst[0].Count;

            for (int i = 0; i < columnNum - 1; i++) 
            {
                List<string> giniScores = new List<string>();
                int atrNum = atrs[i].Count;
                List<string> columnAtrs = atrs[i];
                int[,] allCounts = new int[targetAtrNum, atrNum * 2];

                for (int j = 0; j < totalInst; j++) // fill the table
                {
                    int k = targetAtrs.IndexOf(lst[j][columnNum - 1]);
                    int l = columnAtrs.IndexOf(lst[j][i]) * 2;
                    allCounts[k, l]++;
                    for (int x = 1; x < atrNum * 2; x = x + 2)
                    {
                        if(x != l + 1)
                            allCounts[k, x]++;
                    }
                    targetCol.Add(lst[j][columnNum - 1]);
                }

                for (int j = 0; j < totalInst; j++) // calculate gini from table
                {
                    int l = columnAtrs.IndexOf(lst[j][i]) * 2;

                    int denominatorL = 0;
                    int denominatorR = 0;

                    for (int x = 0; x < allCounts.GetLength(0); x++)
                    {
                        denominatorL = allCounts[x,l] + denominatorL;
                    }
                    denominatorR = totalInst - denominatorL;

                    double giniL = 1.0;
                    for (int x = 0; x < allCounts.GetLength(0); x++)
                    {
                        giniL = giniL - Math.Pow(Convert.ToDouble(allCounts[x, l]) / Convert.ToDouble(denominatorL), 2.0);
                    }

                    double giniR = 1.0;
                    for (int x = 0; x < allCounts.GetLength(0); x++)
                    {
                        giniR = giniR - Math.Pow(Convert.ToDouble(allCounts[x, l + 1]) / Convert.ToDouble(denominatorR), 2.0);
                    }

                    double gini = (giniR * denominatorR + giniL * denominatorL) / totalInst;
                    giniScores.Add(gini.ToString());
                }

                numericDataset.Add(giniScores);
            }

            numericDataset.Add(targetCol);

            return numericDataset;
        }

        public List<List<string>> ConvertToNumericWithTwoing(List<List<string>> lst, List<List<string>> atrs)
        {
            List<List<string>> numericDataset = new List<List<string>>();
            List<string> targetCol = new List<string>();
            int targetAtrNum = atrs[atrs.Count - 1].Count;
            List<string> targetAtrs = atrs[atrs.Count - 1];
            int totalInst = lst.Count;
            int columnNum = lst[0].Count;

            for (int i = 0; i < columnNum - 1; i++) 
            {
                int atrNum = atrs[i].Count;
                int[] table = new int[atrNum + (atrs[i].Count * targetAtrNum)];
                int[] atrCount = new int[atrNum];
                int[] targetCount = new int[targetAtrNum];
                List<string> columnAtrs = atrs[i];
                List<string> twoingScores = new List<string>();

                for (int j = 0; j < totalInst; j++) // fill the table
                {
                    int k = targetAtrs.IndexOf(lst[j][columnNum - 1]);
                    int l = columnAtrs.IndexOf(lst[j][i]);

                    table[l * (targetAtrNum + 1)]++;
                    table[(l * (targetAtrNum + 1)) + (k + 1)]++;
                    targetCount[k]++;
                    atrCount[l]++;

                    targetCol.Add(lst[j][columnNum - 1]);
                }

                for (int j = 0; j < totalInst; j++) // calculate twoing
                {
                    int k = targetAtrs.IndexOf(lst[j][columnNum - 1]);
                    int l = columnAtrs.IndexOf(lst[j][i]);

                    double pLeft = Convert.ToDouble(table[l * (targetAtrNum + 1)]) / Convert.ToDouble(totalInst);
                    double pRight = Convert.ToDouble((totalInst - table[l * (targetAtrNum + 1)])) / Convert.ToDouble(totalInst);

                    double total = 0;

                    for (int x = 1; x < targetAtrNum  + 1; x++)
                    {
                        double pTLeft = Convert.ToDouble(table[l * (targetAtrNum + 1) + x] / Convert.ToDouble(table[l * (targetAtrNum + 1)]));
                        double pTRight = Convert.ToDouble(targetCount[k] - table[l * (targetAtrNum + 1) + x]) / Convert.ToDouble((totalInst - table[l * (targetAtrNum + 1)]));
                        total = total + Math.Abs(pTLeft - pTRight);
                            
                    }

                    double q = 2 * pLeft * pRight * total;
                    twoingScores.Add(q.ToString());
                }

                numericDataset.Add(twoingScores);
            }

            numericDataset.Add(targetCol);

            return numericDataset;
        }

        public List<List<string>> ChangeBackNumericalColumns(List<List<string>> numericDataset)
        {
            return numericDataset;
        }

        public List<List<string>> Arrange2CategoricalColumns(List<List<string>> numericDataset)
        {
            return numericDataset;
        }

        public void WriteFile()
        {

        }
    }
}
