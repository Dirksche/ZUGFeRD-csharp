/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 * 
 *   http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace s2industries.ZUGFeRD.Test
{
    public class TestBase
    {
        protected string _makeSurePathIsCrossPlatformCompatible(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return path;
            }

            return path.Replace('\\', System.IO.Path.DirectorySeparatorChar);
        } // !_makeSurePathIsCrossPlatformCompatible()


        /// <summary>
        /// Asserts that two strings are equal, using an ordinal comparison, and suppresses detailed failure output.
        /// </summary>
        /// <remarks>This method performs a case-sensitive, culture-invariant comparison of the two
        /// strings. If the strings are not equal, the provided <paramref name="message"/> is displayed in the assertion
        /// failure.</remarks>
        /// <param name="expected">The expected string value.</param>
        /// <param name="actual">The actual string value to compare against the expected value.</param>
        /// <param name="message">The message to display if the assertion fails.</param>
        protected void AssertEqualSuppressDetails(string expected, string actual, string message)
        {
            if (!string.Equals(expected, actual, StringComparison.Ordinal))
            {
                Assert.Fail(message);
            }
        }
    }
}
