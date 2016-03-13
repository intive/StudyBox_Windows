using FakeItEasy;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace StudyBox.Core.Tests.Samples
{
    [TestClass]
    public class When_adding_two_numbers
    {
        [TestMethod]
        public void the_add_should_returns_sum_of_passed_integers()
        {
            //Arrange
            var firstNumber = 5;
            var secondNumber = 4;

            //Act
            var result = Add(firstNumber, secondNumber);

            //Assert
            Assert.AreEqual(9, result);
            Assert.AreNotEqual(8, result);
        }

        private int Add(int firstNumber, int secondNumber)
        {
            return firstNumber + secondNumber;
        }
    }
}
