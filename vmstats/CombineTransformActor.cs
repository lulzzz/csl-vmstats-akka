﻿using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Event;
using vmstats;
using static vmstats.Messages;

namespace vmstats
{
    public class CombineTransformActor : BaseTransformActor
    {
        public static readonly string TRANSFORM_NAME = "COM";
        public static readonly string TRANSFORM_NAME_CONCATENATOR = ":";
        public static readonly string TRANSFORM_COLLECTION_START = "(";
        public static readonly string TRANSFORM_COLLECTION_END = ")";
        public static readonly string TRANSFORM_COLLECTION_CONTATENATOR = "+";

        
        // Parameters for the transform
        // TODO plumb the DSL parser to create a parameter using the following name that contains the number of transforms in the combine
        public static readonly string COUNT = "C";

        // The store to hold the transforms in pending receiving all of them to be be combined
        Dictionary<Guid, List<TransformSeries>> TransformSereiesHoldingStore = new Dictionary<Guid, List<TransformSeries>>();

        public CombineTransformActor()
        {
            Receive<TransformSeries>(msg => ProcessRequest(msg));
        }


        private void ProcessRequest(TransformSeries msg)
        {
            _log.Debug($"Received transform series for combining. GroupID: {msg.GroupID}");

            // Check to see if there are any transforms already received and stored which have the same ID
            if (TransformSereiesHoldingStore.ContainsKey(msg.GroupID))
            {
                _log.Debug($"Already have some transforms for GroupID: {msg.GroupID}");

                // There are some transforms with the same id. Check to see if all of them have been received.
                var numExpected = Convert.ToInt32(msg.Transforms.Dequeue().Parameters[COUNT]);
                var storedTransforms = TransformSereiesHoldingStore[msg.GroupID];

                if (storedTransforms.Count == numExpected - 1)
                {
                    _log.Debug($"All transforms now received for GroupID: {msg.GroupID}. Combining the metrics from each and then routing.");

                    // Enough transforms have been received so combine them
                    storedTransforms.Add(msg);
                    var metric = Combine(storedTransforms);

                    // Route the result of the combine transform
                    var series = new TransformSeries(metric, msg.Transforms, msg.GroupID, msg.ConnectionId);
                    RouteTransform(series);
                }
                else
                {
                    _log.Debug($"Still waiting for some transforms to be received, storing received transforms with others. GroupID: {msg.GroupID}");
 
                    // Still waiting for some of the metrics in the combine to be received so store this one
                    storedTransforms.Add(msg);
                }
            }
            else
            {
                _log.Debug($"This is the first transform with GroupID: {msg.GroupID}. Storing it and awaiting the rest.");

                // There are no entries for this TransformSeries, this is the first one
                var list = new List<TransformSeries>();
                list.Add(msg);
            }

        }


        private Metric Combine(List<TransformSeries> transforms)
        {
            var combinedValues = new SortedDictionary<long, float>();

            // Combine each metric with the others
            foreach (var transform in transforms)
            {
                foreach (KeyValuePair<long, float> entry in transform.Measurements.Values)
                {
                    if (combinedValues.ContainsKey(entry.Key))
                    {
                        // Value already exists so add this value to it and store
                        var val = combinedValues[entry.Key];
                        val += entry.Value;
                        combinedValues.Remove(entry.Key);
                        combinedValues.Add(entry.Key, val);
                    }
                    else
                    {
                        // No entry exists so just add this value
                        combinedValues.Add(entry.Key, entry.Value);
                    }
                }
            }

            // Create the new name for the metric by combining the names from each transform
            var sb = new System.Text.StringBuilder();
            sb.Append(TRANSFORM_COLLECTION_START);
            var first = true;
            foreach (var transform in transforms)
            {
                if (!first)
                {
                    sb.Append(TRANSFORM_COLLECTION_CONTATENATOR);
                }
                else
                {
                    first = false;
                }
                sb.Append(transform.Measurements.Name);
            }
            sb.Append(TRANSFORM_COLLECTION_END);
            sb.Append(TRANSFORM_NAME_CONCATENATOR);
            sb.Append(TRANSFORM_NAME);

            // Create the new Metric from the combined values and return it to the caller
            var metric = new Metric(sb.ToString(), combinedValues);
            return metric;
        }
    }
}




