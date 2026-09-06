// Each fixture owns its own container and store, so the three database suites are safe to run side by side and
// their container startups overlap instead of stacking up.
[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(3)]
