using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ThreeShape.Maths.LinearAlgebra;

namespace Chess
{
    [RPlotExporter]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    public class Bench
    {
        static List<Matrix4x4> _mtxNum = new List<Matrix4x4>();
        List<MatrixAffine4x4d> _mtxShape = new List<MatrixAffine4x4d>();

        [Params(1000000)] public int Num { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _mtxNum.Clear();
            _mtxShape.Clear();
            _mtxNum.AddRange(GetMatricies().Take(Num));
            _mtxShape.AddRange(GetMatricies3Shape().Take(Num));
        }

        private IEnumerable<Matrix4x4> GetMatricies()
        {
            var random = new Random();

            while (true)
            {
                var num = random.NextDouble() * Math.PI;

                yield return Matrix4x4.CreateRotationX((float) num);
            }
        }

        private IEnumerable<MatrixAffine4x4d> GetMatricies3Shape()
        {
            var random = new Random();

            while (true)
            {
                var num = random.NextDouble() * Math.PI;

                yield return MatrixAffine4x4d.BuildRotation(Vector3d.UnitX, num);
            }
        }

        [Benchmark]
        public Matrix4x4 MtxMultIntristics()
        {
            Matrix4x4 result = Matrix4x4.Identity;
            for (var i = 0; i < _mtxNum.Count; i++)
            {
                result = _mtxNum[i] * _mtxNum[i];
            }

            return result;
        }

        [Benchmark]
        public static Matrix4x4 MtxMultIntristicsStatic()
        {
            Matrix4x4 result = Matrix4x4.Identity;
            for (var i = 0; i < _mtxNum.Count; i++)
            {
                result = _mtxNum[i] * _mtxNum[i];
            }

            return result;
        }


        // [Benchmark]
        // public MatrixAffine4x4d MtxMultUnrolled()
        // {
        //     MatrixAffine4x4d result = MatrixAffine4x4d.Identity;
        //     for (var i = 0; i < _mtxNum.Count; i++)
        //     {
        //         result = _mtxShape[i] * _mtxShape[i];
        //     }
        //
        //     return result;
        // }
        //
        // [Benchmark]
        // public Matrix3x3d MtxDet()
        // {
        //     Matrix3x3d result = Matrix3x3d.Identity;
        //     for (var i = 0; i < _mtxNum.Count; i++)
        //     {
        //     }
        //
        //     return result;
        // }

        // [Benchmark]
        // public static Matrix4x4 MtxDetIntristicsStatic()
        // {
        //     Matrix4x4 result = Matrix4x4.Identity;
        //     for (var i = 0; i < _mtxNum.Count; i++)
        //     {
        //         
        //         result = Matrix4x4.Transpose(_mtxNum[i]);
        //     }
        //
        //     return result;
        // }

        // [Benchmark]
        // public Matrix4x4 MtxDetIntristics()
        // {
        //     Matrix4x4 result = Matrix4x4.Identity;
        //     for (var i = 0; i < _mtxNum.Count; i++)
        //     {
        //         
        //         result = Matrix4x4.Transpose(_mtxNum[i]);
        //     }
        //
        //     return result;
        // }
    }
}