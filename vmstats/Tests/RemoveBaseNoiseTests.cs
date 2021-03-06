﻿using System.Collections.Generic;
using NUnit.Framework;
using Akka.TestKit.NUnit3;
using Akka.Actor;
using vmstats;
using Akka.Configuration;
using Newtonsoft.Json;

namespace vmstats.Tests
{
    [TestFixture] //using NUnit
    class RemoveBaseNoiseTests : TestKit
    {
        /*
        static void Main(string[] args)
        {
            var test = new RemoveBaseNoiseTests();

            // Get the configuration of the akka system
            var config = ConfigurationFactory.ParseString(GetConfiguration());

            // Create the container for all the actors
            var sys = ActorSystem.Create("vmstats-test", config);

            var msg = test.generateTestData_When_AllValuesAreNoise_Expect_AllReturnValuesSetToBaseNoise();

            // Create the actor and send it the data to be transformed
            var actor = sys.ActorOf(Props.Create(() => new RemoveBaseNoiseActor()));
            actor.Tell(msg);

            // Wait for the actor system to terminate so we have time to debug things
            sys.WhenTerminated.Wait();
        }

        [Test]
        public void When_AllValuesAreNoise_Expect_AllReturnValuesSetToBaseNoise()
        {
            var msg = generateTestData_When_AllValuesAreNoise_Expect_AllReturnValuesSetToBaseNoise();

            // Create the actor and send it the data to be transformed
            var actor = Sys.ActorOf(Props.Create(() => new RemoveBaseNoiseActor()));
            actor.Tell(msg);
            var result = ExpectMsg<Result>();

            // Create the expected results
            var expectedResult = generateResults_When_AllValuesAreNoise_Expect_AllReturnValuesSetToBaseNoise();

            // Check expected results match real results
            CollectionAssert.AreEqual(expectedResult.Values, result.Measurements.Values,
                "Resuls and Expected values are different.\nExpected values = {0}\nActual result values = {1}. ",
                new object[] { JsonConvert.SerializeObject(expectedResult.Values), JsonConvert.SerializeObject(result.Measurements.Values) }
                );
            Assert.AreEqual(expectedResult.Name, result.Measurements.Name);
        }

        public Transform generateTestData_When_AllValuesAreNoise_Expect_AllReturnValuesSetToBaseNoise()
        {
            // Create the data for the test
            var d = new SortedDictionary<long, float>();
            for (int x = 1; x <= 20; x++)
            {
                d.Add(x, 1.0F);
            }
            var metric = new Metric("When_AllValuesAreNoise_Expect_AllReturnValuesSetToBaseNoise", d);
            return new Transform(metric);

        }

        public Metric generateResults_When_AllValuesAreNoise_Expect_AllReturnValuesSetToBaseNoise()
        {
            // Create the data for the expected result
            var d = new SortedDictionary<long, float>();
            for (int x = 1; x <= 20; x++)
            {
                d.Add(x, 0.0F);
            }

            return new Metric("When_AllValuesAreNoise_Expect_AllReturnValuesSetToBaseNoise:RBN", d);

        }

        [Test]
        public void When_SomeNoisePresent_Expect_AllBaseNoiseIsRemoved()
        {
            // Create the data for the test
            var msg = generateTestData_When_SomeNoisePresent_Expect_AllBaseNoiseIsRemoved();

            // Perform the test
            var actor = Sys.ActorOf(Props.Create(() => new RemoveBaseNoiseActor()));
            actor.Tell(msg);
            var result = ExpectMsg<Result>();

            // Create the expected results
            var expectedResult = generateResults_When_SomeNoisePresent_Expect_AllBaseNoiseIsRemoved();

            // Check results
            CollectionAssert.AreEqual(expectedResult.Values, result.Measurements.Values,
                "Resuls and Expected values are different.\nExpected values = {0}\nActual result values = {1}. ",
                new object[] { JsonConvert.SerializeObject(expectedResult.Values), JsonConvert.SerializeObject(result.Measurements.Values) }
                );
            Assert.AreEqual(expectedResult.Name, result.Measurements.Name);
        }

        public Transform generateTestData_When_SomeNoisePresent_Expect_AllBaseNoiseIsRemoved()
        {
            // Create the data for the test
            var d = new SortedDictionary<long, float>();
            d.Add(1, 1.0F);
            d.Add(2, 2.0F);
            d.Add(3, 3.0F);
            d.Add(4, 4.0F);
            d.Add(5, 5.0F);
            d.Add(6, 6.0F);
            d.Add(7, 7.0F);
            d.Add(8, 8.0F);
            d.Add(9, 9.0F);
            d.Add(10, 10.0F);
            d.Add(11, 1.0F);
            d.Add(12, 1.0F);
            d.Add(13, 1.0F);
            d.Add(14, 1.0F);
            d.Add(15, 1.0F);
            d.Add(16, 1.0F);
            d.Add(17, 1.0F);
            d.Add(18, 1.0F);
            d.Add(19, 1.0F);
            d.Add(20, 1.0F);
            var metric = new Metric("When_SomeNoisePresent_Expect_AllBaseNoiseIsRemoved", d);
            return new Transform(metric);
        }

        public Metric generateResults_When_SomeNoisePresent_Expect_AllBaseNoiseIsRemoved()
        {
            // Create the expected result
            var d = new SortedDictionary<long, float>();
            d.Add(1, 0.0F);
            d.Add(2, 1.0F);
            d.Add(3, 2.0F);
            d.Add(4, 3.0F);
            d.Add(5, 4.0F);
            d.Add(6, 5.0F);
            d.Add(7, 6.0F);
            d.Add(8, 7.0F);
            d.Add(9, 8.0F);
            d.Add(10, 9.0F);
            d.Add(11, 0.0F);
            d.Add(12, 0.0F);
            d.Add(13, 0.0F);
            d.Add(14, 0.0F);
            d.Add(15, 0.0F);
            d.Add(16, 0.0F);
            d.Add(17, 0.0F);
            d.Add(18, 0.0F);
            d.Add(19, 0.0F);
            d.Add(20, 0.0F);
            return new Metric("When_SomeNoisePresent_Expect_AllBaseNoiseIsRemoved:RBN", d);
        }


        [Test]
        public void When_SomeNoisePresentAndDefaultRollingAvgChanged_Expect_AllBaseNoiseIsRemoved()
        {
            // Create the data for the test
            var msg = generateTestData_When_SomeNoisePresentAndDefaultRollingAvgChanged_Expect_AllBaseNoiseIsRemoved();

            // Perform the test
            var actor = Sys.ActorOf(Props.Create(() => new RemoveBaseNoiseActor()));
            actor.Tell(msg);
            var result = ExpectMsg<Result>();

            // Create the expected results
            var expectedResult = generateResults_When_SomeNoisePresentAndDefaultRollingAvgChanged_Expect_AllBaseNoiseIsRemoved();

            // Check results
            CollectionAssert.AreEqual(expectedResult.Values, result.Measurements.Values,
                "Resuls and Expected values are different.\nExpected values = {0}\nActual result values = {1}. ",
                new object[] { JsonConvert.SerializeObject(expectedResult.Values), JsonConvert.SerializeObject(result.Measurements.Values) }
                );
            Assert.AreEqual(expectedResult.Name, result.Measurements.Name);
        }

        public Transform generateTestData_When_SomeNoisePresentAndDefaultRollingAvgChanged_Expect_AllBaseNoiseIsRemoved()
        {
            // Create the data for the test
            var d = new SortedDictionary<long, float>();
            d.Add(1, 1.0F);
            d.Add(2, 2.0F);
            d.Add(3, 3.0F);
            d.Add(4, 4.0F);
            d.Add(5, 5.0F);
            d.Add(6, 6.0F);
            d.Add(7, 7.0F);
            d.Add(8, 8.0F);
            d.Add(9, 9.0F);
            d.Add(10, 10.0F);
            d.Add(11, 1.0F);
            d.Add(12, 1.0F);
            d.Add(13, 100.0F);
            d.Add(14, 1.0F);
            d.Add(15, 1.0F);
            d.Add(16, 1.0F);
            d.Add(17, 1.0F);
            d.Add(18, 180.0F);
            d.Add(19, 1.0F);
            d.Add(20, 0.0F);
            var metric = new Metric("When_SomeNoisePresentAndDefaultRollingAvgChanged_Expect_AllBaseNoiseIsRemoved", d);

            // Create the poarameter to oiverride the rolling everage length
            var p = new Dictionary<string, string>();
            p.Add(RemoveBaseNoiseActor.ROLLING_AVG_LENGTH, "4");

            return new Transform(metric, p);
        }

        public Metric generateResults_When_SomeNoisePresentAndDefaultRollingAvgChanged_Expect_AllBaseNoiseIsRemoved()
        {
            // Create the expected result
            var d = new SortedDictionary<long, float>();
            d.Add(1, 0.0F);
            d.Add(2, 1.0F);
            d.Add(3, 2.0F);
            d.Add(4, 3.0F);
            d.Add(5, 4.0F);
            d.Add(6, 5.0F);
            d.Add(7, 6.0F);
            d.Add(8, 7.0F);
            d.Add(9, 8.0F);
            d.Add(10, 9.0F);
            d.Add(11, 0.0F);
            d.Add(12, 0.0F);
            d.Add(13, 99.0F);
            d.Add(14, 0.0F);
            d.Add(15, 0.0F);
            d.Add(16, 0.0F);
            d.Add(17, 0.0F);
            d.Add(18, 179.0F);
            d.Add(19, 0.0F);
            d.Add(20, 0.0F);
            return new Metric("When_SomeNoisePresentAndDefaultRollingAvgChanged_Expect_AllBaseNoiseIsRemoved:RBN", d);
        }



        [Test]
        public void When_VarietyOfNoisePresentAndDefaultRollingAvgChanged_Expect_AllBaseNoiseIsRemoved()
        {
            // Create the data for the test
            var metric = generateTestData_When_VarietyOfNoisePresentAndDefaultRollingAvgChanged_Expect_AllBaseNoiseIsRemoved();

            // Create the actor
            var actor = Sys.ActorOf(Props.Create(() => new RemoveBaseNoiseActor()));

            // Rolling Avg Length = 5, Avg should be 1
            var p = new Dictionary<string, string>();
            p.Add(RemoveBaseNoiseActor.ROLLING_AVG_LENGTH, "5");
            var msg = new Transform(metric, p);
            actor.Tell(msg);
            var result = ExpectMsg<Result>();

            // Create the expected results
            var expectedResult = generateResults_When_VarietyOfNoisePresentAndDefaultRollingAvgChanged_Expect_AllBaseNoiseIsRemoved_5();

            // Check results
            CollectionAssert.AreEqual(expectedResult.Values, result.Measurements.Values,
                "Resuls and Expected values are different.\nExpected values = {0}\nActual result values = {1}. ",
                new object[] { JsonConvert.SerializeObject(expectedResult.Values), JsonConvert.SerializeObject(result.Measurements.Values) }
                );
            Assert.AreEqual(expectedResult.Name, result.Measurements.Name);

            // Rolling Avg Length = 5, Avg should be 2
            p = new Dictionary<string, string>();
            p.Add(RemoveBaseNoiseActor.ROLLING_AVG_LENGTH, "6");
            msg = new Transform(metric, p);
            actor.Tell(msg);
            result = ExpectMsg<Result>();

            // Create the expected results
            expectedResult = generateResults_When_VarietyOfNoisePresentAndDefaultRollingAvgChanged_Expect_AllBaseNoiseIsRemoved_6();

            // Check results
            CollectionAssert.AreEqual(expectedResult.Values, result.Measurements.Values,
                "Resuls and Expected values are different.\nExpected values = {0}\nActual result values = {1}. ",
                new object[] { JsonConvert.SerializeObject(expectedResult.Values), JsonConvert.SerializeObject(result.Measurements.Values) }
                );
            Assert.AreEqual(expectedResult.Name, result.Measurements.Name);

        }

        public Metric generateTestData_When_VarietyOfNoisePresentAndDefaultRollingAvgChanged_Expect_AllBaseNoiseIsRemoved()
        {
            // Create the data for the test
            var d = new SortedDictionary<long, float>();
            d.Add(1, 1.0F);
            d.Add(2, 1.0F);
            d.Add(3, 7.0F);
            d.Add(4, 1.0F);
            d.Add(5, 1.0F);
            d.Add(6, 1.0F);
            d.Add(7, 9.0F);
            d.Add(8, 1.0F);
            d.Add(9, 1.0F);
            d.Add(10, 1.0F);
            d.Add(11, 1.0F);
            d.Add(12, 10.0F);
            d.Add(13, 1.0F);
            d.Add(14, 1.0F);
            d.Add(15, 1.0F);
            d.Add(16, 1.0F);
            d.Add(17, 1.0F);
            d.Add(18, 20.0F);
            d.Add(19, 21.0F);
            d.Add(20, 1.0F);
            return new Metric("When_VarietyOfNoisePresentAndDefaultRollingAvgChanged_Expect_AllBaseNoiseIsRemoved", d);
        }

        public Metric generateResults_When_VarietyOfNoisePresentAndDefaultRollingAvgChanged_Expect_AllBaseNoiseIsRemoved_5()
        {
            // Create the expected result
            var d = new SortedDictionary<long, float>();
            d.Add(1, 0.0F);
            d.Add(2, 0.0F);
            d.Add(3, 6.0F);
            d.Add(4, 0.0F);
            d.Add(5, 0.0F);
            d.Add(6, 0.0F);
            d.Add(7, 8.0F);
            d.Add(8, 0.0F);
            d.Add(9, 0.0F);
            d.Add(10, 0.0F);
            d.Add(11, 0.0F);
            d.Add(12, 9.0F);
            d.Add(13, 0.0F);
            d.Add(14, 0.0F);
            d.Add(15, 0.0F);
            d.Add(16, 0.0F);
            d.Add(17, 0.0F);
            d.Add(18, 19.0F);
            d.Add(19, 20.0F);
            d.Add(20, 0.0F);
            return new Metric("When_VarietyOfNoisePresentAndDefaultRollingAvgChanged_Expect_AllBaseNoiseIsRemoved:RBN", d);
        }

        public Metric generateResults_When_VarietyOfNoisePresentAndDefaultRollingAvgChanged_Expect_AllBaseNoiseIsRemoved_6()
        {
            // Create the expected result
            var d = new SortedDictionary<long, float>();
            d.Add(1, 0.0F);
            d.Add(2, 0.0F);
            d.Add(3, 5.0F);
            d.Add(4, 0.0F);
            d.Add(5, 0.0F);
            d.Add(6, 0.0F);
            d.Add(7, 7.0F);
            d.Add(8, 0.0F);
            d.Add(9, 0.0F);
            d.Add(10, 0.0F);
            d.Add(11, 0.0F);
            d.Add(12, 8.0F);
            d.Add(13, 0.0F);
            d.Add(14, 0.0F);
            d.Add(15, 0.0F);
            d.Add(16, 0.0F);
            d.Add(17, 0.0F);
            d.Add(18, 18.0F);
            d.Add(19, 19.0F);
            d.Add(20, 0.0F);
            return new Metric("When_VarietyOfNoisePresentAndDefaultRollingAvgChanged_Expect_AllBaseNoiseIsRemoved:RBN", d);
        }








        private static string GetConfiguration()
        {
            string config = @"
                akka {  
                    stdout-loglevel = DEBUG
                    loglevel = DEBUG
                    loggers = [""Akka.Logger.NLog.NLogLogger, Akka.Logger.NLog""]
                    log-config-on-start = on

                    actor
                    {
                      debug
                      {
                        receive = on      # log any received message
                        autoreceive = on  # log automatically received messages, e.g. PoisonPill
                        lifecycle = on    # log actor lifecycle changes
                        event-stream = on # log subscription changes for Akka.NET event stream
                        unhandled = on    # log unhandled messages sent to actors
                      }
                    }
                  }
                }

            ";

            return config;
        }

*/
    }
}
