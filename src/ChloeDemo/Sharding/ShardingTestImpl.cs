﻿using Chloe;
using Chloe.MySql;
using Chloe.MySql.DDL;
using Chloe.Oracle;
using Chloe.Oracle.DDL;
using Chloe.PostgreSQL;
using Chloe.PostgreSQL.DDL;
using Chloe.SqlServer;
using Chloe.SqlServer.DDL;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChloeDemo.Sharding
{
    internal class MsSqlShardingTest : ShardingTest
    {
        string GetConnString(int year)
        {
            string connString = $"Data Source =.;Initial Catalog = order{year};Integrated Security = SSPI;";
            return connString;
        }

        public override DbContext CreateInitDataDbContext(int year)
        {
            string connString = this.GetConnString(year);
            DbContext dbContext = new MsSqlContext(connString);
            return dbContext;
        }

        public override IDbContextProvider CreateDbContextProvider(int year)
        {
            string connString = this.GetConnString(year);
            var dbContextProvider = new MsSqlContextProvider(connString);
            return dbContextProvider;
        }

        public override void CreateTable<TEntity>(DbContext dbContext, string table)
        {
            var tableGenerator = new SqlServerTableGenerator(dbContext);
            tableGenerator.CreateTable(typeof(TEntity), table);
        }
    }

    internal class MySqlShardingTest : ShardingTest
    {
        string GetConnString(int year)
        {
            string connString = $"Server=localhost;Port=3306;Database=order{year};Uid=root;Password=sasa;Charset=utf8; Pooling=True; Max Pool Size=200;Allow User Variables=True;SslMode=none;allowPublicKeyRetrieval=true";
            return connString;
        }

        public override DbContext CreateInitDataDbContext(int year)
        {
            string connString = this.GetConnString(year);
            DbContext dbContext = new MySqlContext(new MySqlConnectionFactory(connString));
            return dbContext;
        }

        public override IDbContextProvider CreateDbContextProvider(int year)
        {
            string connString = this.GetConnString(year);
            var dbContextProvider = new MySqlContextProvider(new MySqlConnectionFactory(connString));
            return dbContextProvider;
        }

        public override void CreateTable<TEntity>(DbContext dbContext, string table)
        {
            var tableGenerator = new MySqlTableGenerator(dbContext);
            tableGenerator.CreateTable(typeof(TEntity), table);
        }
    }

    internal class OracleShardingTest : ShardingTest
    {
        string GetConnString(int year)
        {
            string connString = $"Data Source=localhost/order{year};User ID=system;Password=sasa;";
            return connString;
        }

        public override DbContext CreateInitDataDbContext(int year)
        {
            string connString = this.GetConnString(year);
            DbContext dbContext = new OracleContext(new OracleConnectionFactory(connString));
            return dbContext;
        }

        public override IDbContextProvider CreateDbContextProvider(int year)
        {
            string connString = this.GetConnString(year);
            var dbContextProvider = new OracleContextProvider(new OracleConnectionFactory(connString));
            return dbContextProvider;
        }

        public override void CreateTable<TEntity>(DbContext dbContext, string table)
        {
            var tableGenerator = new OracleTableGenerator(dbContext);
            tableGenerator.CreateTable(typeof(TEntity), table);
        }
    }

    internal class PostgreSQLShardingTest : ShardingTest
    {
        string GetConnString(int year)
        {
            string connString = $"User ID=postgres;Password=sasa;Host=localhost;Port=5432;Database=order{year};Pooling=true;";
            return connString;
        }

        public override DbContext CreateInitDataDbContext(int year)
        {
            string connString = this.GetConnString(year);
            DbContext dbContext = new PostgreSQLContext(new PostgreSQLConnectionFactory(connString));
            return dbContext;
        }

        public override IDbContextProvider CreateDbContextProvider(int year)
        {
            string connString = this.GetConnString(year);
            var dbContextProvider = new PostgreSQLContextProvider(new PostgreSQLConnectionFactory(connString));
            return dbContextProvider;
        }

        public override void CreateTable<TEntity>(DbContext dbContext, string table)
        {
            var tableGenerator = new PostgreSQLTableGenerator(dbContext);
            tableGenerator.CreateTable(typeof(TEntity), table);
        }
    }
}
