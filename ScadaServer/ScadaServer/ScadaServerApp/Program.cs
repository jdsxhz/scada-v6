﻿/*
 * Copyright 2020 Mikhail Shiryaev
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : Server Application
 * Summary  : The Server cross-platform console application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Server.Engine;
using System;
using System.IO;
using System.Threading;

namespace Scada.Server.App
{
    /// <summary>
    /// The Server cross-platform console application.
    /// <para>Кросс-платформенное консольное приложение Сервера.</para>
    /// </summary>
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            // start the service
            Console.WriteLine("Starting Server...");
            Manager manager = new Manager();

            if (manager.StartService())
                Console.WriteLine("Server is started successfully");
            else
                Console.WriteLine("Server is started with errors");

            // stop the service if 'x' is pressed or a stop file exists
            Console.WriteLine("Press 'x' or create 'serverstop' file to stop Server");
            FileListener stopFileListener = new FileListener(Path.Combine(manager.AppDirs.CmdDir, "serverstop"));

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.X || stopFileListener.FileFound))
            {
                Thread.Sleep(ScadaUtils.ThreadDelay);
            }

            manager.StopService();
            stopFileListener.Terminate();
            stopFileListener.DeleteFile();
            Console.WriteLine("Server is stopped");
        }
    }
}
