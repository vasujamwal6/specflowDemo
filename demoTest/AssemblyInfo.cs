using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.Fixtures)]  // Run feature classes in parallel
[assembly: LevelOfParallelism(1)]  // Adjust based on the number of concurrent tests you want
