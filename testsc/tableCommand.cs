using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace testsc
{
    class tableCommand : CoreCommand
    {
        public override string Help(params string[] help_args)
        {
            return "Build table from input";
        }
        public override void Run()
        {
            myTable table = new myTable();
            if(last_message.Equals(""))
            {
                //ConsoleWrite("tableCommand: Run(): no data");
                return;
            }
            table.Init(last_message);
            ConsoleWrite(table.Build());
        }
    }
    class myTable
    {
        public int Cols { set; get; }
        public int Rows { set; get; }
        public int[] ColumnsWidth;
        private string[] _lines;
        public void Init(string data)
        {
            this.Rows = 1;
            this.Cols = 1;
            if (data.Length == 0)
                return;
            this._lines = Core.TagCommandlineChars(data).Split('\n');            
            this.Rows = _lines.Length;
            this.Cols = _lines.Select(line => line.Split(';').Length).Max();
            this.ColumnsWidth = new int[this.Cols];

            foreach (string line in _lines)
            {
                string[] cols = line.Split(';');
                for (int i = 0; i < cols.Length; i++) 
                {
                    if (RealLength(cols[i]) > this.ColumnsWidth[i]) this.ColumnsWidth[i] = RealLength(cols[i]);
                }
            }
        }
        public string Build()
        {
            if (this.ColumnsWidth == null) return "tableCommand: Build(): no columns";
            List<string> output = new List<string>();
            string header = "┌" + new string('─', this.ColumnsWidth.Sum() + this.ColumnsWidth.Length - 1) + "┐";
            string footer = "└" + new string('─', this.ColumnsWidth.Sum() + this.ColumnsWidth.Length - 1) + "┘";

            int pos = 2;
            for (int j = 0; j < this.ColumnsWidth.Length; j++) 
            {
                pos += j < this.ColumnsWidth.Length-1 ? this.ColumnsWidth[j] : 0;
                if (pos == 2) continue;
                header = header.Substring(0, pos-1) + "┬" + header.Substring(pos);
                footer = footer.Substring(0, pos - 1) + "┴" + footer.Substring(pos);
                if (j < this.ColumnsWidth.Length - 2)
                    pos++;
            }
            output.Add(header);
            for (int j = 0; j < Rows; j++)
            {
                string sRow = "│";
                for (int i = 0; i < Cols; i++)
                {
                    string value = readCell(i, j);
                    sRow += value;
                    sRow += new string(' ', this.ColumnsWidth[i] - RealLength(value));
                    sRow += "│";
                }
                output.Add(sRow);
            }
            output.Add(footer);

            // if lines are too long trim using consolewidth
            output = output.Select(line => line.Substring(0, Math.Min(line.Length, Console.WindowWidth))).ToList();
            return string.Join("\n", output);
        }
        public int RealLength(string text)
        {
            string tmp = text;
            if(tmp.Contains("\u001b"))
                tmp = Regex.Replace(tmp, "\u001b\\[.+?m", "");
            return tmp.Length;
        }
        public string readCell(int j,int i)
        {
            string[] cols = _lines[i].Split(';');
            if (j < cols.Length) return Core.UntagCommandlineChars(cols[j]);
            return "";
        }
    }
}