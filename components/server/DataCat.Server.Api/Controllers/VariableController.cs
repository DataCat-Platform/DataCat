namespace DataCat.Server.Api.Controllers;

public sealed class VariableController : ApiControllerBase
{
    [HttpGet]
    public IActionResult GetVariables()
    {
        // var variables = _variableService.Search(); // Получаем список всех переменных через сервис
        // return Ok(variables);
        return Ok();
    }
    
    [HttpGet("{id}")]
    public IActionResult GetVariable(int id)
    {
        // var variable = _variableService.Search(id); // Получаем переменную по ID через сервис
        // if (variable == null)
        // {
        //     return NotFound(); // Если переменная не найдена, возвращаем 404
        // }
        // return Ok(variable);
        return Ok();
    }
    
    [HttpPost]
    public IActionResult CreateVariable([FromBody] Variable newVariable) // Variable
    {
        // if (newVariable == null)
        // {
        //     return BadRequest("Invalid data.");
        // }
        //
        // var createdVariable = _variableService.Create(newVariable); // Создаем переменную через сервис
        // return CreatedAtAction(nameof(GetVariable), new { id = createdVariable.Id }, createdVariable);
        return Ok();
    }
    
    [HttpPut("{id}")]
    public IActionResult UpdateVariable(int id, [FromBody] string updatedVariable) // Variable
    {
        // if (updatedVariable == null || updatedVariable.Id != id)
        // {
        //     return BadRequest("Data is invalid.");
        // }
        //
        // var existingVariable = _variableService.Search(id);
        // if (existingVariable == null)
        // {
        //     return NotFound(); // Если переменная не найдена, возвращаем 404
        // }
        //
        // _variableService.Update(id, updatedVariable); // Обновляем переменную через сервис
        // return NoContent(); // Возвращаем 204 No Content
        return Ok();
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteVariable(int id)
    {
        // var variable = _variableService.Search(id);
        // if (variable == null)
        // {
        //     return NotFound(); // Если переменная не найдена, возвращаем 404
        // }
        //
        // _variableService.Delete(id); // Удаляем переменную через сервис
        // return NoContent(); // Возвращаем 204 No Content
        return Ok();
    }
    
    [HttpGet("search")]
    public IActionResult SearchVariables([FromQuery] string name, [FromQuery] string type)
    {
        // var filteredVariables = _variableService.Search(name, type); // Поиск переменных по имени и типу
        // return Ok(filteredVariables);
        return Ok();
    }
    
    [HttpGet("types")]
    public IActionResult GetVariableTypes()
    {
        // var types = _variableService.GetVariableTypes(); // Получаем типы переменных через сервис
        // return Ok(types);
        return Ok();
    }
}

public class Variable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
}