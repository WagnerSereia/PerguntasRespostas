using Dapper;
using Microsoft.EntityFrameworkCore;
using PerguntasRespostas.Domain.Entities;
using PerguntasRespostas.Domain.Interfaces.Repositories;
using PerguntasRespostas.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PerguntasRespostas.Infra.Data.Repositories
{
    public class PerguntaRepository : RepositoryBase<Pergunta>, IPerguntaRepository
    {
        public PerguntaRepository(DBContext context)
            : base(context)
        {
        }

        public IEnumerable<Pergunta> ObterMinhasPerguntas(String autor)
        {
            var cn = Db.Database.GetDbConnection();
            //string sql = @"SELECT  P.Id,P.Autor,P.Titulo,P.Descricao
            //                FROM Perguntas P 
            //                WHERE autor=@autor";
            //var pergunta = cn.Query<Pergunta>(sql, new { autor = autor }).ToList();
            //return pergunta;

            string sql = @"SELECT  P.Id,P.Autor,P.Titulo,P.Descricao,P.CategoriaId,P.DataCadastro,
                                   C.Id,C.Titulo,C.Descricao,
                                   R.Id,R.Autor,R.Descricao                                   
                            FROM Perguntas P INNER JOIN
                                Categorias C ON P.CategoriaId = C.Id LEFT OUTER  JOIN
                                Respostas R ON P.Id = R.PerguntaId
                            WHERE P.autor=@autor";

            var perguntas = new Dictionary<Guid, Pergunta>();

            cn.Query<Pergunta, Categoria, Respostas, Pergunta>(sql,
                (p, c, r) =>
                {
                    Pergunta pergunta;
                    if (!perguntas.TryGetValue(p.Id, out pergunta))
                    {
                        perguntas.Add(p.Id, pergunta = p);
                    }

                    pergunta.Categoria = c;

                    if (r != null)
                        pergunta.Respostas.Add(r);

                    return pergunta;

                }
                , new { autor = autor }
                , splitOn: "Id,Id,Id")
                .Distinct()
                .ToList();

            return perguntas.Values;
        }

        public override Pergunta ObterPorId(Guid id)
        {
            var cn = Db.Database.GetDbConnection();
            string sql = @"SELECT  P.Id,P.Autor,P.Titulo,P.Descricao,P.CategoriaId,
                                   R.Id,R.Autor,R.Descricao,R.PerguntaId,
                                   C.Id,C.Titulo,C.Descricao
                            FROM Perguntas P LEFT JOIN
                                Respostas R ON P.Id = R.PerguntaId INNER JOIN
                                Categorias C ON P.CategoriaId = C.Id
                            WHERE P.id=@idPergunta";

            var pergunta = cn.Query<Pergunta, Respostas, Categoria, Pergunta>(sql,
                (p, r, c) =>
                {
                    if (r != null)
                        p.Respostas.Add(r);
                    p.Categoria = c;

                    return p;
                }
                , new { idPergunta = id }
                , splitOn: "Id,Id,Id")
                .Distinct()
                .ToList();
            return pergunta.FirstOrDefault();

        }
        public override IEnumerable<Pergunta> ObterTodos()
        {
            var cn = Db.Database.GetDbConnection();
            string sql = @"SELECT  P.Id,P.Autor,P.Titulo,P.Descricao,P.CategoriaId
                            FROM Perguntas P ";
            var pergunta = cn.Query<Pergunta>(sql).ToList();
            return pergunta;
        }
    }
}
