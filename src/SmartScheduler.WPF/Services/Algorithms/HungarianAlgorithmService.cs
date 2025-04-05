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

        public static HungarianAlgorithmService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new HungarianAlgorithmService();
            }
            return _instance;
        }

        /// <summary>
        ///  1) Metodă nouă: Generează o matrice de cost (n x n),
        ///     luând în considerare hobby-urile userului (bonus).
        ///  
        ///  - i = index Task
        ///  - j = index "slot" (tot atâtea sloturi cât taskuri, doar ca exemplu)
        ///  - cost de bază = task.RequiredHours
        ///  - dacă userul are un hobby egal cu task.Category, scădem 1 din cost
        /// </summary>
        public double[,] GenerateCostMatrixForTasksWithHobbyBonus(List<TaskModel> tasks, User user)
        {
            int n = tasks.Count;
            double[,] costMatrix = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                var currentTask = tasks[i];
                for (int j = 0; j < n; j++)
                {
                    // cost de bază = RequiredHours
                    double cost = currentTask.RequiredHours;

                    // dacă userul are un hobby care corespunde cu Category
                    bool matches = false;
                    if (user.Hobbies != null && currentTask.Category != null)
                    {
                        matches = user.Hobbies.Any(h => h.HobbyName.Equals(currentTask.Category, StringComparison.OrdinalIgnoreCase));
                    }

                    if (matches)
                    {
                        // scădem 1 (poți folosi altă valoare)
                        cost -= 1.0;
                        if (cost < 0) cost = 0;
                    }

                    costMatrix[i, j] = cost;
                }
            }

            return costMatrix;
        }

        /// <summary>
        ///  2) Apel "convenience": Generează matricea cu hobby-bonus și rulează Solve(...).
        ///     Returnează un array "assignment" unde assignment[i] = slotul (coloana) pentru taskul i.
        /// </summary>
        public int[] SolveWithHobbyBonus(List<TaskModel> tasks, User user)
        {
            var costMatrix = GenerateCostMatrixForTasksWithHobbyBonus(tasks, user);
            return Solve(costMatrix);
        }

        /// <summary>
        ///  Metoda principală care rezolvă problema de assignare pe baza unei matrice de cost (cost minim).
        ///  Returnează un array "assignment" unde assignment[i] = coloana la care e alocat rândul i.
        /// </summary>
        public int[] Solve(double[,] costMatrix)
        {
            int n = costMatrix.GetLength(0);
            int m = costMatrix.GetLength(1);

            if (n != m)
            {
                throw new ArgumentException("Algoritmul Hungarian necesită o matrice pătrată (n == m).");
            }

            // 1. Copiem matricea de cost, deoarece vom face transformări (reduceri)
            double[,] cost = new double[n, n];
            Array.Copy(costMatrix, cost, costMatrix.Length);

            // 2. Reducere pe rânduri (Row Reduction)
            for (int i = 0; i < n; i++)
            {
                // Găsim minimul de pe rând
                double rowMin = cost[i, 0];
                for (int j = 1; j < n; j++)
                {
                    if (cost[i, j] < rowMin)
                        rowMin = cost[i, j];
                }
                // Scădem minimul din fiecare element
                for (int j = 0; j < n; j++)
                {
                    cost[i, j] -= rowMin;
                }
            }

            // 3. Reducere pe coloane (Column Reduction)
            for (int j = 0; j < n; j++)
            {
                double colMin = cost[0, j];
                for (int i = 1; i < n; i++)
                {
                    if (cost[i, j] < colMin)
                        colMin = cost[i, j];
                }
                for (int i = 0; i < n; i++)
                {
                    cost[i, j] -= colMin;
                }
            }

            // 4. Markare și acoperire (pas cu pas)
            int[] result = new int[n]; // result[i] = coloana aleasă pentru rândul i
            for (int i = 0; i < n; i++) result[i] = -1;

            bool[] rowCover = new bool[n];
            bool[] colCover = new bool[n];
            int[,] marks = new int[n, n]; // 0 = nemarcat, 1 = star, 2 = prime

            int step = 0;
            Step2(ref cost, ref marks, ref rowCover, ref colCover, ref step);

            while (true)
            {
                switch (step)
                {
                    case 2:
                        Step2(ref cost, ref marks, ref rowCover, ref colCover, ref step);
                        break;
                    case 3:
                        Step3(ref marks, ref colCover, ref step);
                        break;
                    case 4:
                        Step4(ref cost, ref marks, ref rowCover, ref colCover, ref step);
                        break;
                    case 5:
                        Step5(ref cost, ref marks, ref rowCover, ref colCover, ref step);
                        break;
                    case 6:
                        Step6(ref cost, ref rowCover, ref colCover, ref step);
                        break;
                    default:
                        // step 7 => gata
                        goto assignment;
                }
            }

        assignment:
            // Extragem assignment-ul
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (marks[i, j] == 1)
                    {
                        result[i] = j;
                        break;
                    }
                }
            }
            return result;
        }

        #region Implementări pas cu pas (simplificate)

        // Step2: Star every zero in the matrix if possible
        private void Step2(ref double[,] cost, ref int[,] marks, ref bool[] rowCover, ref bool[] colCover, ref int step)
        {
            int n = cost.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                rowCover[i] = false;
            }
            for (int j = 0; j < n; j++)
            {
                colCover[j] = false;
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (cost[i, j] == 0 && !rowCover[i] && !colCover[j])
                    {
                        marks[i, j] = 1; // star
                        rowCover[i] = true;
                        colCover[j] = true;
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                rowCover[i] = false;
            }
            for (int j = 0; j < n; j++)
            {
                colCover[j] = false;
            }

            step = 3;
        }

        // Step3: Cover columns containing starred zeros
        private void Step3(ref int[,] marks, ref bool[] colCover, ref int step)
        {
            int n = marks.GetLength(0);
            int colCount = 0;
            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    if (marks[i, j] == 1)
                    {
                        colCover[j] = true;
                        break;
                    }
                }
            }
            for (int j = 0; j < n; j++)
            {
                if (colCover[j]) colCount++;
            }
            if (colCount >= n)
            {
                step = 7; // done
            }
            else
            {
                step = 4;
            }
        }

        // Step4 & Step5 sunt parțial implementate (simplificate)
        private void Step4(ref double[,] cost, ref int[,] marks, ref bool[] rowCover, ref bool[] colCover, ref int step)
        {
            step = 5; // Simplificat
        }

        private void Step5(ref double[,] cost, ref int[,] marks, ref bool[] rowCover, ref bool[] colCover, ref int step)
        {
            step = 6; // Simplificat
        }

        // Step6: Ajustăm valorile din matrice
        private void Step6(ref double[,] cost, ref bool[] rowCover, ref bool[] colCover, ref int step)
        {
            int n = cost.GetLength(0);
            double minVal = double.MaxValue;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (!rowCover[i] && !colCover[j] && cost[i, j] < minVal)
                    {
                        minVal = cost[i, j];
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (rowCover[i])
                    {
                        cost[i, j] += minVal;
                    }
                    if (!colCover[j])
                    {
                        cost[i, j] -= minVal;
                    }
                }
            }
            step = 4;
        }
        #endregion
    }
}
