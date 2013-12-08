﻿#region Copyright (c) 2013 S. van Deursen
/* The Simple Injector is an easy-to-use Inversion of Control library for .NET
 * 
 * Copyright (C) 2013 S. van Deursen
 * 
 * To contact me, please visit my blog at http://www.cuttingedge.it/blogs/steven/ or mail to steven at 
 * cuttingedge.it.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
 * associated documentation files (the "Software"), to deal in the Software without restriction, including 
 * without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 * copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the 
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial 
 * portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT 
 * LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO 
 * EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER 
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE 
 * USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion

namespace SimpleInjector.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using SimpleInjector.Advanced;
    using SimpleInjector.Diagnostics.Debugger;

    /// <summary>
    /// Diagnostic result that warns about a
    /// component that depends on an unregistered concrete type and this concrete type has a lifestyle that is 
    /// different than the lifestyle of an explicitly registered type that uses this concrete type as its 
    /// implementation.
    /// For more information, see: https://simpleinjector.codeplex.com/wikipage?title=ShortCircuitedDependencies.
    /// </summary>
    public class ShortCircuitedDependencyDiagnosticResult : DiagnosticResult
    {
        internal ShortCircuitedDependencyDiagnosticResult(Type serviceType, string description,
            InstanceProducer registration, KnownRelationship relationship,
            IEnumerable<InstanceProducer> expectedDependencies)
            : base(serviceType, description, DiagnosticType.ShortCircuitedDependency,
                CreateDebugValue(registration, relationship, expectedDependencies.ToArray()))
        {
            this.Relationship = relationship;
            this.ExpectedDependencies = new ReadOnlyCollection<InstanceProducer>(expectedDependencies.ToList());
        }

        /// <summary>Gets the instance that describes the current relationship between the checked component
        /// and the short-circuited dependency.</summary>
        /// <value>The <see cref="KnownRelationship"/>.</value>
        public KnownRelationship Relationship { get; private set; }

        /// <summary>
        /// Gets the collection of registrations that have the component's current dependency as 
        /// implementation type, but have a lifestyle that is different than the current dependency.
        /// </summary>
        /// <value>A collection of <see cref="InstanceProducer"/> instances.</value>
        public ReadOnlyCollection<InstanceProducer> ExpectedDependencies { get; private set; }

        private static DebuggerViewItem[] CreateDebugValue(InstanceProducer registration,
            KnownRelationship actualDependency, 
            InstanceProducer[] possibleSkippedRegistrations)
        {
            return new[]
            {
                new DebuggerViewItem(
                    name: "Registration", 
                    description: registration.ServiceType.ToFriendlyName(), 
                    value: registration),
                new DebuggerViewItem(
                    name: "Actual Dependency", 
                    description: actualDependency.Dependency.ServiceType.ToFriendlyName(), 
                    value: actualDependency),
                new DebuggerViewItem(
                    name: "Expected Dependency", 
                    description: possibleSkippedRegistrations.First().ServiceType.ToFriendlyName(),
                    value: possibleSkippedRegistrations.Length == 1 ? 
                        (object)possibleSkippedRegistrations[0] : 
                        possibleSkippedRegistrations),
            };
        }
    }
}