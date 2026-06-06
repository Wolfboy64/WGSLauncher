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
        public static string ConnectStr = "Server=mysql8.srkhost.eu;port=3306;Database=s85686_workmtav2;Uid=u85686_zf4KNiwdrl;Pwd=uTuvo8f1s9DqxvANrkxE5tPv;";
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
        public static List<Changelog> GetChangelog()
        {
            List<Changelog> changelogList = new List<Changelog>();
            try
            {
                using (var conn = new MySqlConnection(ConnectStr))
                {
                    conn.Open();
                    string query = "SELECT * FROM changelog";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Changelog changelogEntry = new Changelog(
                                    reader.GetInt32("id"),
                                    reader.GetString("script"),
                                    reader.GetString("leiras"),
                                    reader.GetString("developer"),
                                    reader.GetDateTime("datum")
                                );
                                changelogList.Add(changelogEntry);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt az adatok lekérése során: {ex.Message}", "Adatbázis Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return changelogList;
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
