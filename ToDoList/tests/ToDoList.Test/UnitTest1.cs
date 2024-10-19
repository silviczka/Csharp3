namespace ToDoList.Test;

public class UnitTest1
{
    [Fact] // atribut fact pouzivame na oznacenie testu
    public void Test1()
    {
    //Arrange
    var calculator = new Calculator ();
    //Act
    var result = calculator.Divide(10,5);
    //Assert
    Assert.Equal(2, result);
    }
    [Fact]
     public void Test2()
    {
    //Arrange
    var calculator = new Calculator ();
    //Act
    var divideAction = () => calculator.Divide(10,0);
    //Assert
    Assert.Equal(0, calculator.Divide(10,0));
    }
}
public class Calculator
{
public int Divide (int dividend, int divisor)
{
    return dividend/divisor;
}

}
