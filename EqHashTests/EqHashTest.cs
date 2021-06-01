using System;
using System.Collections.Generic;
using Xunit;

namespace EqHashTests
{
    public class EqHashTest
    {
        [Fact]
        public void EqualsUsingHashTests()
        {
            var num1 = new EqHasImaginaryNumber
                ( 42);
            var num2 = new EqHasImaginaryNumber
                (21);
            var arith
                = new Dictionary<MyImaginaryNumber
                    , int>();
            arith
                .Add(num1,  42);
            Assert.Throws<ArgumentException>(() => arith
                .Add(num2,  42));
        }
        [Fact]
        public void MyImaginaryNumberHash()
        {
            var num1 = new ImaginaryNumberWithEquals
                ( 42);
            var num2 = new ImaginaryNumberWithEquals
                (21);
            var arith
                = new Dictionary<MyImaginaryNumber
                    , int>();
            arith
                .Add(num1,  42);
            arith
                .Add(num2,  42);
            Assert.True(num1.Equals(num2));
            Assert.Contains(arith
                , pair => pair.Key.RealNumber
                          ==21);
            Assert.Contains(arith
                , pair => pair.Key.RealNumber
                          == 42);
        }
    }
    internal abstract class MyImaginaryNumber
    {
            public int RealNumber
             { get; }
            public MyImaginaryNumber
            (int value)
            {
                RealNumber
                 = value;
            }
        }
    internal class EqHasImaginaryNumber
        :MyImaginaryNumber
        
        {
            public override bool Equals(object? obj)
            {
                return obj != null && obj.GetHashCode() == this.GetHashCode();
            }
            public override int GetHashCode()
            {
                return RealNumber
                 / 42
                 ;
            }
            public EqHasImaginaryNumber
            (int value) : base(value)
            {
            }
        }
    
        internal class ImaginaryNumberWithEquals
        :MyImaginaryNumber
        
        {
            public override bool Equals(object? obj)
            {
                return obj != null && ((MyImaginaryNumber
                ) obj).RealNumber
                 / 42
                  == this.RealNumber
                  / 42
                  ;
            }
            public ImaginaryNumberWithEquals
            (int value) : base(value)
            {
            }
        }
}