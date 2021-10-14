using Alura.ListaLeitura.App.Negocio;
using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        public void Configure(IApplicationBuilder app)
        {
            var builder = new RouteBuilder(app);


            builder.MapRoute("Livros/ParaLer", LivroParaLer);
            builder.MapRoute("Livros/Lendo", LivrosLendo);
            builder.MapRoute("Livros/Lidos", LivrosLidos);
            builder.MapRoute("Cadastro/NovoLivro/{nome}/{autor}", NovoLivroParaLer);
            builder.MapRoute("Livros/Detalhes/{id:int}", ExibeDetalhes);
            builder.MapRoute("Cadastro/NovoLivro", ExibeFormulario);
            builder.MapRoute("Cadastro/Incluir", ProcessaFormulario);

            var rotas = builder.Build();
            app.UseRouter(rotas);

        }

        private Task ProcessaFormulario(HttpContext context)
        {
            var livro = new Livro()
            {
                Titulo = context.Request.Query["titulo"].First(),
                Autor = context.Request.Query["autor"].First()
            };

            var repo = new LivroRepositorioCSV();

            repo.Incluir(livro);

            return context.Response.WriteAsync("O Livro foi adicionado com sucesso");
        }

        private Task ExibeFormulario(HttpContext context)
        {
            var html = @"
                        <html>
                            <form action='/Cadastro/Incluir'>
                               <input name='titulo'/>
                               <input name='autor'/>
                               <button>Gravar</button>
                            </form>
                        </html>";

            return context.Response.WriteAsync(html);
        }

        public Task ExibeDetalhes(HttpContext context)
        {
            int id = Convert.ToInt32(context.GetRouteValue("id"));
            var repo = new LivroRepositorioCSV();
            var livro = repo.Todos.First(l => l.Id == id);

            return context.Response.WriteAsync(livro.Detalhes());
        }


        public Task NovoLivroParaLer(HttpContext context)
        {
            var livro = new Livro()
            {
                Titulo = context.GetRouteValue("nome").ToString(),
                Autor = context.GetRouteValue("autor").ToString()
            };

            var repo = new LivroRepositorioCSV();

            repo.Incluir(livro);

            return context.Response.WriteAsync("O Livro foi adicionado com sucesso");
        }


        public Task Roteamento(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();

            var caminhosAtendidos = new Dictionary<string, RequestDelegate>
            {
                {"/Livros/ParaLer", LivroParaLer},
                {"/Livros/Lendo", LivrosLendo},
                {"/Livros/Lidos", LivrosLidos},
            };

            if (caminhosAtendidos.ContainsKey(context.Request.Path))
            {
                var metodo = caminhosAtendidos[context.Request.Path];

                return metodo.Invoke(context);
            }

            context.Response.StatusCode = 404;

            return context.Response.WriteAsync("Caminho inexistente.");

        }


        public Task LivroParaLer(HttpContext contexto)
        {

            var _repo = new LivroRepositorioCSV();

            return contexto.Response.WriteAsync(_repo.ParaLer.ToString());

        }


        public Task LivrosLendo(HttpContext contexto)
        {

            var _repo = new LivroRepositorioCSV();

            return contexto.Response.WriteAsync(_repo.Lendo.ToString());

        }


        public Task LivrosLidos(HttpContext contexto)
        {

            var _repo = new LivroRepositorioCSV();

            return contexto.Response.WriteAsync(_repo.Lidos.ToString());

        }

    }
}