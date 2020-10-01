using System;
using System.Collections.Generic;
using System.Text;
using Libraries;
using ProgrammingParadigms;
using System.Data.SqlClient;

namespace DomainAbstractions
{
    public class FlagQuery : IEvent
    {
        // Public properties
        public string InstanceName { get; set; } = "Default";
        public string Query { get; set; } = "";

        // Private fields
        private int _flag = 0;

        // Output ports
        private IEvent high;
        private IEvent low;
        private IDataFlow<int> flagOutput;

        // IEvent implementation
        void IEvent.Execute()
        {
            _flag = GetFlagFromDB(Query);
            Output();
        }

        // Methods
        private int GetFlagFromDB(string query)
        {
            int flag = 0;

            SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=flagdata;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            flag = (Int32)cmd.ExecuteScalar();
            conn.Close();
            //return flag;

            return flag;
        }

        private void Output()
        {
            if (_flag == 1) high?.Execute();
            if (_flag == 0) low?.Execute();
            if (flagOutput != null) flagOutput.Data = _flag;
        }

        public FlagQuery()
        {

        }
    }
}