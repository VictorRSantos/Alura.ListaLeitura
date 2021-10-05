using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.Run(Roteamento);
        }

        public Task Roteamento(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();

            var caminhosAtendidos = new Dictionary<string, string>
            {
                {"/Livros/ParaLer", _repo.ParaLer.ToString()},
                {"/Livros/Lendo", _repo.Lendo.ToString()},
                {"/Livros/Lidos", _repo.Lidos.ToString()},
            };

            if (caminhosAtendidos.ContainsKey(context.Request.Path))
            {
                return context.Response.WriteAsync(caminhosAtendidos[context.Request.Path]);
            }

            return context.Response.WriteAsync("Caminho inexistente.");

        }

        public Task LivroParaLer(HttpContext contexto)
        {

            var _repo = new LivroRepositorioCSV();

            return contexto.Response.WriteAsync(_repo.ParaLer.ToString());

        }

    }
}