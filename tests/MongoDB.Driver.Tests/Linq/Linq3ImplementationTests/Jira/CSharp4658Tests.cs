﻿/* Copyright 2010-present MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System.Linq;
using MongoDB.Driver.Linq;
using MongoDB.TestHelpers.XunitExtensions;
using Xunit;

namespace MongoDB.Driver.Tests.Linq.Linq3ImplementationTests.Jira
{
    public class CSharp4658Tests : Linq3IntegrationTest
    {
        [Theory]
        [ParameterAttributeData]
        public void Select_new_Model_should_work(
            [Values(LinqProvider.V2, LinqProvider.V3)] LinqProvider linqProvider)
        {
            var collection = GetCollection(linqProvider);

            var queryable = collection.AsQueryable()
                .GroupBy(x => x.Name)
                .Select(x =>
                    new Model
                    {
                        NotId = string.Empty,
                        Name = x.Key
                    });

            var stages = Translate(collection, queryable);
            AssertStages(
                stages,
                "{ $group : { _id : '$Name' } }",
                "{ $project : { NotId : '', Name : '$_id', _id : 0 } }");
        }

        [Theory]
        [ParameterAttributeData]
        public void Project_new_ModelAggregated_should_work(
            [Values(LinqProvider.V2, LinqProvider.V3)] LinqProvider linqProvider)
        {
            var collection = GetCollection(linqProvider);

            var aggregate = collection.Aggregate()
                .Group(
                    x => "",
                    g => new { Count = g.Count()}
                )
                .Project(
                    x => new ModelAggregated
                    {
                        NotId = string.Empty,
                        Count = x.Count
                    });

            var stages = Translate(collection, aggregate);
            if (linqProvider == LinqProvider.V2)
            {
                AssertStages(
                    stages,
                    "{ $group : { _id : '', Count : { $sum : 1 } } }",
                    "{ $project : { NotId : '', Count : '$Count', _id : 0 } }");
            }
            else
            {
                AssertStages(
                    stages,
                    "{ $group : { _id : '', __agg0 : { $sum : 1 } } }",
                    "{ $project : { Count : '$__agg0', _id : 0 } }",
                    "{ $project : { NotId : '', Count : '$Count', _id : 0 } }");
            }
        }

        private IMongoCollection<Model> GetCollection(LinqProvider linqProvider)
        {
            var collection = GetCollection<Model>("test", linqProvider);
            return collection;
        }

        private class Model
        {
            public string NotId { get; set; }
            public string Name { get; set; }
        }

        private class ModelAggregated
        {
            public string NotId { get; set; }
            public int Count { get; set; }
        }
    }
}
