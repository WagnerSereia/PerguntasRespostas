DELETE FROM Respostas
DELETE FROM PERGUNTAS
DELETE FROM Categorias

INSERT INTO Categorias(ID,Titulo,Descricao)VALUES('7dcb4f03-d981-40e9-a006-2c64ae5532c1','Banco de Dados', 'Perguntas relacionados com banco de dados')
INSERT INTO Categorias(ID,Titulo,Descricao)VALUES('7dcb4f03-d981-40e9-a006-2c64ae5532c2','Linguagem de programação', 'Perguntas sobre as mais diversas linguagens de programação')
INSERT INTO Categorias(ID,Titulo,Descricao)VALUES('7dcb4f03-d981-40e9-a006-2c64ae5532c3','IoT', 'Perguntas sobre sensores, automação e internet')
INSERT INTO Categorias(ID,Titulo,Descricao)VALUES('7dcb4f03-d981-40e9-a006-2c64ae5532c4','WEB', 'Perguntas sobre desenvolvimento WEB')


INSERT INTO Perguntas(ID,Autor,Titulo,Descricao,CategoriaId,DataCadastro)VALUES('c872072a-e10f-482a-9054-63b9a3ebb6d9','Wagner','Como transformo um metodo em async?','Como transformo um metodo em async?','7dcb4f03-d981-40e9-a006-2c64ae5532c2',GETDATE())
INSERT INTO Perguntas(ID,Autor,Titulo,Descricao,CategoriaId,DataCadastro)VALUES('3a5f2980-010d-4d20-83cb-a07d6316653b','Wagner','Qual é a linguagem mais performática?','Qual é a linguagem mais performática?','7dcb4f03-d981-40e9-a006-2c64ae5532c2',GETDATE())
INSERT INTO Perguntas(ID,Autor,Titulo,Descricao,CategoriaId,DataCadastro)VALUES('f7b64d32-a7a2-4e88-bcfe-d1ac9c84e57d','Rodolfo','Como usar JWT?','Como usar JWT?','7dcb4f03-d981-40e9-a006-2c64ae5532c4',GETDATE())
INSERT INTO Perguntas(ID,Autor,Titulo,Descricao,CategoriaId,DataCadastro)VALUES('a4d74007-1d70-483f-a44f-ef24ad622cfa','Rodolfo','Como realizar deploy no Azure?','Como realizar deploy no Azure?','7dcb4f03-d981-40e9-a006-2c64ae5532c4',GETDATE())
