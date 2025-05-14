using SmartScheduler.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScheduler.WPF.Services.Algorithms
{
    public class HungarianAlgorithmService
    {
        private static HungarianAlgorithmService? _instance;
        private HungarianAlgorithmService() { }
        private readonly Random _rng = new();

        #region PUBLIC API

        public static HungarianAlgorithmService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new HungarianAlgorithmService();
            }
            return _instance;
        }

        public List<TaskModel> GetTaskOrderWithHobbyBonus(
            List<TaskModel> tasks,
            User user)
        {
            if (tasks is null || tasks.Count == 0)
                return new();

            /* 1. Matrice de cost de bază */
            var baseCost = GenerateCostMatrixForTasksWithHobbyBonus(tasks, user);
            int n = tasks.Count;

            /* 2. Permutăm aleator coloanele (sloturile) */
            int[] colPerm = Enumerable.Range(0, n)
                                      .OrderBy(_ => _rng.Next())
                                      .ToArray();

            double[,] permCost = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    permCost[i, j] = baseCost[i, colPerm[j]];

            /* 3. Rezolvăm Hungarian pe matricea permutată */
            int[] permAssignment = Solve(permCost);       // assignment in space “permutat”

            /* 4. Mapăm assignment‑ul înapoi la sloturile originale */
            int[] assignment = new int[n];
            for (int i = 0; i < n; i++)
                assignment[i] = colPerm[permAssignment[i]];

            /* 5. Asamblăm lista de task‑uri ordonată după slot */
            return tasks
                .Select((task, row) => new { Slot = assignment[row], Task = task })
                .Where(p => p.Slot >= 0)
                .OrderBy(p => p.Slot)   // sloturile (0..n‑1) – dar acum într‑o ordine random
                .Select(p => p.Task)
                .ToList();
        }

#endregion
        /*──────────────────────────────────────────*/

        #region COST MATRIX  (cu bonus hobby)

        private double[,] GenerateCostMatrixForTasksWithHobbyBonus(
            List<TaskModel> tasks,
            User user)
        {
            int n = tasks.Count;
            var cost = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                double baseCost = tasks[i].RequiredHours;
                bool hobbyHit = user?.Hobbies?.Any(h =>
                                   h.HobbyName.Equals(tasks[i].Category,
                                   StringComparison.OrdinalIgnoreCase)) == true;

                double finalCost = hobbyHit ? Math.Max(0, baseCost - 1) : baseCost;

                for (int j = 0; j < n; j++)
                    cost[i, j] = finalCost;
            }

            return cost;
        }

        #endregion

        #region HUNGARIAN CORE  (n × n, cost minim)

        /// <summary>
        /// Hungarian clasic (versiune simplificată). <br/>
        /// Returnează assignment[i] = coloana pentru rândul i.
        /// </summary>
        public int[] Solve(double[,] costMatrix)
        {
            int n = costMatrix.GetLength(0);
            if (n != costMatrix.GetLength(1))
                throw new ArgumentException("Hungarian: matricea trebuie să fie pătrată.");

            /* 1. Copie */
            var cost = (double[,])costMatrix.Clone();

            /* 2. Reducere pe rânduri */
            for (int i = 0; i < n; i++)
            {
                double min = cost[i, 0];
                for (int j = 1; j < n; j++)
                    if (cost[i, j] < min) min = cost[i, j];

                for (int j = 0; j < n; j++)
                    cost[i, j] -= min;
            }

            /* 3. Reducere pe coloane */
            for (int j = 0; j < n; j++)
            {
                double min = cost[0, j];
                for (int i = 1; i < n; i++)
                    if (cost[i, j] < min) min = cost[i, j];

                for (int i = 0; i < n; i++)
                    cost[i, j] -= min;
            }

            /* 4‑6. Mark/cover (simplificat) */
            var marks = new int[n, n];   // 1 = star
            var rowCover = new bool[n];
            var colCover = new bool[n];

            StarZeros(cost, marks, rowCover, colCover);
            CoverStarColumns(marks, colCover);

            while (CoveredColumnCount(colCover) < n)
            {
                var zero = FindUncoveredZero(cost, rowCover, colCover);
                while (zero == (-1, -1))
                {
                    AdjustMatrix(cost, rowCover, colCover);
                    zero = FindUncoveredZero(cost, rowCover, colCover);
                }

                marks[zero.row, zero.col] = 2; // prime

                int starCol = FindStarInRow(marks, zero.row);
                if (starCol >= 0)
                {
                    rowCover[zero.row] = true;
                    colCover[starCol] = false;
                }
                else
                {
                    AugmentPath(marks, zero);
                    Array.Clear(rowCover, 0, n);
                    Array.Clear(colCover, 0, n);
                    CoverStarColumns(marks, colCover);
                }
            }

            /* 7. Extragem assignment‑ul */
            var result = Enumerable.Repeat(-1, n).ToArray();
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (marks[i, j] == 1) { result[i] = j; break; }

            return result;
        }

        /*──────── Helper‑uri minimal‑implementate (enough for cost-matrix egală pe coloane) ────────*/
        private static void StarZeros(double[,] cost, int[,] marks, bool[] rowC, bool[] colC)
        {
            int n = cost.GetLength(0);
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (cost[i, j] == 0 && !rowC[i] && !colC[j])
                    {
                        marks[i, j] = 1;
                        rowC[i] = colC[j] = true;
                    }
            Array.Clear(rowC); Array.Clear(colC);
        }
        private static void CoverStarColumns(int[,] marks, bool[] colC)
        {
            int n = marks.GetLength(0);
            for (int j = 0; j < n; j++)
                for (int i = 0; i < n; i++)
                    if (marks[i, j] == 1) { colC[j] = true; break; }
        }
        private static int CoveredColumnCount(bool[] colC) => colC.Count(c => c);
        private static (int row, int col) FindUncoveredZero(double[,] cost, bool[] rowC, bool[] colC)
        {
            int n = cost.GetLength(0);
            for (int i = 0; i < n; i++)
                if (!rowC[i])
                    for (int j = 0; j < n; j++)
                        if (!colC[j] && cost[i, j] == 0) return (i, j);
            return (-1, -1);
        }
        private static int FindStarInRow(int[,] marks, int row)
        {
            int n = marks.GetLength(1);
            for (int j = 0; j < n; j++)
                if (marks[row, j] == 1) return j;
            return -1;
        }
        private static void AugmentPath(int[,] marks, (int row, int col) zeroPrime)
        {
            int n = marks.GetLength(0);
            var path = new List<(int r, int c)> { zeroPrime };

            while (true)
            {
                int starRow = path.Last().r;
                int starCol = FindStarInColumn(marks, path.Last().c);
                if (starCol == -1) break;
                path.Add((starCol, path.Last().c));

                int primeCol = FindPrimeInRow(marks, starCol);
                path.Add((starCol, primeCol));
            }

            foreach (var (r, c) in path)
                marks[r, c] = marks[r, c] == 1 ? 0 : 1;

            /* ștergem toate prime‑urile */
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (marks[i, j] == 2) marks[i, j] = 0;
        }
        private static int FindStarInColumn(int[,] marks, int col)
        {
            int n = marks.GetLength(0);
            for (int i = 0; i < n; i++)
                if (marks[i, col] == 1) return i;
            return -1;
        }
        private static int FindPrimeInRow(int[,] marks, int row)
        {
            int n = marks.GetLength(1);
            for (int j = 0; j < n; j++)
                if (marks[row, j] == 2) return j;
            return -1;
        }
        private static void AdjustMatrix(double[,] cost, bool[] rowC, bool[] colC)
        {
            int n = cost.GetLength(0);
            double min = double.MaxValue;
            for (int i = 0; i < n; i++)
                if (!rowC[i])
                    for (int j = 0; j < n; j++)
                        if (!colC[j] && cost[i, j] < min) min = cost[i, j];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    if (rowC[i]) cost[i, j] += min;
                    if (!colC[j]) cost[i, j] -= min;
                }
        }

        #endregion
    }
}
