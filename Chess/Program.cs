using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Running;

namespace Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            MathNet.Numerics.LinearAlgebra.Vector<int>.OuterProduct()            
        }
    }

    // enum Side
    // {
    //     White,
    //     Black
    // }
    //
    // public class GameEngine 
    // {
    //     public Span<short> MakeMove(Side side, Span<short> position, object properties)
    //     {
    //         
    //     }
    //     
    // }
}