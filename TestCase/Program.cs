using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestCase
{
    class Program
    {
        static int starti = 0, startj = 0; //откуда стартуем

        static void Main(string[] args)
        {
            int[,] matrix = new int [3, 3]; //Матрица
            bool[] visited = new bool[10]; //Посещенные ноды
            
            int curi = 0, curj = 0; 
            string answer = "";

            Console.WriteLine("Введите числа от одного до девяти в формате (число число число) на строчку");
            string line;
            for (int i = 0; i < 3; i++)
            {
                while (!(Regex.Match(line = Console.ReadLine(), @"^[0-9] [0-9] [0-9]$").Success))
                {
                    Console.WriteLine("Входная строка имела неправильный формат (число число число), попробуйте ещё раз");
                }

                string[] input = line.Split(' ');

                for (int j = 0; j < 3; j++)
                {
                    matrix[i, j] = Convert.ToInt32(input[j]);
                }

            }
            

            //считаем первую ноду
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (matrix[i, j] > matrix[curi, curj]
                        && !((i == 0 && j == 1) || (i == 1 && j == 0) || (i == 1 && j == 2) || (i == 2 && j == 1)) //Боковые ноды ("середины рёбер") не нужны т.к. с них не построишь "однопроходный" маршрут. Никак.
                        )
                    {
                        curi = i;
                        curj = j;
                    }
                }

            }

            //Сохраняем стартовые координаты на случай если жадный алгоритм будет выходить из центральной ноды
            starti = curi;
            startj = curj;


            visited[matrix[curi, curj]] = true; //Стартуем с ноды где макс. значение

            for (int count = 0; count < 9; count++)
            {
                answer += Convert.ToString(matrix[curi, curj]);
                    GetNextNodeGreedy(matrix, ref visited, ref curi, ref curj, count);

            }
            

            Console.WriteLine(answer);
            Console.ReadLine();
        }

        static void GetNextNodeGreedy(int[,] matrix, ref bool[] visited, ref int x, ref int y, int count)
        {
            int maxi = x, maxj = y;//Буфер поиска максимальных координат
            int max = 0;

            //Проверяем все возможности - вправо, влево, вверх, вниз, находим максимальный существующий вариант
            if (x + 1 < 3) //Если не вышли за границы массива
                if ((matrix[x + 1,y] > max) && visited[matrix[x + 1, y]] == false) //Если значение там больше и нода ещё не посещенная
                {
                    maxi = x + 1; //сохраняем координату после сдвига
                    max = matrix[x + 1, y];
                }

            if (x - 1 >= 0)
                if ((matrix[x - 1, y] > max) && visited[matrix[x - 1, y]] == false)
                {
                    maxi = x - 1;
                    max = matrix[x - 1, y];
                }


            if (y + 1 < 3) 
                if ((matrix[x , y + 1] > max) && visited[matrix[x, y + 1]] == false)
                {
                    maxi = x; //Повороты "влево и вправо" не подошли, координату сдвига по икс очистим
                    maxj = y + 1;
                    max = matrix[x, y + 1];
                }

            if (y - 1 >= 0)
                if ((matrix[x, y - 1] > max) && visited[matrix[x, y - 1]] == false)
                {
                    maxi = x;
                    maxj = y - 1;
                    max = matrix[x, y - 1];
                }


            if (x == 1 && y == 1 && (count == 2 || count == 4)) //Если мы идем из центральной ноды на втором и четвертом шаге (когда жадина может застопориться)
            {

                var Tuple = Closest_unvisited(starti, startj, matrix, visited); //Вместо ноды, где максимальное значение, мы направляем его в ноду ближайшую к стартовому ноду.
                x = Tuple.Item1;
                y = Tuple.Item2;
                visited[matrix[x, y]] = true;

            }
            else
            {
                //Из всех остальных нод
                //После того как "жадный" вариант нашли, ставим ноду как посещенную, и смещаемся.
                x = maxi;
                y = maxj;
                visited[matrix[x, y]] = true;
            }


            return;

        }

        static Tuple<int, int> Closest_unvisited (int x, int y, int[,] matrix, bool[] visited)
        {
            //Нам нужна первая ещё не посещённая нода "около" искомой (искомая это угловая нода, варианта всего два и один из них уже посещён, так что подойдёт первая)

            if (x + 1 < 3) //Если не вышли за границы массива
                if (visited[matrix[x + 1, y]] == false) //Если нода ещё не посещенная
                {
                    return Tuple.Create(x+1, y);
                }

            if (x - 1 >= 0)
                if (visited[matrix[x - 1, y]] == false)
                {
                    return Tuple.Create(x - 1, y);
                }


            if (y + 1 < 3)
                if (visited[matrix[x, y + 1]] == false)
                {
                    return Tuple.Create(x, y + 1);
                }

            if (y - 1 >= 0)
                if (visited[matrix[x, y - 1]] == false)
                {
                    return Tuple.Create(x, y - 1);
                }

            return null;
        }

        



    }
}
