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
 * Module   : Log
 * Summary  : Provides logging functionality
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2020
 */

using System;

namespace Scada.Log
{
    /// <summary>
    /// Provides logging functionality.
    /// <para>Обеспечивает функциональность логгирования.</para>
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Writes action of the specified type to the log.
        /// </summary>
        void WriteAction(string text, LogActType actType);

        /// <summary>
        /// Writes the informational message to the log.
        /// </summary>
        void WriteInfo(string text);

        /// <summary>
        /// Writes the action to the log.
        /// </summary>
        void WriteAction(string text);

        /// <summary>
        /// Writes the error to the log.
        /// </summary>
        void WriteError(string text);

        /// <summary>
        /// Writes the exception to the log.
        /// </summary>
        void WriteException(Exception ex, string errMsg = "", params object[] args);

        /// <summary>
        /// Writes the specified line to the log.
        /// </summary>
        void WriteLine(string text = "");

        /// <summary>
        /// Writes a divider to the log.
        /// </summary>
        void WriteBreak();
    }
}