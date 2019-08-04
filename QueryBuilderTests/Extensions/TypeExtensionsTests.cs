using System;
using System.Collections.Generic;
using QueryBuilder.Extensions;
using Xunit;

namespace QueryBuilderTests.Extensions
{
    public class TypeExtensionsTests
    {
        [Theory]
        [InlineData(typeof(bool[]))]
        [InlineData(typeof(int[]))]
        [InlineData(typeof(char[]))]
        [InlineData(typeof(List<bool>))]
        [InlineData(typeof(List<int>))]
        [InlineData(typeof(List<char>))]
        public void IsTypeEnumerable_EnumerableType_ReturnsTrue(Type type)
        {
            // Arrange & Act & Assert
            Assert.True(type.IsTypeEnumerable());
            Assert.True(type.IsTypeEnumerable(true));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(bool))]
        [InlineData(typeof(int))]
        [InlineData(typeof(char))]
        public void IsTypeEnumerable_NonEnumerableType_ReturnsFalse(Type type)
        {
            // Arrange & Act & Assert
            Assert.False(type.IsTypeEnumerable());
            Assert.False(type.IsTypeEnumerable(true));
        }

        [Theory]
        [InlineData(typeof(string))]
        public void IsTypeEnumerable_StringType_ReturnsFalseWithoutFlag(Type type)
        {
            // Arrange & Act & Assert
            Assert.False(type.IsTypeEnumerable());
        }

        [Theory]
        [InlineData(typeof(string))]
        public void IsTypeEnumerable_StringType_ReturnsTrueWithFlag(Type type)
        {
            // Arrange & Act & Assert
            Assert.True(type.IsTypeEnumerable(true));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(string))]
        [InlineData(typeof(bool?))]
        [InlineData(typeof(int?))]
        [InlineData(typeof(char?))]
        [InlineData(typeof(string[]))]
        [InlineData(typeof(List<string>))]
        public void IsTypeNullable_NullableType_ReturnsTrue(Type type)
        {
            // Arrange & Act & Assert
            Assert.True(type.IsTypeNullable());
        }

        [Theory]
        [InlineData(typeof(bool))]
        [InlineData(typeof(int))]
        [InlineData(typeof(char))]
        public void IsTypeNullable_NonNullableType_ReturnsFalse(Type type)
        {
            // Arrange & Act & Assert
            Assert.False(type.IsTypeNullable());
        }

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(string))]
        [InlineData(typeof(bool?))]
        [InlineData(typeof(int?))]
        [InlineData(typeof(char?))]
        [InlineData(typeof(string[]))]
        [InlineData(typeof(List<string>))]
        public void ConvertToNullableType_NullableType_ReturnsSameType(Type type)
        {
            // Arrange & Act & Assert
            Assert.Equal(type, type.ConvertToNullableType());
        }

        [Theory]
        [InlineData(typeof(bool), typeof(bool?))]
        [InlineData(typeof(int), typeof(int?))]
        [InlineData(typeof(char), typeof(char?))]
        public void ConvertToNullableType_NonNullableType_ReturnsNullableType(Type type1, Type type2)
        {
            // Arrange & Act & Assert
            Assert.Equal(type2, type1.ConvertToNullableType());
        }

        [Theory]
        [InlineData(typeof(string[]), typeof(string))]
        [InlineData(typeof(bool[]), typeof(bool))]
        [InlineData(typeof(int[]), typeof(int))]
        [InlineData(typeof(char[]), typeof(char))]
        [InlineData(typeof(List<string>), typeof(string))]
        [InlineData(typeof(List<bool>), typeof(bool))]
        [InlineData(typeof(List<int>), typeof(int))]
        [InlineData(typeof(List<char>), typeof(char))]
        public void GetNonEnumerableBaseType_EnumerableType_ReturnsBaseType(Type type1, Type type2)
        {
            // Arrange & Act & Assert
            Assert.Equal(type2, type1.GetNonEnumerableBaseType());
        }

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(string))]
        [InlineData(typeof(bool))]
        [InlineData(typeof(int))]
        [InlineData(typeof(char))]
        public void GetNonEnumerableBaseType_NonEnumerableType_ReturnsSameType(Type type)
        {
            // Arrange & Act & Assert
            Assert.Equal(type, type.GetNonEnumerableBaseType());
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(typeof(string), typeof(string))]
        [InlineData(typeof(bool?), typeof(bool))]
        [InlineData(typeof(int?), typeof(int))]
        [InlineData(typeof(char?), typeof(char))]
        public void GetNonNullableBaseType_NullableType_ReturnsBaseType(Type type1, Type type2)
        {
            // Arrange & Act & Assert
            Assert.Equal(type2, type1.GetNonNullableBaseType());
        }

        [Theory]
        [InlineData(typeof(bool))]
        [InlineData(typeof(int))]
        [InlineData(typeof(char))]
        public void GetNonNullableBaseType_NonNullableType_ReturnsSameType(Type type)
        {
            // Arrange & Act & Assert
            Assert.Equal(type, type.GetNonNullableBaseType());
        }

        [Theory]
        [InlineData(typeof(string[]), typeof(string))]
        [InlineData(typeof(bool?[]), typeof(bool))]
        [InlineData(typeof(int?[]), typeof(int))]
        [InlineData(typeof(char?[]), typeof(char))]
        [InlineData(typeof(List<string>), typeof(string))]
        [InlineData(typeof(List<bool?>), typeof(bool))]
        [InlineData(typeof(List<int?>), typeof(int))]
        [InlineData(typeof(List<char?>), typeof(char))]
        public void GetNonEnumerableNonNullableBaseType_EnumerableNullableType_ReturnsNonNullableBaseType(Type type1, Type type2)
        {
            // Arrange & Act & Assert
            Assert.Equal(type2, type1.GetNonEnumerableNonNullableBaseType());
        }

        [Theory]
        [InlineData(typeof(bool[]), typeof(bool))]
        [InlineData(typeof(int[]), typeof(int))]
        [InlineData(typeof(char[]), typeof(char))]
        [InlineData(typeof(List<bool>), typeof(bool))]
        [InlineData(typeof(List<int>), typeof(int))]
        [InlineData(typeof(List<char>), typeof(char))]
        public void GetNonEnumerableNonNullableBaseType_EnumerableNonNullableType_ReturnsNonNullableBaseType(Type type1, Type type2)
        {
            // Arrange & Act & Assert
            Assert.Equal(type2, type1.GetNonEnumerableNonNullableBaseType());
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(typeof(string), typeof(string))]
        [InlineData(typeof(bool?), typeof(bool))]
        [InlineData(typeof(int?), typeof(int))]
        [InlineData(typeof(char?), typeof(char))]
        public void GetNonEnumerableNonNullableBaseType_NonEnumerableNullableType_ReturnsNonNullableBaseType(Type type1, Type type2)
        {
            // Arrange & Act & Assert
            Assert.Equal(type2, type1.GetNonEnumerableNonNullableBaseType());
        }

        [Theory]
        [InlineData(typeof(bool), typeof(bool))]
        [InlineData(typeof(int), typeof(int))]
        [InlineData(typeof(char), typeof(char))]
        public void GetNonEnumerableNonNullableBaseType_NonEnumerableNonNullableType_ReturnsNonNullableBaseType(Type type1, Type type2)
        {
            // Arrange & Act & Assert
            Assert.Equal(type2, type1.GetNonEnumerableNonNullableBaseType());
        }
    }
}
