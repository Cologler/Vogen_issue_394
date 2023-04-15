// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

using Vogen_issue_394;

try
{
    using var ctx = new SomeDbContext();
    ctx.SomeEntities.Add(new());
    ctx.SaveChanges();
    Debug.Assert(false);
}
catch (Vogen.ValueObjectValidationException)
{
    // should not throw for unset id
    Debug.Assert(true);
}

try
{
    using var ctx = new SomeDbContext();
    ctx.SomeEntities.Add(new SomeEntity
    {
        SomeId = SomeId.Unset
    });
    ctx.SomeEntities.Add(new SomeEntity
    {
        SomeId = SomeId.Unset
    });
    ctx.SaveChanges();
    Debug.Assert(false);
}
catch (InvalidOperationException)
{
    // should allow add entity
    Debug.Assert(true);
}

Debug.Assert(true);

