using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_Auto_Merge
{
    public partial class Form1 : Form
    {
        string[,] GRID = new string[,] {

            {"A", "A", "A", "A", "F", "H", "I"},
            {"I", "B", "B", "C", "F", "G", "I"},
            {"I", "D", "E", "C", "F", "G", "I"}, 
            //new string[]{"1", "2", "3", "4", "5"},
            //new string[]{"0", "0", "0", "0", "7"},
            //new string[]{"1", "1", "2", "2", "7"}, 
            //new string[]{"3", "4", "5", "6", "7"}
        };
         
        public Form1()
        {
            InitializeComponent();

            var lst =  Merge(); 
            var lst2 = Merge2();
        }

        private object Merge2()
        { 
            List<CellRange> cr = new List<CellRange>();

            Func<string, int, int, CellRange> find = new Func<string, int, int, CellRange>((key, row, col) => {
                CellRange cell = null;
                cell = cr.FirstOrDefault(o => o.Value == key && ( 1 >= (col - o.c1) || 1 >= (col - o.c2) ) && (1 >= (row - o.r1) || 1 >= ( row - o.r2)));                
                return cell;
            });

            int colCount = GRID.GetLength(1);
            for (int c = 0; c < colCount; c++)
            {
                string R0 = GRID[0, c];
                string R1 = GRID[1, c];
                string R2 = GRID[2, c];

                CellRange r0 = find(R0, 0, c); 
                if (r0 == null)
                {
                    r0 = new CellRange() { Value = R0, c1 = c, c2 = c, r1 = 0, r2 = 0 };
                    cr.Add(r0);
                }
                else
                {
                    r0.c2 = c;
                }

                CellRange r1 = find(R1, 1, c);
                if (r1 == null && R0 != R1)
                {
                    r1 = new CellRange() { Value = R1, c1 = c, c2 = c, r1 = 1, r2 = 1 };
                    cr.Add(r1);
                }
                else
                {
                    r1 = find(R1, 1, c);
                    if (r1 == null)
                    {
                        r1 = new CellRange() { Value = R1, c1 = c, c2 = c, r1 = 1, r2 = 1 };
                        cr.Add(r1);
                    }
                    r1.c2 = c;
                    if (R0 == R1)
                    {
                        r1.r2 = 1;
                    }
                }

                CellRange r2 = find(R2, 2, c);
                if (r2 == null && R1 != R2)
                {
                    r2 = new CellRange() { Value = R2, c1 = c, c2 = c, r1 = 2, r2 = 2 };
                    cr.Add(r2);
                }
                else
                {
                    r2 = find(R2, 2, c);
                    if (r2 == null)
                    {
                        r2 = new CellRange() { Value = R2, c1 = c, c2 = c, r1 = 2, r2 = 2 };
                        cr.Add(r2);
                    }
                    r2.c2 = c;
                    if (R1 == R2)
                    {
                        r2.r2 = 2;
                    }
                }
            }
            return cr;
        }

        //private void Merge1()
        //{
        //    /*
        //        ["0", "0", "0", "0", "7"],
        //        ["1", "1", "2", "2", "7"], 
        //        ["3", "4", "5", "6", "7"] 
        //     */
        //    int rowCount = GRID.Length;
        //    int colCount = GRID[0].Length;

        //    // Col Max 초기값 셋팅!
        //    int[] rowColMax = new int[rowCount];
        //    for (int loop = 0; loop < rowCount; loop++)
        //    {
        //        rowColMax[loop] = colCount;
        //    }

        //    Dictionary<int, List<int>> Cells = new Dictionary<int, List<int>>();

        //    List<CellRange> CellRanges = new List<CellRange>();

        //    int stRow = 0;
        //    int stCol = 0;

        //    int cnt = 0;
        //    while (cnt < rowCount * colCount) 
        //    {

        //        //todo : cell 값이 같은 목록을 구함 
        //        // 각 Row별 cell목록이 같은 목록을 구한다.
        //        string cellValue = GRID[stRow][stCol];
        //        for (int row = stRow; row < rowCount; row++)
        //        {
        //            if (cellValue != GRID[row][stCol]) break;

        //            cellValue = GRID[row][stCol];
        //            Cells.Add(row, new List<int>());

        //            for (int col = stCol; col < rowColMax[row]; col++)
        //            {
        //                if (cellValue == GRID[row][col])
        //                {
        //                    Cells[row].Add(col);
        //                }
        //                else
        //                {
        //                    if (col < colCount)
        //                    {
        //                        //각 row별 colmax 값 셋팅.
        //                        rowColMax[row] = col;
        //                    }
        //                    break;
        //                }
        //            }
        //        }

        //        // todo : Cells 에는 머지 가능한 Cell목록이 나오므로.
        //        var cellrange = new CellRange() { Value = cellValue, r1 = stRow, c1 = stCol, r2 = stRow, c2 = stCol };
        //        CellRanges.Add(cellrange);
        //        for (int row = 0; row < Cells.Count; row++)
        //        {
        //            var RowCells = Cells.ElementAt(row);
        //            int _colCnt = RowCells.Value.Count;
        //            int _colcnt_NextRow = _colCnt;
        //            if (row + 1 < Cells.Count)
        //            {
        //                _colcnt_NextRow = Cells.ElementAt(row + 1).Value.Count;
        //            }

        //            if (_colCnt == 0) continue;

        //            if (_colCnt == _colcnt_NextRow)
        //            {
        //                cellrange.r2 = RowCells.Key;
        //                cellrange.c2 = RowCells.Value[_colCnt - 1];
        //            }
        //            else
        //            {
        //                if (RowCells.Key == 0)
        //                {
        //                    cellrange.r2 = RowCells.Key;
        //                    cellrange.c2 = RowCells.Value[_colCnt - 1];
        //                }
        //                break;
        //            }
        //        }

        //        Cells.Clear();

        //        if (stRow == 0)
        //        {
        //            if ((stRow + 1) >= rowCount)
        //            {
        //                // Row를 증가시킬 수 없을때
        //                // 우측 이동
        //                stCol = cellrange.c2 + 1;
        //                if (stCol >= rowColMax[stRow])
        //                {
        //                    //행이 감소시켜야 할때임.
        //                    if (stCol < colCount)
        //                    {
        //                        stCol = rowColMax[stRow];

        //                        if (0 <= stRow - 1 && rowColMax[stRow] == rowColMax[stRow - 1])
        //                        {
        //                            // 상위 ColMax와 동일할 경우! 상위로 이동.    
        //                            do
        //                            {
        //                                if (stRow == 0) break;
        //                                if (rowColMax[stRow] != rowColMax[stRow - 1]) break;

        //                                if (rowColMax[stRow] == rowColMax[stRow - 1])
        //                                    stRow--;
        //                            }
        //                            while (true);
        //                            // 하위 ColMax를 모두 초기화
        //                            for (int bRow = stRow; bRow < rowCount; bRow++)
        //                            {
        //                                rowColMax[bRow] = colCount;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            rowColMax[stRow] = colCount;
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                // Row를 증가 시킬 수 있을때
        //                stCol = cellrange.c1; stRow++;
        //            }
        //        }
        //        else if ((stRow + 1) >= rowCount)
        //        {
        //            // Row를 증가시킬 수 없을때
        //            // 우측 이동
        //            stCol = cellrange.c2 + 1;
        //            if (stCol >= rowColMax[stRow])
        //            {
        //                //행이 감소시켜야 할때임.
        //                if (stCol < colCount)
        //                {
        //                    stCol = rowColMax[stRow];
        //                    if (0 <= stRow - 1 && rowColMax[stRow] == rowColMax[stRow - 1])
        //                    {
        //                        // 상위 ColMax와 동일할 경우! 상위로 이동.    
        //                        do
        //                        {
        //                            if (stRow == 0) break;
        //                            if (rowColMax[stRow] != rowColMax[stRow - 1]) break;

        //                            if (rowColMax[stRow] == rowColMax[stRow - 1])
        //                                stRow--; 
        //                        }
        //                        while (true);
        //                        // 하위 ColMax를 모두 초기화
        //                        for (int bRow = stRow; bRow < rowCount; bRow++)
        //                        {
        //                            rowColMax[bRow] = colCount;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        rowColMax[stRow] = colCount;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            // Row를 증가 시킬 수 있을때
        //            stCol = cellrange.c1; stRow++;
        //        }

        //        int idx = cellrange.r2 * colCount + cellrange.c2;
        //        if (idx >= (colCount * rowCount - 1)) break;

        //        cnt++;
        //    }
        //}

        private List<CellRange> Merge()
        {
            int rowCount = GRID.GetLength(0);
            int colCount = GRID.GetLength(1);

            // Col Max 초기값 셋팅!
            int[] rowColMax = new int[rowCount];
            for (int loop = 0; loop < rowCount; loop++)
            {
                rowColMax[loop] = colCount;
            }

            Dictionary<int, List<int>> Cells = new Dictionary<int, List<int>>();

            List<CellRange> range = new List<CellRange>();

            int stRow = 0;
            int stCol = 0;

            int cnt = 0;
            while (stRow < rowCount && stCol < colCount)
            {
                //todo : cell 값이 같은 목록을 구함 
                // 각 Row별 cell목록이 같은 목록을 구한다.  

                string cellValue = "" + GRID[stRow, stCol];// GRID[stRow][stCol];
                for (int row = stRow; row < rowCount; row++)
                {
                    if (GRID[row, stCol] == null || cellValue != ("" + GRID[row, stCol]))//GRID[row][stCol])
                    {
                        rowColMax[row] = 0;
                        continue;
                    }

                    cellValue = "" + GRID[row, stCol];//GRID[row][stCol];
                    Cells.Add(row, new List<int>());

                    for (int col = stCol; col < rowColMax[row]; col++)
                    {
                        if (cellValue == "" + GRID[row, col])//GRID[row][col])
                        {
                            Cells[row].Add(col);
                        }
                        else
                        {
                            if (col < colCount)
                            {
                                //각 row별 colmax 값 셋팅.
                                rowColMax[row] = col;
                            }
                            break;
                        }
                    }
                }

                // todo : Cells 에는 머지 가능한 Cell목록이 나오므로.
                var cellrange = new CellRange() { Value = cellValue, r1 = stRow, c1 = stCol, r2 = stRow, c2 = stCol };
                range.Add(cellrange);
                for (int row = 0; row < Cells.Count; row++)
                {
                    var RowCells = Cells.ElementAt(row);
                    int _colCnt = RowCells.Value.Count;
                    int _colcnt_NextRow = _colCnt;
                    if (row + 1 < Cells.Count)
                    {
                        _colcnt_NextRow = Cells.ElementAt(row + 1).Value.Count;
                    }

                    if (_colCnt == 0) continue;

                    if (_colCnt == _colcnt_NextRow)
                    {
                        cellrange.r2 = RowCells.Key;
                        cellrange.c2 = RowCells.Value[_colCnt - 1];
                    }
                    else
                    {
                        if (RowCells.Key == 0)
                        {
                            cellrange.r2 = RowCells.Key;
                            cellrange.c2 = RowCells.Value[_colCnt - 1];
                        }
                        break;
                    }
                }

                Cells.Clear();

                stRow = cellrange.r1;
                stCol = cellrange.c1;

                int idx = cellrange.r2 * colCount + cellrange.c2;
                if (idx >= (colCount * rowCount - 1)) break;

                if (cellrange.r2 + 1 < rowCount)
                {
                    if (rowColMax[cellrange.r2] > rowColMax[cellrange.r2 + 1])
                    {
                        for (int loop = cellrange.r2; loop < rowCount; loop++) rowColMax[loop] = rowColMax[cellrange.r2];
                        stRow = cellrange.r2 + 1;
                    }
                    else
                    {
                        if (stRow == 0)
                        {
                            stCol = cellrange.c2 + 1;
                            for (int loop = stRow; loop < rowCount; loop++) rowColMax[loop] = colCount;
                        }
                        else
                        {
                            stRow--;
                            stCol = cellrange.c2 + 1;
                            for (int loop = stRow; loop < rowCount; loop++) rowColMax[loop] = colCount;
                        }
                    }
                }
                else
                {
                    stRow = cellrange.r2;
                    if (stRow == 0)
                    {
                        rowColMax[stRow] = colCount;
                    }
                    else
                    {
                        do
                        {
                            if (rowColMax[stRow - 1] > rowColMax[stRow])
                            {
                                for (int loop = stRow; loop < rowCount; loop++) rowColMax[loop] = rowColMax[stRow - 1];
                                break;
                            }
                            stRow--;

                            if (stRow == 0)
                            {
                                for (int loop = stRow; loop < rowCount; loop++) rowColMax[loop] = colCount;
                                break;
                            }
                        } while (true);
                    }

                    stCol = cellrange.c2 + 1;
                     
                    // row증가를 시킬수 없을때 col Index 증가 가능 여부를 체크
                    if (stCol >= rowColMax[stRow])
                    {
                        // col 값이 꽉 찼을때 row -- 를 시켜 다음 진행 순서를 찾음.  
                        for (int loop = stRow - 1; loop >= 0; loop--)
                        {
                            if (stCol <= rowColMax[loop])
                            {
                                stRow = loop;
                                break;
                            }
                        }

                        if (stRow == 0)
                        {
                            for (int loop = stRow; loop < rowCount; loop++) rowColMax[loop] = colCount;
                        }
                        else
                        {
                            // row를 찾고
                            if (rowColMax[stRow - 1] > rowColMax[stRow])
                            {
                                for (int loop = stRow; loop < rowCount; loop++) rowColMax[loop] = rowColMax[stRow - 1];
                            }
                        }
                    }
                }
                cnt++;
            }
            // cellranges 목록 병합 소스 ! 
            return range;
        } 

        class CellRange {
            
            public string Value { get; set; }
            public int r1;
            public int r2;
            public int c1;
            public int c2;

            public override string ToString()
            {
                return string.Format("Value={0}, R1=[{1}:{2}], R2=[{3}:{4}]", Value, r1, c1, r2, c2);
            }
        }
    }
}
