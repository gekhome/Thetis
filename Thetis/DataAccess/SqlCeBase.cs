using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.Common;
using System.Data;
//using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace Prototype.DataAcess
{
    public class SqlCeBase : IDisposable
    {
        private static string[] _availableVersions = { "3.1", "3.5", "4.0" };  // List of available versions

        /// <summary>
        /// Initialize a new instance of SqlCeDb
        /// </summary>
        public SqlCeBase()
        {
            LastError = "";
        }

        public static string[] AvailableVersions { get { return _availableVersions; } }
        public DbConnection Connection { get; private set; }
        public string FileName { get; private set; }
        public string Password { get; private set; }
        public string LastError { get; private set; }
        public string Version { get; private set; }
        public bool BadPassword { get; private set; }
        private Assembly assembly;

        public void Dispose()
        {
            Close();
        }

        /// <summary>
        /// Open the specified database file
        /// </summary>
        /// <param name="databaseFile">Database file name to open</param>
        public bool Open(string databaseFile)
        {
            return Open(databaseFile, null);
        }

        /// <summary>
        /// Open the specified database file
        /// </summary>
        /// <param name="databaseFile">Database file name to open</param>
        /// <param name="password">Password of database file</param>
        public bool Open(string databaseFile, string password)
        {
            Close();
            FileName = Path.GetFullPath(databaseFile);
            Password = password;
            bool ok = false;
            foreach (string version in AvailableVersions.Reverse<string>())
            {
                ok = OpenConnection(version, databaseFile, password);
                if (ok || BadPassword) break;
            }
            return ok;
        }

        public void Close()
        {
            if (Connection != null && Connection.State != ConnectionState.Closed) Connection.Close();
            Connection = null;
            Version = "";
            LastError = "";
            FileName = "";
            Password = "";
            BadPassword = false;
            _tableNames = null;
            _databaseInfo = null;
        }

        public bool IsOpen
        {
            get
            {
                return (Connection != null && Connection.State == ConnectionState.Open);
            }
        }

        private DataTable _databaseInfo;

        public DataTable DatabaseInfo
        {
            get
            {
                if (_databaseInfo != null) return _databaseInfo;
                if (Connection == null) return null;
                _databaseInfo = new DataTable();
                _databaseInfo.Columns.Add("Property");
                _databaseInfo.Columns.Add("Value");
                _databaseInfo.Rows.Add("Version", Version);
                _databaseInfo.Rows.Add("File Path", FileName);
                _databaseInfo.Rows.Add("File Size", GetFileSize(new FileInfo(FileName).Length));

                try
                {
                    List<KeyValuePair<string, string>> dbInfo = (List<KeyValuePair<string, string>>)Connection.GetType().InvokeMember("GetDatabaseInfo", BindingFlags.InvokeMethod, null, Connection, null);
                    foreach (KeyValuePair<string, string> key in dbInfo) _databaseInfo.Rows.Add(System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(key.Key), key.Value);
                }
                catch
                {
                }

                try
                {
                    DbCommand cmd = Connection.CreateCommand();

                    cmd.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES";
                    _databaseInfo.Rows.Add("Tables", cmd.ExecuteScalar().ToString());

                    cmd.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.INDEXES";
                    _databaseInfo.Rows.Add("Indexes", cmd.ExecuteScalar().ToString());

                    cmd.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE";
                    _databaseInfo.Rows.Add("Keys", cmd.ExecuteScalar().ToString());

                    cmd.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS";
                    _databaseInfo.Rows.Add("Table Constraints", cmd.ExecuteScalar().ToString());

                    cmd.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS";
                    _databaseInfo.Rows.Add("Foreign Constraints", cmd.ExecuteScalar().ToString());

                    return _databaseInfo;
                }
                catch
                {
                    return null;
                }
            }
        }

        private string GetFileSize(long bytes)
        {
            string[] size = { "Bytes", "KB", "MB", "GB", "TB" };
            int log = (int)Math.Log(0.1 + bytes, 1024);
            if (log > 4) log = 4;
            double n = bytes / Math.Pow(1024, log);
            return String.Format("{0:N0} {1}", n, size[log]);
        }

        private List<string> _tableNames = null;

        public List<string> TableNames
        {
            get
            {
                if (_tableNames != null) return _tableNames;
                _tableNames = new List<string>();
                DbDataReader dr = ExecuteReader("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = N'TABLE'");
                while (dr.Read()) _tableNames.Add(dr.GetString(0));
                dr.Close();
                return _tableNames;
            }
        }

        protected void ResetTableNames()
        {
            _tableNames.Clear();
            _tableNames = null;
        }

        private enum ResultSetOptions : int { None, Updatable, Scrollable, Sensitive, Insensitive };

        public object ExecuteSql(string sql, bool updatable)
        {
            if (Connection == null) return null;
            if (Connection.State == ConnectionState.Closed) Connection.Open();
            LastError = "";

            object command = assembly.CreateInstance("System.Data.SqlServerCe.SqlCeCommand", false, BindingFlags.CreateInstance, null, new object[] { null, Connection }, null, null);
            var enumType = assembly.GetType("System.Data.SqlServerCe.ResultSetOptions");
            ResultSetOptions options = updatable ? ResultSetOptions.Scrollable | ResultSetOptions.Updatable : ResultSetOptions.Scrollable;
            try
            {
                command.GetType().InvokeMember("CommandText", BindingFlags.SetProperty, null, command, new object[] { sql });
                object resultset = command.GetType().GetMethod("ExecuteResultSet", new System.Type[] { enumType }, null).Invoke(command, new object[] { options });
                bool scrollable = (bool)resultset.GetType().InvokeMember("Scrollable", BindingFlags.GetProperty, null, resultset, null);
                return scrollable ? resultset : null;
            }
            catch (Exception e)
            {
                LastError = (e.InnerException == null) ? e.Message : e.InnerException.Message;
                return null;
            }
        }

        public object ExecuteSql0(string sql, bool updatable)
        {
            if (Connection == null) return null;
            if (Connection.State == ConnectionState.Closed) Connection.Open();
            LastError = "";

            object command = assembly.CreateInstance("System.Data.SqlServerCe.SqlCeCommand", false, BindingFlags.CreateInstance, null, new object[] { sql, Connection }, null, null);
            var enumType = assembly.GetType("System.Data.SqlServerCe.ResultSetOptions");
            ResultSetOptions options = updatable ? ResultSetOptions.Scrollable | ResultSetOptions.Updatable : ResultSetOptions.Scrollable;
            try
            {
                object resultset = command.GetType().GetMethod("ExecuteResultSet", new System.Type[] { enumType }, null).Invoke(command, new object[] { options });
                bool scrollable = (bool)resultset.GetType().InvokeMember("Scrollable", BindingFlags.GetProperty, null, resultset, null);
                return scrollable ? resultset : null;
            }
            catch (Exception e)
            {
                LastError = (e.InnerException == null) ? e.Message : e.InnerException.Message;
                return null;
            }
        }

        public bool Compact(string databaseFile)
        {
            return Compact(databaseFile, null, null);
        }

        public bool Compact(string databaseFile, string password, string newPassword)
        {
            string connectionStr = password == newPassword ? null : "Data Source=; Password=" + (newPassword == null ? "" : newPassword);
            return Tool(databaseFile, password, "Compact", new object[] { connectionStr });
        }

        private enum RepairOption : int { DeleteCorruptedRows, RecoverCorruptedRows, RecoverAllPossibleRows, RecoverAllOrFail }

        public bool Repair(string databaseFile)
        {
            return Repair(databaseFile, null, null);
        }

        public bool Repair(string databaseFile, string password, string newPassword)
        {
            string connectionStr = password == newPassword ? null : "Data Source=" + databaseFile + "; Password=" + (newPassword == null ? "" : newPassword);
            bool ok = DoRepair(databaseFile, password, new object[] { connectionStr, RepairOption.RecoverAllPossibleRows });
            if (ok) ok = DoRepair(databaseFile, newPassword, new object[] { connectionStr, RepairOption.DeleteCorruptedRows });
            return ok;
        }

        private bool DoRepair(string databaseFile, string password, object[] parameters)
        {
            Close();
            if (!Open(databaseFile, password)) return false;
            Close();

            string connectionStr = "Data Source=" + databaseFile;
            if (!string.IsNullOrEmpty(password)) connectionStr += "; Password=" + password;
            try
            {
                object engine = assembly.CreateInstance("System.Data.SqlServerCe.SqlCeEngine", false, BindingFlags.CreateInstance, null, new object[] { connectionStr }, null, null);
                var enumType = assembly.GetType("System.Data.SqlServerCe.RepairOption");
                engine.GetType().GetMethod("Repair", new System.Type[] { typeof(string), enumType }, null).Invoke(engine, parameters);
                engine.GetType().InvokeMember("Dispose", BindingFlags.InvokeMethod, null, engine, null);
                return true;
            }
            catch (Exception e)
            {
                LastError = (e.InnerException == null) ? e.Message : e.InnerException.Message;
                return false;
            }
        }

        public bool Shrink(string databaseFile)
        {
            return Shrink(databaseFile, null, null);
        }

        public bool Shrink(string databaseFile, string password, string newPassword)
        {
            string connectionStr = password == newPassword ? null : "Data Source=; Password=" + (newPassword == null ? "" : newPassword);
            return Tool(databaseFile, password, "Shrink", new object[] { connectionStr });
        }

        public bool Upgrade(string databaseFile, string toVersion)
        {
            return Upgrade(databaseFile, null, null, toVersion);
        }

        public bool Upgrade(string databaseFile, string password, string newPassword, string toVersion)
        {
            Close();
            OpenConnection(toVersion, databaseFile, password);
            Close();

            string newConnectionStr = password == newPassword ? null : "Data Source=; Password=" + (newPassword == null ? "" : newPassword);

            string connectionStr = "Data Source=" + databaseFile;
            if (!string.IsNullOrEmpty(password)) connectionStr += "; Password=" + password;
            try
            {
                object engine = assembly.CreateInstance("System.Data.SqlServerCe.SqlCeEngine", false, BindingFlags.CreateInstance, null, new object[] { connectionStr }, null, null);
                engine.GetType().InvokeMember("Upgrade", BindingFlags.InvokeMethod, null, engine, new object[] { newConnectionStr });
                return true;
            }
            catch (Exception e)
            {
                LastError = (e.InnerException == null) ? e.Message : e.InnerException.Message;
                return false;
            }
        }

        public bool Verify(string databaseFile)
        {
            return Verify(databaseFile, null);
        }

        public bool Verify(string databaseFile, string password)
        {
            Close();
            if (!Open(databaseFile, password)) return false;
            Close();

            string connectionStr = "Data Source=" + databaseFile;
            if (!string.IsNullOrEmpty(password)) connectionStr += "; Password=" + password;
            try
            {
                LastError = "";
                object engine = assembly.CreateInstance("System.Data.SqlServerCe.SqlCeEngine", false, BindingFlags.CreateInstance, null, new object[] { connectionStr }, null, null);
                return (bool)engine.GetType().InvokeMember("Verify", BindingFlags.InvokeMethod, null, engine, null);
            }
            catch (Exception e)
            {
                LastError = (e.InnerException == null) ? e.Message : e.InnerException.Message;
                return false;
            }
        }

        public bool CreateDatabase(string databaseFile, string version)
        {
            return CreateDatabase(databaseFile, null, int.MinValue, version);
        }

        public bool CreateDatabase(string databaseFile, string password, string version)
        {
            return CreateDatabase(databaseFile, password, int.MinValue, version);
        }

        public bool CreateDatabase(string databaseFile, string password, int lcid, string version)
        {
            try
            {
                OpenConnection(version, databaseFile, password);
            }
            catch
            {
            }
            Close();

            string connectionStr = "Data Source=" + databaseFile;
            if (!string.IsNullOrEmpty(password)) connectionStr += "; Password=" + password + "; Encrypt=TRUE;";
            if (lcid != int.MinValue) connectionStr += "; LCID=" + lcid;
            try
            {
                object engine = assembly.CreateInstance("System.Data.SqlServerCe.SqlCeEngine", false, BindingFlags.CreateInstance, null, new object[] { connectionStr }, null, null);
                engine.GetType().InvokeMember("CreateDatabase", BindingFlags.InvokeMethod, null, engine, null);
                return true;
            }
            catch (Exception e)
            {
                LastError = (e.InnerException == null) ? e.Message : e.InnerException.Message;
                return false;
            }
        }

        private DbDataReader ExecuteReader(string sql)
        {
            DbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = sql;
            return cmd.ExecuteReader();
        }

        private bool Tool(string databaseFile, string password, string tool, object[] parameters)
        {
            Close();
            if (!Open(databaseFile, password)) return false;
            Close();

            string connectionStr = "Data Source=" + databaseFile;
            if (!string.IsNullOrEmpty(password)) connectionStr += "; Password=" + password;
            try
            {
                object engine = assembly.CreateInstance("System.Data.SqlServerCe.SqlCeEngine", false, BindingFlags.CreateInstance, null, new object[] { connectionStr }, null, null);
                engine.GetType().InvokeMember(tool, BindingFlags.InvokeMethod, null, engine, parameters);
                return true;
            }
            catch (Exception e)
            {
                LastError = (e.InnerException == null) ? e.Message : e.InnerException.Message;
                return false;
            }
        }

        private bool OpenConnection(string version, string databaseFile, string password)
        {
            Connection = null;
            Version = "";

            string sVersion = GetLongVersion(version);
            string[] vers = sVersion.Split('.');

            string connectionStr = "Data Source=" + databaseFile;
            if (!string.IsNullOrEmpty(password)) connectionStr += "; Password=" + password;

            try
            {
                assembly = Assembly.Load("System.Data.SqlServerCe, Version=" + sVersion + ", Culture=neutral, PublicKeyToken=89845dcd8080cc91");
            }
            catch
            {
                try
                {
                    string desktop = (version == "3.1") ? "" : @"\Desktop";
                    string path = string.Format(@"{0}\Microsoft SQL Server Compact Edition\v{1}.{2}{3}\System.Data.SqlServerCe.dll",
                        Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), vers[0], vers[1], desktop);
                    assembly = Assembly.LoadFile(path);
                }
                catch (Exception e)
                {
                    LastError = e.Message;
                    return false;
                }
            }
            if (assembly == null) return false;

            try
            {
                Connection = (DbConnection)assembly.CreateInstance("System.Data.SqlServerCe.SqlCeConnection", false, BindingFlags.CreateInstance, null, new object[] { connectionStr }, null, null);
                if (Connection == null) return false;
                Connection.Open();
                Version = version;
                return true;
            }
            catch (Exception e)
            {
                int nativeError = int.MinValue;
                try
                {
                    var ex = e.GetBaseException();
                    nativeError = (int)ex.GetType().InvokeMember("NativeError", BindingFlags.GetProperty, null, ex, null);
                }
                catch
                {
                }
                switch (nativeError)
                {
                    case 25138: break;  // Old database version that needs to be upgraded
                    case 25028: BadPassword = true; break;  // Bad password
                    case 28609: break;  // Incorrect version of database, more recent than expected
                    default: throw (e);
                }
                LastError = e.Message;
                return false;
            }
        }

        private string GetLongVersion(string version)
        {
            string[] vers = version.Split('.');
            for (int i = vers.Length; i < 4; i++) version += ".0";
            return version;
        }
    }
}
