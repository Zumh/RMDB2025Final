
using MySql.Data.MySqlClient;
using System.Data;

namespace BookStore
{
    public class PublisherRepository
    {
        private readonly string connectionString;
        private DataSet dataset;
        private MySqlDataAdapter adapter;
        public DataTable? Table;

        public PublisherRepository(string connectionString, DataSet currentDataset)
        {
            this.connectionString = connectionString;
            dataset = currentDataset;

            InitializeAdapter();
            LoadPublishers();
        }

        private void InitializeAdapter()
        {
            adapter = new MySqlDataAdapter("SELECT * FROM publisher", connectionString);
            new MySqlCommandBuilder(adapter);
        }

        public void Add(Publisher currentPublisher)
        {
            DataRow row = Table.NewRow();
            row["name"] = currentPublisher.Name;
            Table.Rows.Add(row);
            adapter.Update(Table);
        }

        public Publisher FindByName(string name)
        {
            LoadPublishers();
            foreach (DataRow row in Table.Rows)
            {
                if (row["publisherName"].ToString() == name)
                {
                    return new Publisher
                    {
                        PublisherID = Convert.ToInt32(row["id"]),
                        Name = row["publisherName"].ToString()
                    };
                }
            }
            return null!;
        }
        private void LoadPublishers()
        {
           

            // Reload schema from database (fresh structure)
            adapter.FillSchema(dataset, SchemaType.Source, "publisher");

            // Reload latest data
            adapter.Fill(dataset, "publisher");

            Table = dataset.Tables["publisher"];

            // Let MySQL handle auto-increment, NOT the DataSet
            Table.Columns["id"].AutoIncrement = true;

            // Set primary key
            Table.PrimaryKey = new DataColumn[] { Table.Columns["id"] };
        }

    
    }


}
