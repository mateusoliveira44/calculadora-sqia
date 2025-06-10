# Executando o Projeto

A inicialização do projeto é simples e pode ser feita em poucos passos:

## 1. Clone o repositório

Clone o repositório em sua máquina local:

```bash
git clone https://github.com/mateusoliveira44/calculadora-sqia
```

## 2. Suba os containers com Docker Compose

Acesse a pasta raiz do projeto (exemplo: `C:\calculadora-sqia`) onde se encontra o arquivo `docker-compose.yml` e execute o comando:

```bash
docker-compose up
```

Isso irá iniciar os serviços necessários (incluindo a aplicação e o banco de dados SQL Server).

## 3. Execute os scripts SQL

Após a inicialização dos containers, abra o **SQL Server Management Studio (SSMS)** ou outro cliente compatível com SQL Server e:

1. Acesse a pasta `docs` do repositório:  
   [docs no GitHub](https://github.com/mateusoliveira44/calculadora-sqia/tree/main/docs)

2. Execute, na ordem, os scripts abaixo no banco de dados criado pelo Docker:
   - `SC01.sql`
   - `SC_02.sql`
