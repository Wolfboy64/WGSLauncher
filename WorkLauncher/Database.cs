using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace WorkLauncher
{
    class Database
    {
        public static string ConnectStr = "Server=localhost;Database=accountsteszt;Uid=root;Pwd=;";
        public static bool Connect()
        {
            bool ret = false;
            try
            {
                using (var conn = new MySqlConnection(ConnectStr))
                {
                    conn.Open();

                }
                ret = true;
            }
            catch (Exception ex)
            {
                ret = false;
                MessageBox.Show($"Hiba történt az adatbázis kapcsolat során: {ex.Message}", "Adatbázis Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return ret;
        }
        public static List<accounts> GetAccounts()
        {
            List<accounts> accountsList = new List<accounts>();
            try
            {
                using (var conn = new MySqlConnection(ConnectStr))
                {
                    conn.Open();
                    string query = "SELECT * FROM accounts";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                accounts acc = new accounts(
                                    reader.GetInt32("id"),
                                    reader.GetString("name"),
                                    reader.GetString("password"),
                                    reader.GetString("email"),
                                    reader.GetString("serial"),
                                    reader.GetString("ip"),
                                    reader.GetInt32("online"),
                                    reader.GetString("banDatas")
                                );
                                accountsList.Add(acc);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt az adatok lekérése során: {ex.Message}", "Adatbázis Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return accountsList;
        }
        
    }
}
