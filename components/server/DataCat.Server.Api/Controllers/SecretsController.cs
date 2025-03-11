namespace DataCat.Server.Api.Controllers;

public sealed class SecretsController(
    ISecretsProvider secretsProvider)
    : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddSecret([FromBody] SecretModel secret)
    {
        if (!secretsProvider.CanWrite)
        {
            return BadRequest("This secrets provider is read-only.");
        }
        
        await secretsProvider.SetSecretAsync(secret.Key, secret.Value);
        return Ok("Secret added");
    }
    
    [HttpDelete("{key}")]
    public async Task<IActionResult> DeleteSecret(string key)
    {
        if (!secretsProvider.CanWrite)
        {
            return BadRequest("This secrets provider is read-only.");
        }
            
        await secretsProvider.DeleteSecretAsync(key);
        return Ok("Secret removed");
    }

#if DEBUG
    [HttpGet("key")]
    public async Task<IActionResult> GetSecrets(string key)
    {
        var secret = await secretsProvider.GetSecretAsync(key);
        return Ok(secret);
    }
#endif
}