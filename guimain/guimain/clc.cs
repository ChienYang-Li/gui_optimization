using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace guimain
{
    public struct pos
    {
        public pos(int y, int x)
        {
            this.x = x;
            this.y = y;
        }

        public int x { get; set; }
        public int y { get; set; }
    }

    public class ccl
    {
        public int maxcc;
        public int[,] labelarray;
        public Dictionary<int, Label> labels;
        public Dictionary<int, List<pos>> ccpos;

        public int findmaxcc(byte[,] array, int leftx, int lefty, int rightx, int righty)
        {

            int labelcnt = 1, currentLabel;
            labelarray = new int[array.GetLength(0), array.GetLength(1)];
            //int[,] labelarray = new int[array.GetLength(0), array.GetLength(1)];
            labels = new Dictionary<int, Label>();

            for (int i = lefty; i < righty; ++i)
            {
                for (int j = leftx; j < rightx; ++j)
                {
                    if (array[i, j] == 0)
                        continue;

                    List<int> neighboringlabels = new List<int>();
                    for (int k = i - 1; k <= i + 1; ++k)
                    {
                        for (int f = j - 1; f <= j + 1; ++f)
                        {
                            if (k >= 0 && k < array.GetLength(0) && f >= 0 && f < array.GetLength(1))
                            {
                                if (labelarray[k, f] != 0)
                                    neighboringlabels.Add(labelarray[k, f]);
                            }
                        }
                    }

                    if (!neighboringlabels.Any())
                    {
                        labelarray[i, j] = labelcnt;
                        labels.Add(labelcnt, new Label(labelcnt));
                        ++labels[labelcnt].num;
                        labelcnt++;
                    }
                    else
                    {
                        currentLabel = neighboringlabels.Min(n => labels[n].GetRoot().Name);
                        Label root = labels[currentLabel].GetRoot();

                        foreach (int neighbor in neighboringlabels)
                        {
                            if (root.Name != labels[neighbor].GetRoot().Name)
                            {
                                labels[neighbor].Join(labels[currentLabel]);
                            }
                        }
                        labelarray[i, j] = currentLabel;
                        ++labels[currentLabel].num;
                    }
                }
            }

            maxcc = 0;
            foreach (int key in labels.Keys)
            {
                if (labels[key].num > maxcc)
                    maxcc = labels[key].num;
            }
            return maxcc;
        }

        public Dictionary<int, List<pos>> findccpos(int leftx, int lefty, int rightx, int righty)
        {
            ccpos = new Dictionary<int, List<pos>>();

            for (int i = lefty; i < righty; i++)
            {
                for (int j = leftx; j < rightx; j++)
                {
                    int patternNumber = labelarray[i, j];

                    if (patternNumber != 0)
                    {
                        patternNumber = labels[patternNumber].GetRoot().Name;

                        if (!ccpos.ContainsKey(patternNumber))
                        {
                            ccpos[patternNumber] = new List<pos>();
                        }

                        ccpos[patternNumber].Add(new pos(i, j));
                    }
                }
            }

            return ccpos;
        }

    }

    public class Label
    {
        public int Name;

        public Label Root;

        public int Rank;

        public int num;

        public Label(int Name)
        {
            this.Name = Name;
            this.Root = this;
            this.Rank = 0;
            this.num = 0;
        }
        internal Label GetRoot()
        {
            if (this.Root != this)
            {
                this.Root = this.Root.GetRoot();//Compact tree
            }

            return this.Root;
        }

        internal void Join(Label root2)
        {
            if (root2.Rank < this.Rank)//is the rank of Root2 less than that of Root1 ?
            {
                root2.Root = this;//yes! then Root1 is the parent of Root2 (since it has the higher rank)
                this.num += root2.num;
            }
            else //rank of Root2 is greater than or equal to that of Root1
            {
                this.Root = root2;//make Root2 the parent
                root2.num += this.num;

                if (this.Rank == root2.Rank)//both ranks are equal ?
                {
                    root2.Rank++;//increment Root2, we need to reach a single root for the whole tree
                }
            }
        }
    }
}
