using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserServiceLibrary.Entities;
using UserServiceLibrary.Services;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var key = Console.ReadLine();
            if (key.ToLowerInvariant() == "t")
            {
                var s = new UserService(true, new HashSet<User>());
                s.notifier.AddEvent += s.TcpAddToSlave;
                var user = new User() { BirthDate = DateTime.Today, FirstName = "masterFN1", LastName = "masterLn1" };
                s.Add(user);
            }
            else
            {
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} - MAIN current thread");
                var hs = new HashSet<User>();
                var user = new User() { BirthDate = DateTime.Today, FirstName = "FN1", LastName = "Ln1" };
                var user2 = new User() { BirthDate = DateTime.Today, FirstName = "FN2", LastName = "Ln2" };
                var user3 = new User() { BirthDate = DateTime.Today, FirstName = "FN3", LastName = "Ln3" };
                hs.Add(user);
                hs.Add(user2);
                hs.Add(user3);
                var service = new UserService(false, hs);
                Task.Run(() => ServerForSlaves(service));
                Task.Run(() => ServerForClients(service));
            }
            Console.ReadKey();
        }

        public static void ServerForSlaves(UserService service)
        {
            Console.WriteLine($"Server for slaves is running on {Thread.CurrentThread.ManagedThreadId} - thread (ID)");
            TcpListener server = null;
            try
            {
                IPAddress serverIpAddress = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(serverIpAddress, 8888);
                server.Start();
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    service.users = (HashSet<User>)new BinaryFormatter().Deserialize(stream);
                    Console.WriteLine("slave is taken");
                    client.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            finally
            {
                server?.Stop();
            }
        }

        public static void ServerForClients(UserService service)
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} - current thread");
            TcpListener server = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, 7777);

                // запуск слушателя
                server.Start();
                byte[] data = new byte[64];
                while (true)
                {
                    Console.WriteLine("Ожидание подключений... ");

                    // получаем входящее подключение
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Подключен клиент. Выполнение запроса...");

                    // получаем сетевой поток для чтения и записи
                    NetworkStream stream = client.GetStream();

                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();

                    Console.WriteLine(message);

                    //// сообщение для отправки клиенту
                    var users = service.Search((u)=>u.FirstName == message);
                    builder.Clear();

                    foreach (var user in users)
                    {
                        builder.Append(user.ToString());
                    }

                    string response = builder.ToString();
                    //// преобразуем сообщение в массив байтов
                    data = Encoding.Unicode.GetBytes(response);

                    //// отправка сообщения
                    stream.Write(data, 0, data.Length);
                    //Console.WriteLine("Отправлено сообщение: {0}", response);
                    //// закрываем подключение

                    client.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                    server?.Stop();
            }
        }
    }
}
