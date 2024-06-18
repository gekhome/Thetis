using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
//using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Data.Common;

namespace Prototype.DataAccess
{
    public struct Column
    {
        public string ColumnName { get; set; }
        public bool IsNullable { get; set; }
        public string DataType { get; set; }
        public int CharacterMaxLength { get; set; }
        public int NumericPrecision { get; set; }
        public int NumericScale { get; set; }
        public Int64 AutoIncrementBy { get; set; }
        public Int64 AutoIncrementSeed { get; set; }
        public Int64 AutoIncrementNext { get; set; }
        public bool ColumnHasDefault { get; set; }
        public string ColumnDefault { get; set; }
        public bool RowGuidCol { get; set; }
        public int Width { get; set; }
    }

    public enum SortOrderEnum
    {
        ASC, DESC
    }

    public struct Index
    {
        public string IndexName { get; set; }
        public bool Unique { get; set; }
        public bool Clustered { get; set; }
        public int OrdinalPosition { get; set; }
        public string ColumnName { get; set; }
        public SortOrderEnum SortOrder { get; set; }
    }

    public class ColumnList : List<string>
    {
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            foreach (string column in this)
            {
                if (column.StartsWith("[")) str.Append(column + ", "); else str.Append("[" + column + "], ");
            }
            if (str.Length >= 2) str.Remove(str.Length - 2, 2);
            return str.ToString();
        }
    }

    public class Constraint
    {
        public string ConstraintTableName { get; set; }
        public string ConstraintName { get; set; }
        public string ColumnName { get; set; }
        public ColumnList Columns { get; set; }
        public string UniqueConstraintTableName { get; set; }
        public string UniqueConstraintName { get; set; }
        public string UniqueColumnName { get; set; }
        public ColumnList UniqueColumns { get; set; }
        public string DeleteRule { get; set; }
        public string UpdateRule { get; set; }
    }

    public class SqlCeDb : DataAcess.SqlCeBase
    {
        private System.Globalization.CultureInfo culture;
        private Dictionary<string, string> tablesDdl = new Dictionary<string, string>();
        private Dictionary<string, string> primaryKeysDdl = new Dictionary<string, string>();
        private Dictionary<string, string> indexesDdl = new Dictionary<string, string>();
        private Dictionary<string, string> foreignKeysDdl = new Dictionary<string, string>();

        public SqlCeDb()
            : base()
        {
            culture = System.Globalization.CultureInfo.InvariantCulture;
        }

        new public bool Open(string databaseFile, string password)
        {
            tablesDdl.Clear();
            return base.Open(databaseFile, password);
        }

        public string GetTableDdl(string tableName, bool table, bool primaryKeys, bool indexes, bool foreignKeys)
        {
            if (!tablesDdl.ContainsKey(tableName)) LoadTableDdl(tableName);
            StringBuilder ddl = new StringBuilder();
            if (table) ddl.Append(tablesDdl[tableName]);
            if (primaryKeys) ddl.Append(primaryKeysDdl[tableName]);
            if (indexes) ddl.Append(indexesDdl[tableName]);
            if (foreignKeys) ddl.Append(foreignKeysDdl[tableName]);
            return ddl.ToString();
        }

        public object GetTableData(string tableName)
        {
            return ExecuteSql(string.Format("SELECT * FROM [{0}]", tableName), true);
        }

        public void ResetDdl()
        {
            tablesDdl.Clear();
            primaryKeysDdl.Clear();
            indexesDdl.Clear();
            foreignKeysDdl.Clear();
            ResetTableNames();
        }

        private void LoadTableDdl(string tableName)
        {
            StringBuilder ddl = new StringBuilder();
            ddl.AppendFormat("CREATE TABLE [{0}]{1}({1}", tableName, Environment.NewLine);
            AddColumnsDdl(ref ddl, tableName);
            ddl.AppendFormat("{0});{0}", Environment.NewLine);
            tablesDdl.Add(tableName, ddl.ToString());

            primaryKeysDdl.Add(tableName, GetPrimaryKeysDdl(tableName));
            indexesDdl.Add(tableName, GetIndexesDdl(tableName));
            foreignKeysDdl.Add(tableName, GetForeignKeysDdl(tableName));
        }

        private string GetColumnType(string tableName, string columnName)
        {
            DbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = string.Format("SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, NUMERIC_SCALE FROM INFORMATION_SCHEMA.COLUMNS " +
                "WHERE TABLE_NAME='[{0}] AND COLUMN_NAME=[{1}]", tableName, columnName);
            DbDataReader dr = cmd.ExecuteReader();

            string s = "";
            if (dr.Read())
            {
                if (!dr.IsDBNull(2))
                {
                    s = string.Format("Data type: {0}\r\nMax length: {1}", dr.GetString(1), dr.GetInt32(2));
                }
                else
                {
                    s = string.Format("Data type: {0}\r\nPrecision: {1}\r\nScale: {2}", dr.GetString(1), dr.GetValue(3), dr.GetValue(4));
                }
            }
            dr.Close();

            return s;
        }

        private Column GetColumn(ref DbDataReader dr)
        {
            Column col = new Column();
            col.ColumnName = dr.GetString(0);
            col.ColumnHasDefault = (dr.IsDBNull(1) ? false : dr.GetBoolean(1));
            col.ColumnDefault = (dr.IsDBNull(2) ? string.Empty : dr.GetString(2).Trim());
            col.RowGuidCol = (dr.IsDBNull(3) ? false : dr.GetInt32(3) == 378 || dr.GetInt32(3) == 282);
            col.IsNullable = dr.GetString(4).ToLower() == "yes";
            col.DataType = dr.GetString(5);
            col.CharacterMaxLength = (dr.IsDBNull(6) ? 0 : dr.GetInt32(6));
            col.NumericPrecision = (dr.IsDBNull(7) ? 0 : Convert.ToInt32(dr[7], culture));
            col.NumericScale = (dr.IsDBNull(8) ? 0 : Convert.ToInt32(dr[8], culture));
            col.AutoIncrementNext = (dr.IsDBNull(9) ? 0 : Convert.ToInt64(dr[9], culture));
            col.AutoIncrementSeed = (dr.IsDBNull(10) ? 0 : Convert.ToInt64(dr[10], culture));
            col.AutoIncrementBy = (dr.IsDBNull(11) ? 0 : Convert.ToInt64(dr[11], culture));
            return col;
        }

        private Index GetIndex(ref DbDataReader dr)
        {
            Index idx = new Index();
            idx.IndexName = dr.GetString(0);
            idx.Unique = dr.GetBoolean(2);
            idx.Clustered = dr.GetBoolean(3);
            idx.OrdinalPosition = dr.GetInt32(4);
            idx.ColumnName = dr.GetString(5);
            idx.SortOrder = (dr.GetInt16(6) == 1 ? SortOrderEnum.ASC : SortOrderEnum.DESC);
            return idx;
        }

        private Constraint GetConstraint(ref DbDataReader dr)
        {
            Constraint cst = new Constraint();
            cst.ConstraintTableName = dr.GetString(0);
            cst.ConstraintName = dr.GetString(1);
            cst.UniqueConstraintTableName = dr.GetString(2);
            cst.UniqueConstraintName = dr.GetString(3);
            cst.DeleteRule = dr.GetString(4);
            cst.UpdateRule = dr.GetString(5);

            cst.Columns = new ColumnList();
            DbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = string.Format("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE " +
                "WHERE TABLE_NAME = '{0}' AND CONSTRAINT_NAME = '{1}' ORDER BY ORDINAL_POSITION ASC", cst.ConstraintTableName, cst.ConstraintName);
            DbDataReader drColumns = cmd.ExecuteReader();
            while (drColumns.Read()) cst.Columns.Add(drColumns.GetString(0));
            drColumns.Close();

            cst.UniqueColumns = new ColumnList();
            cmd.CommandText = string.Format("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE " +
                "WHERE TABLE_NAME = '{0}' AND CONSTRAINT_NAME = '{1}' ORDER BY ORDINAL_POSITION ASC", cst.UniqueConstraintTableName, cst.UniqueConstraintName);
            DbDataReader drUniqueColumns = cmd.ExecuteReader();
            while (drUniqueColumns.Read()) cst.UniqueColumns.Add(drUniqueColumns.GetString(0));
            drUniqueColumns.Close();

            return cst;
        }

        private void AddColumnLine(ref StringBuilder ddl, Column col)
        {
            switch (col.DataType)
            {
                case "nvarchar":
                    ddl.AppendFormat(culture,
                        "   [{0}] {1}({2}){3}{4}",
                        col.ColumnName,
                        "NVARCHAR",
                        col.CharacterMaxLength,
                        (col.IsNullable ? "" : " NOT NULL"),
                        (col.ColumnHasDefault ? " DEFAULT " + col.ColumnDefault : string.Empty)
                        );
                    break;
                case "nchar":
                    ddl.AppendFormat(culture,
                        "   [{0}] {1}({2}){3}{4}",
                        col.ColumnName,
                        "NCHAR",
                        col.CharacterMaxLength,
                        (col.IsNullable ? "" : " NOT NULL"),
                        (col.ColumnHasDefault ? " DEFAULT " + col.ColumnDefault : string.Empty)
                        );
                    break;
                case "numeric":
                    ddl.AppendFormat(culture,
                        "   [{0}] {1}({2},{3}){4}{5}",
                        col.ColumnName,
                        "DECIMAL",
                        col.NumericPrecision,
                        col.NumericScale,
                        (col.IsNullable ? "" : " NOT NULL"),
                        (col.ColumnHasDefault ? " DEFAULT " + col.ColumnDefault : string.Empty)
                        );
                    break;
                case "binary":
                    ddl.AppendFormat(culture,
                        "   [{0}] {1}({2}){3}{4}",
                        col.ColumnName,
                        "BINARY",
                        col.CharacterMaxLength,
                        (col.IsNullable ? "" : " NOT NULL"),
                        (col.ColumnHasDefault ? " DEFAULT " + col.ColumnDefault : string.Empty)
                        );
                    break;
                case "varbinary":
                    ddl.AppendFormat(culture,
                        "   [{0}] {1}({2}){3}{4}",
                        col.ColumnName,
                        "VARBINARY",
                        col.CharacterMaxLength,
                        (col.IsNullable ? "" : " NOT NULL"),
                        (col.ColumnHasDefault ? " DEFAULT " + col.ColumnDefault : string.Empty)
                        );
                    break;
                default:
                    string rowGuidCol = string.Empty;
                    if (col.RowGuidCol)
                    {
                        rowGuidCol = " ROWGUIDCOL";
                    }
                    ddl.AppendFormat(culture,
                        "   [{0}] {1}{2}{3}{4}{5}",
                        col.ColumnName,
                        col.DataType.ToUpper(),
                        (col.IsNullable ? "" : " NOT NULL"),
                        (col.ColumnHasDefault ? " DEFAULT" + col.ColumnDefault : string.Empty),
                        rowGuidCol,
                        (col.AutoIncrementBy > 0 ? string.Format(culture, " IDENTITY ({0},{1})", col.AutoIncrementSeed, col.AutoIncrementBy) : string.Empty)
                        );
                    break;
            }
        }

        private void AddColumnsDdl(ref StringBuilder ddl, string tableName)
        {
            DbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT COLUMN_NAME, COLUMN_HASDEFAULT, COLUMN_DEFAULT, COLUMN_FLAGS, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, " +
                "NUMERIC_PRECISION, NUMERIC_SCALE, AUTOINC_NEXT, AUTOINC_SEED, AUTOINC_INCREMENT " +
                "FROM INFORMATION_SCHEMA.COLUMNS " +
                "WHERE SUBSTRING(COLUMN_NAME, 1,5) <> '__sys' AND TABLE_NAME = '" + tableName + "' " +
                "ORDER BY ORDINAL_POSITION ASC";
            DbDataReader dr = cmd.ExecuteReader();
            bool first = true;
            while (dr.Read())
            {
                if (first) first = false; else ddl.AppendFormat(",{0}", Environment.NewLine);
                AddColumnLine(ref ddl, GetColumn(ref dr));
            }
            dr.Close();
        }

        private string GetPrimaryKeysDdl(string tableName)
        {
            DbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT u.COLUMN_NAME, c.CONSTRAINT_NAME " +
                "FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS c " +
                "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS u ON c.CONSTRAINT_NAME = u.CONSTRAINT_NAME AND u.TABLE_NAME = c.TABLE_NAME " +
                "WHERE c.CONSTRAINT_TYPE = 'PRIMARY KEY' AND c.TABLE_NAME = '" + tableName + "' " +
                "ORDER BY c.CONSTRAINT_NAME, u.ORDINAL_POSITION";
            DbDataReader dr = cmd.ExecuteReader();
            StringBuilder ddl = new StringBuilder();
            bool first = true;
            while (dr.Read())
            {
                if (first) ddl.AppendFormat("{0}ALTER TABLE [{1}] ADD CONSTRAINT [{2}] PRIMARY KEY (", Environment.NewLine, tableName, dr.GetString(1)); else ddl.Append(", ");
                ddl.AppendFormat("[{0}]", dr.GetString(0));
                first = false;
            }
            if (!first) ddl.AppendFormat(");{0}", Environment.NewLine);
            dr.Close();
            return ddl.ToString();
        }

        private string GetIndexesDdl(string tableName)
        {
            DbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT INDEX_NAME, PRIMARY_KEY, [UNIQUE], [CLUSTERED], ORDINAL_POSITION, COLUMN_NAME, [COLLATION] AS SORT_ORDER " + // Column name COLLATION is SORT_ORDER
                "FROM INFORMATION_SCHEMA.INDEXES " +
                "WHERE (PRIMARY_KEY = 0) " +
                "AND (TABLE_NAME = '" + tableName + "') " +
                "AND (SUBSTRING(COLUMN_NAME, 1,5) <> '__sys') " +
                "ORDER BY INDEX_NAME, ORDINAL_POSITION";
            DbDataReader dr = cmd.ExecuteReader();
            StringBuilder ddl = new StringBuilder();
            string prevName = "";
            while (dr.Read())
            {
                Index idx = GetIndex(ref dr);
                if (idx.IndexName == prevName)
                {
                    ddl.Append(",");
                }
                else
                {
                    if (prevName != "") ddl.AppendFormat(");{0}", Environment.NewLine);
                    ddl.AppendFormat("{0}CREATE ", Environment.NewLine);
                    if (idx.Unique) ddl.Append("UNIQUE ");
                    if (idx.Clustered) ddl.Append("CLUSTERED ");
                    ddl.AppendFormat("INDEX [{0}] ON [{1}] (", idx.IndexName, tableName);
                }
                ddl.AppendFormat("[{0}] {1}", idx.ColumnName, idx.SortOrder.ToString());
                prevName = idx.IndexName;
            }
            if (prevName != "") ddl.AppendFormat(");{0}", Environment.NewLine);
            dr.Close();
            return ddl.ToString();
        }

        private string GetForeignKeysDdl(string tableName)
        {
            DbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT DISTINCT CONSTRAINT_TABLE_NAME, CONSTRAINT_NAME, UNIQUE_CONSTRAINT_TABLE_NAME, UNIQUE_CONSTRAINT_NAME, DELETE_RULE, UPDATE_RULE " +
                "FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS " +
                "WHERE CONSTRAINT_TABLE_NAME='" + tableName + "'";
            DbDataReader dr = cmd.ExecuteReader();
            StringBuilder ddl = new StringBuilder();
            while (dr.Read())
            {
                Constraint cst = GetConstraint(ref dr);
                ddl.AppendFormat("{7}ALTER TABLE [{0}] ADD CONSTRAINT [{1}] FOREIGN KEY ({2}){7}   REFERENCES [{3}]({4}) ON DELETE {5} ON UPDATE {6};{7}",
                    cst.ConstraintTableName,
                    cst.ConstraintName,
                    cst.Columns.ToString(),
                    cst.UniqueConstraintTableName,
                    cst.UniqueColumns.ToString(),
                    cst.DeleteRule,
                    cst.UpdateRule,
                    Environment.NewLine);
            }
            dr.Close();
            return ddl.ToString();
        }
    }
}
