﻿/* Copyright 2013-2014 MongoDB Inc.
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

using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Core.Misc;
using MongoDB.Driver.Core.WireProtocol.Messages.Encoders;

namespace MongoDB.Driver.Core.WireProtocol
{
    public class QueryWireProtocolArgs<TDocument>
    {
        // fields
        private readonly bool _awaitData;
        private readonly int _batchSize;
        private readonly CollectionNamespace _collectionNamespace;
        private readonly MessageEncoderSettings _messageEncoderSettings;
        private readonly BsonDocument _fields;
        private readonly bool _noCursorTimeout;
        private readonly bool _partialOk;
        private readonly BsonDocument _query;
        private readonly IElementNameValidator _queryValidator;
        private readonly IBsonSerializer<TDocument> _serializer;
        private readonly int _skip;
        private readonly bool _slaveOk;
        private readonly bool _tailableCursor;

        // constructors
        public QueryWireProtocolArgs(
            CollectionNamespace collectionNamespace,
            BsonDocument query,
            BsonDocument fields,
            IElementNameValidator queryValidator,
            int skip,
            int batchSize,
            bool slaveOk,
            bool partialOk,
            bool noCursorTimeout,
            bool tailableCursor,
            bool awaitData,
            IBsonSerializer<TDocument> serializer,
            MessageEncoderSettings messageEncoderSettings)
        {
            _collectionNamespace = Ensure.IsNotNull(collectionNamespace, "collectionNamespace");
            _query = Ensure.IsNotNull(query, "query");
            _fields = fields; // can be null
            _queryValidator = Ensure.IsNotNull(queryValidator, "queryValidator");
            _skip = Ensure.IsGreaterThanOrEqualToZero(skip, "skip");
            _batchSize = batchSize; // can be negative
            _slaveOk = slaveOk;
            _partialOk = partialOk;
            _noCursorTimeout = noCursorTimeout;
            _tailableCursor = tailableCursor;
            _awaitData = awaitData;
            _serializer = Ensure.IsNotNull(serializer, "serializer");
            _messageEncoderSettings = messageEncoderSettings;
        }

        // properties
        public bool AwaitData
        {
            get { return _awaitData; }
        }

        public int BatchSize
        {
            get { return _batchSize; }
        }

        public CollectionNamespace CollectionNamespace
        {
            get { return _collectionNamespace; }
        }

        public MessageEncoderSettings MessageEncoderSettings
        {
            get { return _messageEncoderSettings; }
        }

        public BsonDocument Fields
        {
            get { return _fields; }
        }

        public bool NoCursorTimeout
        {
            get { return _noCursorTimeout; }
        }

        public bool PartialOk
        {
            get { return _partialOk; }
        }

        public BsonDocument Query
        {
            get { return _query; }
        }

        public IElementNameValidator QueryValidator
        {
            get { return _queryValidator; }
        }

        public IBsonSerializer<TDocument> Serializer
        {
            get { return _serializer; }
        }

        public int Skip
        {
            get { return _skip; }
        }

        public bool SlaveOk
        {
            get { return _slaveOk; }
        }

        public bool TailableCursor
        {
            get { return _tailableCursor; }
        }
    }
}