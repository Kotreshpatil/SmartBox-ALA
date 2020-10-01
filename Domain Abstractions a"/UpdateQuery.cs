using System;
using System.Collections.Generic;
using System.Text;
using Libraries;
using ProgrammingParadigms;
using System.Data.SqlClient;

namespace DomainAbstractions
{
    public class UpdateQuery : IDataFlow<string>
    {
        // Public properties
        public string InstanceName { get; set; } = "Default";
        public string Config { get; set; } = "";

        // IDataFlow<string> implementation
        string IDataFlow<string>.Data
        {
            get => default;
            set
            {
                var query = value;
                SelectDbMethod(query);
            }
        }

        // Methods
        private int SelectDbMethod(string query)
        {
            SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=flagdata;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            int flag = (Int32)cmd.ExecuteScalar();
            conn.Close();
            return flag;
        }
        
        public UpdateQuery()
        {

        }
    }
}