using System.Collections.Generic;

namespace MoodleApiWrapper.Model;

public class CourseCompletionStatus
{
    public CompletionStatus completionstatus { get; set; }
    public List<Warning> warnings { get; set; }
}

public class CompletionStatus
{
    public bool completed { get; set; }
    public int aggregation { get; set; }
    public List<Completion> completions { get; set; }
}

public class Completion
{
    public int type { get; set; }
    public string title { get; set; }
    public string status { get; set; }
    public bool complete { get; set; }
    public int timecompleted { get; set; }
    public CompletionDetails details { get; set; }
}

public class CompletionDetails
{
    public string type { get; set; }
    public string criteria { get; set; }
    public string requirement { get; set; }
    public string status { get; set; }
}
