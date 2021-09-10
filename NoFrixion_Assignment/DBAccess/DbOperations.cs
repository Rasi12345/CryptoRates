using NoFrixion_Assignment.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace NoFrixion_Assignment.DBAccess
{
    public class DbOperations
    {
        static string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=E:\NoFrixinSubmissioni\CryptoExercise.mdb";
        public void Insert(CryptoModel model)
        {

            int? ID = GetCryptoID(model.CryptoName);

            if (ID == null)
            {
                InsertIntoCryptoDetails(model);
                ID = GetCryptoID(model.CryptoName);
            }

            InsertIntoCryptoPriceDetails(model.CryptoCurrencyandRate, ID.Value);
        }

        private int? GetCryptoID(string name)
        {
            OleDbConnection conn = new OleDbConnection(connectionString);

            OleDbCommand cmd = new OleDbCommand($"select ID from cryptodetails where CryptoName='{name}'", conn);

            conn.Open();
            int? ID = (int?)cmd.ExecuteScalar();

            conn.Close();

            return ID;
        }

        private void InsertIntoCryptoDetails(CryptoModel model)
        {
            OleDbConnection conn = new OleDbConnection(connectionString);

            OleDbCommand cmd = new OleDbCommand($"insert into CryptoDetails(CryptoName) values ('{model.CryptoName}')", conn);

            conn.Open();
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        private void InsertIntoCryptoPriceDetails(List<PriceModel> model, int Id)
        {
            OleDbConnection conn = new OleDbConnection(connectionString);

            conn.Open();
            foreach (var item in model)
            {
                OleDbCommand cmd = new OleDbCommand($"insert into CryptoPriceDetails([Currency], Rate, LastUpdated, CryptoDetailsID) values ('{item.CurrencyName}', '{item.Rate}', '{item.LastUpdated.ToString()}', {Id})", conn);

                cmd.ExecuteNonQuery();
            }

            conn.Close();
        }

        public List<CryptoModel> Read()
        {
            OleDbConnection conn = new OleDbConnection(connectionString);

            OleDbCommand cmd = new OleDbCommand($"select * from cryptodetails", conn);

            conn.Open();
            OleDbDataReader reader = cmd.ExecuteReader();

            List<CryptoModel> cryptoModels = new List<CryptoModel>();

            while (reader.Read())
            {
                var id = Convert.ToInt32(reader[0].ToString());
                var name = reader[1].ToString();

                CryptoModel model = new CryptoModel();
                model.CryptoName = name;
                model.id = id;
                model.CryptoCurrencyandRate = ReadCurrAndRate(id);

                cryptoModels.Add(model);
            }

            conn.Close();

            return cryptoModels;
        }


        public List<PriceModel> ReadCurrAndRate(int id)
        {
            OleDbConnection conn = new OleDbConnection(connectionString);

            OleDbCommand cmd = new OleDbCommand($"select * from cryptopricedetails where cryptodetailsid={id}", conn);

            conn.Open();
            OleDbDataReader reader = cmd.ExecuteReader();

            List<PriceModel> lstPrice = new List<PriceModel>();
            while (reader.Read())
            {
                PriceModel p = new PriceModel();

                p.CurrencyName = reader[1].ToString();
                p.Rate = Convert.ToDouble(reader[2].ToString());
                p.LastUpdated = DateTime.Parse(reader[4].ToString());

                lstPrice.Add(p);
            }

            return lstPrice;
        }
    }
}