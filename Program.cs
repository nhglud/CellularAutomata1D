/* 
* CELLULAR AUTOMATA SCRIPT
* A little program that calculates a one-dimensional cellular automaton with N cells and tMax time steps. 
* We can think of a 1D cellular automaton at a time t as a 1D array c_i(t) with i = 0, 1, ..., N-1, where c_i = 0 or 1. 
* The update rule is given by some sum over the cells at time t like this: c_i(t+1) = sum_j weight_ij * c_j(t) mod 2. */

/* wrong way to create the Cellular automaton but it also looks cool:

for (int i = 0; i < N; i++)
{
    for (int t = 1; t < TMax; t++)
    {
        cells[i, t] = UpdateCell(cells, i, t, N);
    }
}
*/

using System;

namespace CellularAutomata1D
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int N = 300;
            int tMax = 200;
            string rule = "Stochastic";
            //string rule = "Vanilla";

            //int[,] cells = SingleCellStart(N, tMax);
            int[,] cells = RandomStart(N, tMax);
            cells = UpdateAllCells(cells, N, tMax, rule);
            VisualizeCells(cells, N, tMax);
            
            // Starting conditions

            int[,] RandomStart(int N, int TMax)
            {
                Random random = new();
                int[,] cells = new int[N, tMax];

                for (int i = 0; i < N; i++)
                {
                    cells[i, 0] = random.Next(2);
                }

                return cells;
            }


            int[,] SingleCellStart(int N, int TMax)
            {
                int[,] cells = new int[N, tMax];

                for (int i = 0; i < N; i++)
                {
                    cells[i, 0] = 0;
                }

                cells[N / 2, 0] = 1;

                return cells;
            }

            //

            int[,] UpdateAllCells(int[,] Cells, int N, int TMax, string Rule)
            {
                for (int t = 1; t < TMax; t++)
                {
                    for (int i = 1; i < N; i++)
                    {
                        Cells[i, t] = UpdateCellGeneral(Cells, i, t, N, Rule);
                        //Cells[i, t] = UpdateCellStochastic(Cells, i, t, N);
                    }
                }

                return Cells;
            }

            int UpdateCellStochastic(int[,] Cells, int I, int T, int N)
            {
                Random random = new();

                if (I == 0)
                {
                    Cells[I, T] = (Cells[I, T - 1]
                        + random.Next(3) * Cells[N - 1, T - 1]
                        + random.Next(3) * Cells[1, T - 1]) % 2;
                }
                else if (I == N - 1)
                {
                    Cells[I, T] = (Cells[I, T - 1]
                        + random.Next(3) * Cells[N - 2, T - 1]
                        + random.Next(3) * Cells[0, T - 1]) % 2;
                }
                else
                {
                    Cells[I, T] = (Cells[I, T - 1]
                        + random.Next(3) * Cells[I - 1, T - 1]
                        + random.Next(3) * Cells[I + 1, T - 1]) % 2;
                }

                return Cells[I, T];
            }



            // General Update function based on weights

            int UpdateCellGeneral(int[,] Cells, int Index, int Time, int N, string Rule)
            {
                int[] weightI = new int[N];

                switch (Rule)
                {
                    case "Vanilla":
                        weightI = WeightsVanilla(Index, N);
                        break;

                    case "Stochastic":
                        weightI = WeightsStochastic(Index, N);
                        break;

                    case "Stochastic3":
                        weightI = WeightsStochastic3(Index, N);
                        break;

                    case "Stochastic4":
                        weightI = WeightsStochastic4(Index, N);
                        break;

                    case "TotalRandom":
                        weightI = WeightsTotalRandom(Index, N);
                        break;
                }                
                
                for (int j = 0; j < N; j++)
                {
                    Cells[Index, Time] += weightI[j] * Cells[j, Time-1];
                }

                return Cells[Index, Time] % 2;
            }

            // Weight functions

            int[] WeightsVanilla(int Index, int N)
            {                
                int[] weightI = new int[N];
                for (int i = 0; i < N; i++)
                {
                    if (i == Index || i == Index + 1 || i == Index - 1)
                    {
                        weightI[i] = 1;
                    }
                    else
                    {
                        weightI[i] = 0;
                    }
                }

                return weightI;
            }

            int[] WeightsStochastic(int Index, int N)
            {
                int[] weightI = new int[N];
                Random random = new();

                for (int i = 0; i < N; i++)
                {
                    if( i == Index)
                    {
                        weightI[i] = 1;
                    }
                    if (i == Index || i == Index + 1 || i == Index - 1)
                    {
                        weightI[i] = random.Next(2);
                    }
                    else
                    {
                        weightI[i] = 0;
                    }
                }

                return weightI;
            }


            int[] WeightsStochastic3(int Index, int N)
            {
                Random random = new Random();

                int[] weightI = new int[N];
                for (int i = 0; i < N; i++)
                {
                    if (i == Index || i == Index + 1 || i == Index - 1 || i == Index + 2 || i == Index - 2)
                    {
                        weightI[i] = 1 + random.Next(2);
                    }
                    else
                    {
                        weightI[i] = 0;
                    }
                }

                return weightI;
            }

            int[] WeightsStochastic4(int Index, int N)
            {
                int[] weightI = new int[N];
                Random random = new Random();

                for (int i = 0; i < N; i++)
                {
                    if (i == Index || i == Index + 1 || i == Index - 1)
                    {
                        weightI[i] = 1 + random.Next(3);
                    }
                    else
                    {
                        weightI[i] = 0;
                    }
                }

                return weightI;
            }

            int[] WeightsTotalRandom(int Index, int N)
            {
                int[] weightI = new int[N];
                Random random = new Random();

                for (int i = 0; i < N; i++)
                {
                  
                    weightI[i] = random.Next(2);
                    
                }
                return weightI;
            }


            //

            void VisualizeCells(int[,] Cells, int N, int TMax)
            {
                string[] cellsVis = new string[TMax];
                for (int t = 0; t < TMax; t++)
                {
                    for (int i = 0; i < N; ++i)
                    {
                        cellsVis[t] += (Cells[i, t] == 1) ? "■" : ".";
                    }                   
                }

                for (int t = TMax - 1; t > -1; t--)
                {
                    Console.WriteLine(cellsVis[t]);
                }
            }


        }
    }
}
