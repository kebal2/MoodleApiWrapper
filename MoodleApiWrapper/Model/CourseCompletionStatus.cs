using System.Collections.Generic;

namespace MoodleApiWrapper.Model;

public class CourseCompletionStatus
{
    public CompletionStatus completionstatus { get; set; } = null!;
    public List<Warning> warnings { get; set; } = null!;
}

public class CompletionStatus
{
    public bool completed { get; set; }
    public int aggregation { get; set; }
    public List<Completion> completions { get; set; } = null!;
}

public class Completion
{
    public int type { get; set; }
    public string title { get; set; } = null!;
    public string status { get; set; } = null!;
    public bool complete { get; set; }
    public int? timecompleted { get; set; } = null!;
    public CompletionDetails details { get; set; } = null!;
}

public class CompletionDetails
{
    public string type { get; set; } = null!;
    public string criteria { get; set; } = null!;
    public string requirement { get; set; } = null!;
    public string status { get; set; } = null!;
}