namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using ToDoList.WebApi.Controllers;

public class GetTests
{
    [Fact] // atribut fact pouzivame na oznacenie testu
    public void Get_AllItems_ReturnsAllItems()
    {
    //Arrange
    var controller = new ToDoItemsController();

    //Act
    var result = controller.Read();
    var value = result.Value;
    var resultResult = result.Result;
    //Assert
    Assert.True(resultResult is OkObjectResult);
    Assert.IsType<OkObjectResult>(resultResult); // malo by to byt to iste ako ten predosly riadok
    }

}



