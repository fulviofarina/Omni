using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Rule = System.Data.Rule;

//namespace Resources
namespace Rsx
{

    public partial class SQL
    {
        public static void CreateDatabase(string databaseName)
        {
            _databaseName = databaseName;
            _db = _server.Databases[databaseName];
            if (_db != null) _db.Drop();
            _db = new Database(_server, _databaseName);
            _db.Create();
        }

        public static void PopulateDatabase()
        {
            createTables(_source.Tables);
            createRelationships();
        }

        public static void SetDataSetToSQLDB(string connectionString, DataSet source)
        {
            _connection = new SqlConnection(connectionString);
            _server = new Server(new ServerConnection(_connection));
            _source = source;
        }
    }

}

//namespace Resources
namespace Rsx
{
    public partial class SQL
  {
        private static  SqlConnection _connection;
        private static  Server _server;
        private static string _databaseName;
        private static Database _db;
        private static DataSet _source;
        private static DataType cLRTypeToSQLType(Type type)
        {
            switch (type.Name)
            {
                case "String":
                    return DataType.NVarCharMax;

                case "Int32":
                    return DataType.Int;

                case "Boolean":
                    return DataType.Bit;

                case "DateTime":
                    return DataType.DateTime;

                case "Byte[]":
                    return DataType.VarBinaryMax;
            }

            return DataType.NVarCharMax;
        }

        private static void createColumns(ref Table outputTable, DataColumn inputColumn, DataTable inputTable)
        {
            Column newColumn = new Column(outputTable, inputColumn.ColumnName);
            newColumn.DataType = cLRTypeToSQLType(inputColumn.DataType);
            newColumn.Identity = inputColumn.AutoIncrement;
            newColumn.IdentityIncrement = inputColumn.AutoIncrementStep;
            newColumn.IdentitySeed = inputColumn.AutoIncrementSeed;
            newColumn.Nullable = inputColumn.AllowDBNull;
            newColumn.UserData = inputColumn.DefaultValue;

            outputTable.Columns.Add(newColumn);
        }

        private static void createRelation(DataRelation relation)
        {
            Table primaryTable = _db.Tables[relation.ParentTable.TableName];
            Table childTable = _db.Tables[relation.ChildTable.TableName];

            ForeignKey fkey = new ForeignKey(childTable, relation.RelationName);
            fkey.ReferencedTable = primaryTable.Name;

            fkey.DeleteAction = sQLActionTypeToSMO(relation.ChildKeyConstraint.DeleteRule);
            fkey.UpdateAction = sQLActionTypeToSMO(relation.ChildKeyConstraint.UpdateRule);

            for (int i = 0; i < relation.ChildColumns.Length; i++)
            {
                DataColumn col = relation.ChildColumns[i];
                ForeignKeyColumn fkc = new ForeignKeyColumn(fkey, col.ColumnName, relation.ParentColumns[i].ColumnName);

                fkey.Columns.Add(fkc);
            }

            fkey.Create();
        }

        private static void createRelationships()
        {
      foreach (DataTable table in _source.Tables)
      {
        foreach (DataRelation rel in table.ChildRelations)
          createRelation(rel);
      }
    }
    private static void createTables(DataTableCollection tables)
    {
      foreach (DataTable table in tables)
      {
        dropExistingTable(table.TableName);
        Table newTable = new Table(_db, table.TableName);

        populateTable(ref newTable, table);
        setPrimaryKeys(ref newTable, table);
        newTable.Create();
      }
    }

        private static void dropExistingTable(string tableName)
        {
            Table table = _db.Tables[tableName];
            if (table != null) table.Drop();
        }

        private static void populateTable(ref Table outputTable, DataTable inputTable)
        {
      foreach (DataColumn column in inputTable.Columns)
      {
        createColumns(ref outputTable, column, inputTable);
      }
    }
    private static void setPrimaryKeys(ref Table outputTable, DataTable inputTable)
    {
      Index newIndex = new Index(outputTable, "PK_" + outputTable.Name);
      newIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
      newIndex.IsClustered = false;

      foreach (DataColumn keyColumn in inputTable.PrimaryKey)
      {
        newIndex.IndexedColumns.Add(new IndexedColumn(newIndex, keyColumn.ColumnName, true));
      }
      if (newIndex.IndexedColumns.Count > 0)
        outputTable.Indexes.Add(newIndex);
    }
    private static ForeignKeyAction sQLActionTypeToSMO(Rule rule)
    {
      string ruleStr = rule.ToString();

      return (ForeignKeyAction)Enum.Parse(typeof(ForeignKeyAction), ruleStr);
    }
  }
}