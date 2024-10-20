namespace ToDoList.Test;

public class UnitTest1
{
    [Fact] // atribut fact pouzivame na oznacenie testu
    public void Test1_divisionOfIntNumbers()
    {
        //Arrange
        var calculator = new Calculator();
        //Act
        var result = calculator.Divide(10, 5);
        //Assert
        Assert.Equal(2, result);
    }
    [Fact]
    public void Test2_divisionByZero()
    {
        //Arrange
        var calculator = new Calculator();
        //Act
        var exception = Assert.Throws<DivideByZeroException>(() => calculator.Divide(10, 0)); // Expecting an exception
        //Assert
        Assert.Equal("Divisor cannot be zero.", exception.Message); // Check exception message

        /*
                 OG code
                 var divideAction = () => calculator.Divide(10, 0);
                //Assert
                Assert.Equal(0, calculator.Divide(10, 0)); */
    }
}
public class Calculator
{
    public int Divide(int dividend, int divisor)
    {
        if (divisor == 0)
        {
            throw new DivideByZeroException("Divisor cannot be zero."); // Throwing an exception
        }
        return dividend / divisor;
    }

}
