using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ClientTextProcessor1
{
    class Program
    {
        static void Main(string[] args)
        {
            int port;
            string address;
            if (args.Length>1)
            {
                address = args[0];
                port = Convert.ToInt32(args[1]);
            }
            else
            {
                Console.WriteLine("При запуске программы указаны не все параметры!");
                Console.ReadLine();
                return;
            }
            TcpClient client = null;
            try
            {
                client = new TcpClient(address, port);
                NetworkStream stream = client.GetStream();

                while (true)
                {
                    string command = Console.ReadLine();
                    string words;
                    string[] arrCommand= command.Split(' ');
                    byte[] data;

                    if (arrCommand[0] == "get")
                    {
                        // преобразуем сообщение в массив байтов
                        data = Encoding.Unicode.GetBytes(arrCommand[1]);
                        // отправка сообщения
                        stream.Write(data, 0, data.Length);
                        // получаем ответ
                        data = new byte[64]; // буфер для получаемых данных
                        StringBuilder builder = new StringBuilder();
                        int bytes = 0;
                        do
                        {
                            bytes = stream.Read(data, 0, data.Length);
                            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        }
                        while (stream.DataAvailable);

                        words = builder.ToString();
                        string[] arrWords = words.Split(' ');
                        foreach (var word in arrWords)
                        {
                            Console.WriteLine(word);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Введена неверная команда");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}
