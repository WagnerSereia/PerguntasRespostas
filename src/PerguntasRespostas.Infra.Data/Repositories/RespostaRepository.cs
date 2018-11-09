using Dapper;
using Microsoft.EntityFrameworkCore;
using PerguntasRespostas.Domain.Entities;
using PerguntasRespostas.Domain.Interfaces.Repositories;
using PerguntasRespostas.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerguntasRespostas.Infra.Data.Repositories
{
    public class RespostaRepository : RepositoryBase<Respostas>, IRespostaRepository
    {
        public RespostaRepository(DBContext context)
            : base(context)
        {
        }
        public override IEnumerable<Respostas> ObterTodos()
        {
            var cn = Db.Database.GetDbConnection();
            string sql = @"SELECT  R.*                                   
                            FROM Respostas as R";
            var respostas = cn.Query<Respostas>(sql)
                .Distinct()
                .ToList();
            return respostas;

        }
        public IEnumerable<Respostas> ObterMinhasRespostas(string autor)
        {
            var cn = Db.Database.GetDbConnection();
            string sql = @"SELECT  P.Id,P.Autor,P.Titulo,P.Descricao,P.CategoriaId,P.DataCadastro,
                                   R.Id,R.Autor,R.Descricao,R.PerguntaId,
                                   C.Id,C.Titulo,C.Descricao
                            FROM Perguntas P INNER JOIN
                                Respostas R ON P.Id = R.PerguntaId INNER JOIN
                                Categorias C ON P.CategoriaId = C.Id
                            WHERE R.autor=@autor";
            var respostas = cn.Query<Pergunta, Respostas, Categoria, Respostas>(sql,
                (p, r, c) =>
                {
                    if (r != null)
                        p.Respostas.Add(r);
                    p.Categoria = c;

                    return r;
                }
                , new { autor = autor }
                , splitOn: "Id,Id,Id")
                .Distinct()
                .ToList();
            return respostas;
        }
    }
}
