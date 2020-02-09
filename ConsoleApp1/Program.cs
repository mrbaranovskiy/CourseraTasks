using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using DenseMatrix = MathNet.Numerics.LinearAlgebra.Complex.DenseMatrix;
using Matrix = MathNet.Numerics.LinearAlgebra.Complex.Matrix;
using Timer = System.Threading.Timer;

namespace ConsoleApp1
{
    
    
    class Program
    {
        private static int counter = 0;
        static void Main(string[] args)
        {
            List<Thread> th = new List<Thread>();

            Console.Error.WriteLine("Bondatiy performance tool");
            Console.WriteLine("Ebany hell edition:");
            Console.WriteLine("Input the number of threads: ");
            var thCount = int.Parse(Console.ReadLine());

            var coreId = -1;
            if (thCount == 1)
            {
                Console.WriteLine("Input core ID :");
                coreId = int.Parse(Console.ReadLine());
            }

            
            for (int i = 0; i < thCount; i++)
            {
                Thread t = new Thread(() =>
                {
                    Matrix mtx = DenseMatrix.CreateRandom(100, 100, new Laplace());
                    var mn1 = GenerateMatrix().Take(10000000);
                    var result = new List<Matrix<Complex>>();
                    
                    foreach (var matrix in mn1)
                    {
                        Interlocked.Add(ref counter, 1);
                        if (counter % 2 == 0)
                        {
                            var res = mtx * matrix;
                        }
                        else if(counter % 3 == 0)
                        {
                            var res = mtx * matrix;
                        }
                        else
                        {
                            var res = mtx * matrix;
                        }
                    }
                });

                if (coreId != -1)
                {
                    var proc = Process.GetCurrentProcess();
                    proc.ProcessorAffinity = (IntPtr) (1 << (coreId - 1));
                }

                t.Start();
            }
            
           
            Timer time = new Timer(callback, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1));
            
            // result.AddRange(mn1.AsParallel().Select(m => m * mtx));

            Console.ReadKey();

            foreach (var thread in th)
            {
                thread.Abort();
            }
        }

        private static void callback(object state)
        {
            Console.Clear();
            Console.WriteLine($"Speed {counter:N}");
            Interlocked.Exchange(ref counter, 0);
        }

        private static IEnumerable<Matrix> GenerateMatrix()
        {
            while (true)
                yield return DenseMatrix.CreateRandom(100, 100, new Laplace());
        }
    }
}