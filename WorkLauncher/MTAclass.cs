using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkLauncher
{
    class MTAclass
    {


        public class MtaServerData
        {
            public string GameName { get; set; }
            public string Port { get; set; }
            public string ServerName { get; set; }
            public string GameMode { get; set; }
            public string MapName { get; set; }
            public string Version { get; set; }
            public bool Passworded { get; set; }
            public int PlayerCount { get; set; }
            public int MaxPlayers { get; set; }
            public Dictionary<string, string> Rules { get; set; } = new();
            public List<MtaPlayer> Players { get; set; } = new();
        }

        public class MtaPlayer
        {
            public string Name { get; set; }
            public string Team { get; set; }
            public string Skin { get; set; }
            public string Score { get; set; }
            public string Ping { get; set; }
            public string Time { get; set; }
        }

        public static class MtaAseParser
        {
            public static MtaServerData Parse(byte[] bytes)
            {
                MtaServerData data = new MtaServerData();
                int index = 0;

                // 1. Ellenőrizzük a fejlécet ("EYE1")
                if (bytes.Length < 4 || Encoding.ASCII.GetString(bytes, 0, 4) != "EYE1")
                    throw new Exception("Érvénytelen ASE fejléc.");

                index += 4; // Átugorjuk az EYE1-et

                // 2. Alapadatok beolvasása sorrendben
                data.GameName = ReadAseString(bytes, ref index);
                data.Port = ReadAseString(bytes, ref index);
                data.ServerName = ReadAseString(bytes, ref index);
                data.GameMode = ReadAseString(bytes, ref index);
                data.MapName = ReadAseString(bytes, ref index);
                data.Version = ReadAseString(bytes, ref index);

                string pwdStr = ReadAseString(bytes, ref index);
                data.Passworded = pwdStr == "1";

                int.TryParse(ReadAseString(bytes, ref index), out int pCount);
                data.PlayerCount = pCount;

                int.TryParse(ReadAseString(bytes, ref index), out int maxPlayers);
                data.MaxPlayers = maxPlayers;

                // 3. Szerver szabályok (Rules) beolvasása (Key-Value párok)
                // Addig megyünk, amíg egy üres kulcsot (1-es bájtot) nem találunk
                while (index < bytes.Length)
                {
                    if (bytes[index] == 1)
                    {
                        index++; // Átugorjuk a lezáró 1-es bájtot
                        break;
                    }

                    string key = ReadAseString(bytes, ref index);
                    string value = ReadAseString(bytes, ref index);

                    if (!string.IsNullOrEmpty(key))
                        data.Rules[key] = value;
                }

                // 4. Játékosok beolvasása
                // Az MTA-nál minden játékoshoz 6 darab ASE string tartozik ebben a sorrendben
                while (index < bytes.Length && data.Players.Count < data.PlayerCount)
                {
                    if (bytes[index] == 1) break; // Ha vége a streamnek

                    MtaPlayer player = new MtaPlayer
                    {
                        Name = ReadAseString(bytes, ref index),
                        Team = ReadAseString(bytes, ref index),
                        Skin = ReadAseString(bytes, ref index),
                        Score = ReadAseString(bytes, ref index),
                        Ping = ReadAseString(bytes, ref index),
                        Time = ReadAseString(bytes, ref index)
                    };

                    data.Players.Add(player);
                }

                return data;
            }

            // Segédfüggvény az ASE formátumú szövegek kiolvasásához
            private static string ReadAseString(byte[] bytes, ref int index)
            {
                if (index >= bytes.Length) return string.Empty;

                int lengthByte = bytes[index++];
                int actualLength = lengthByte - 1; // A hossz mindig 1-gyel nagyobb a valóságnál

                if (actualLength <= 0) return string.Empty;

                // Biztonsági ellenőrzés túlcsordulás ellen
                if (index + actualLength > bytes.Length)
                    actualLength = bytes.Length - index;

                string result = Encoding.UTF8.GetString(bytes, index, actualLength);
                index += actualLength;
                return result;
            }
        }
    }
}
