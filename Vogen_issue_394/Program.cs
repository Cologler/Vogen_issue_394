using Vogen;
using System.Diagnostics;
using Vogen_issue_394;

[assembly: VogenDefaults(debuggerAttributes: DebuggerAttributeGeneration.Basic)]

try
{
    addAndSave(10);
    addAndSave(10);

    printItems();
}
catch (ValueObjectValidationException)
{
    // should not throw for unset id
    Debug.Assert(true);
}

static void addAndSave(int amount)
{
    using var context = new SomeDbContext();

    for (int i = 0; i < amount; i++)
    {
        var entity = new SomeEntity
        {
            Name = Name.From("Fred # " + i),
            Age = Age.From(42 + i)
        };

        context.SomeEntities.Add(entity);
    }

    context.SaveChanges();
}

static void printItems()
{
    using var ctx = new SomeDbContext();

    var entities = ctx.SomeEntities.ToList();
    Console.WriteLine(string.Join(Environment.NewLine, entities.Select(e => $"{e.Id.Value} {e.Name} {e.Age}")));

    Console.WriteLine("Done");
}

