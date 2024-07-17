using Microsoft.AspNetCore.Mvc;
using RedisMultiKeyTest.Caching;
using RedisMultiKeyTest.Entities;
using System.Text.Json;

namespace RedisMultiKeyTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PessoaController : ControllerBase
    {
        //private static ConnectionMultiplexer _redis;
        //private static IDatabase _db;

        // 07DB944D-D3FC-490E-9628-7391F1E12AF7

        private readonly ICachingService _cachingService;

        /*
        private static readonly Pessoa[] Pessoas = new[]
        {
            new Pessoa {
                Id = 1,
                Nome= "Antonio Raulande",
                Ativo = true,
                DataCadastro = DateTime.Now,
                DataNascimento = new DateTime(1983,6,12),
                DataUltimaAlteracao = DateTime.Now,
                UsuarioCadastro = "Antonio",
                UsuarioUltimaAlteracao = "Antonio",
                Documentos = new[]
                {
                    new Documento {Tipo = "RG", Valor = "39.000.457-5"},
                    new Documento {Tipo = "CPF", Valor = "111.111.111-11"},
                    new Documento {Tipo = "CNH", Valor = "021.548.964.564"}
                },
                Enderecos = new[]
                {
                    new Endereco
                    {
                        Id = 1,
                        Logradouro = "Anhaia Mello",
                        Numero = "1500",
                        Bairro = "Vila Ema",
                        Cep = "00000-154",
                        Cidade = "Sao Paulo",
                        IdPessoa = 1,
                        Complemento = "",
                        Uf = "",
                        Ativo = true,
                        DataCadastro = DateTime.Now,
                        DataUltimaAlteracao = DateTime.Now,
                        UsuarioCadastro = "Antonio",
                        UsuarioUltimaAlteracao = "Antonio",
                    }
                }
            }
        };
        */

        private readonly ILogger<PessoaController> _logger;

        public PessoaController(ILogger<PessoaController> logger, ICachingService cachingService)
        {
            _logger = logger;
            _cachingService = cachingService;
        }

        //[HttpGet(Name = "GetWeatherForecast")]
        [HttpGet]
        public async Task<IActionResult> Get(string key)
        {
            var result = await _cachingService.GetAsync(key);
            return Ok(result);

            //var key = "07DB944D-D3FC-490E-9628-7391F1E12AF7";
            //// Conectar ao Redis com AbortOnConnectFail = false
            //var configurationOptions = new ConfigurationOptions
            //{
            //    EndPoints = { "localhost:6379" },
            //    Password = "Redis2019!",
            //    AbortOnConnectFail = false
            //};

            //// Conectar ao Redis
            //_redis = ConnectionMultiplexer.Connect(configurationOptions);
            //_db = _redis.GetDatabase();

            //// Definir um valor em uma chave
            //_db.Set(key, Pessoas.First().Serialize);

            //// Obter o valor da chave
            //string value = _db.StringGet(key);

            //Console.WriteLine($"Value from _redis: {value}");

            //// Fechar a conexão quando terminar
            //_redis.Close();

            //return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] string key, [FromBody] Pessoa pessoa)
        {
            // Serializar o objeto para JSON
            string jsonString = JsonSerializer.Serialize(pessoa);

            await _cachingService.SetAsync(key, jsonString);

            return Ok();
        }
    }
}
