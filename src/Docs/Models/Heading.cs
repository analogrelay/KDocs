using System.Collections.Generic;

namespace Docs.Models
{
    public class Heading
    {
        public string Id { get; }
        public string Title { get; }
        public IEnumerable<Heading> Subheadings { get; }

        public Heading(string id, string title, IEnumerable<Heading> subheadings)
        {
            Id = id;
            Title = title;
            Subheadings = subheadings;
        }
    }
}