using System;
using System.Collections.Generic;
using Xunit;

namespace EqHashTests
{
    public class Person
    {
        public int Age { get; }

        public Person(int age)
        {
            Age = age;
        }
    }

    public class PersonWithEqualsUsingHash : Person
    {
        public override bool Equals(object obj)
        {
            return obj != null && obj.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return Age % 10;
        }

        public PersonWithEqualsUsingHash(int age) : base(age)
        {
        }
    }

    public class PersonWithEqualsButWithoutHash : Person
    {
        public override bool Equals(object obj)
        {
            return obj is Person person && person.Age % 10 == Age % 10;
        }

        public PersonWithEqualsButWithoutHash(int age) : base(age)
        {
        }
    }
    
    public class EqualsHashCodeTests
    {
        [Fact]
        public void EqualsUsingHash()
        {
            var person1 = new PersonWithEqualsUsingHash(10);
            var person2 = new PersonWithEqualsUsingHash(20);
            
            var persons = new Dictionary<Person, string>();
            persons.Add(person1, "10 лет");

            Assert.Throws<ArgumentException>(() => persons.Add(person2, "20 лет"));
        }

        [Fact]
        public void OverrideOnlyEqual()
        {
            var person1 = new PersonWithEqualsButWithoutHash(10);
            var person2 = new PersonWithEqualsButWithoutHash(20);
            
            var persons = new Dictionary<Person, string>();
            persons.Add(person1, "10 лет");
            persons.Add(person2, "20 лет");
            
            Assert.Equal(person1, person2);
            Assert.True(persons.ContainsKey(person1));
            Assert.True(persons.ContainsKey(person2));
        }
    }
}