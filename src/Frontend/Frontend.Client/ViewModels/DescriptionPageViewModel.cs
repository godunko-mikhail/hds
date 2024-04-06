namespace Frontend.Client.ViewModels;

public class DescriptionPageViewModel
{
    public string PageTitle { get; set; } = null!;
    public PageHead HeadSection { get; set; } = null!;

    public Segment[] MainSection { get; set; } = null!;
    
    public Segment[]? ArticlesSection { get; set; }

    public class PageHead
    {
        public string Header { get; init; } = null!;
        public string? Description { get; init; } = null!;
    }
    
    public class Segment
    {
        public string Header { get; set; } = null!;
        public string? Link { get; set; } = null!;
        public ICollection<Paragraph> Paragraphs { get; set; } = null!;
    }

    public class Paragraph
    {
        public string Text { get; set; } = null!;
        public string Link { get; set; } = null!;
        public string? ReadMoreLink { get; set; } 
    }
}